using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cainos.CustomizablePixelCharacter
{
    public class PixelCharacterController : Controller
    {
        const float GROUND_CHECK_RADIUS = 0.1f;                 // radius of the overlap circle to determine if the character is on ground

        public MovementType defaultMovement = MovementType.Walk;

        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode crouchKey = KeyCode.S;
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode climbKey = KeyCode.W;
        public KeyCode moveModifierKey = KeyCode.LeftShift;
        private KeyCode interactKey = KeyCode.F;

        public KeyCode attackKey = KeyCode.Mouse0;

        public float walkSpeedMax = 2.5f;                       // max walk speed, ideally should be half of runSpeedMax
        public float walkAcc = 10.0f;                           // walking acceleration

        public float runSpeedMax = 5.0f;                        // max run speed
        public float runAcc = 10.0f;                            // running acceleration

        public float crouchSpeedMax = 1.0f;                     // max move speed while crouching
        public float crouchAcc = 8.0f;                          // crouching acceleration

        public float airSpeedMax = 2.0f;                        // max move speed while in air
        public float airAcc = 8.0f;                             // air acceleration

        public float groundBrakeAcc = 6.0f;                     // braking acceleration (from movement to still) while on ground
        public float airBrakeAcc = 1.0f;                        // braking acceleration (from movement to still) while in air

        public float jumpSpeed = 5.0f;                          // speed applied to character when jump
        public float SpeedKeep = 0f;
        public float jumpCooldown = 0.55f;                      // time to be able to jump again after landing
        public float jumpGravityMutiplier = 0.6f;               // gravity multiplier when character is jumping, should be within [0.0,1.0], set it to lower value so that the longer you press the jump button, the higher the character can jump    
        public float fallGravityMutiplier = 1.3f;               // gravity multiplier when character is falling, should be equal or greater than 1.0

        public float groundCheckRadius = 0.17f;                 // radius of the circle at the character's bottom to determine whether the character is on ground

        public float maxDistance = 15f;
        private RaycastHit2D hit2D;

        private float inputV = 0.0f;
        private float ClimbBreakAcc = 0.0f;
        private bool isLadder = false;

        [ExposeProperty]                                        // is the character dead? if dead, plays dead animation and disable control
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                fx.IsDead = isDead;
                fx.DropWeapon();
            }
        }                    
        private bool isDead;


        private PixelCharacter fx;                              // the FXCharacter script attached the character
        private CapsuleCollider2D collider2d;                   // Collider compoent on the character
        private Rigidbody2D rb2d;                               // Rigidbody2D component on the character


        private bool isGrounded;                                // is the character on ground
        private Vector2 curVel;                                 // current velocity
        private float jumpTimer;                                // timer for jump cooldown
        private Vector2 posBot;                                 // local position of the character's middle bottom
        private Vector2 posTop;                                 // local position of the character's middle top

        //user
        public PlayerCtrl pctrl;
        private bool attackAble = true;

        private Dictionary<CCType, float> CC_Dic;

        [System.NonSerialized]
        private KeyCode skillKey = KeyCode.Q;



        private void Awake()
        {
            fx = GetComponent<PixelCharacter>();
            collider2d = GetComponent<CapsuleCollider2D>();
            rb2d = GetComponent<Rigidbody2D>();
            pctrl = GetComponent<PlayerCtrl>();
            //user
            CC_Dic = new Dictionary<CCType, float>();
        }

        private void Start()
        {
            posBot = collider2d.offset - new Vector2 ( 0.0f , collider2d.size.y * 0.5f );
            posTop = collider2d.offset + new Vector2( 0.0f, collider2d.size.y * 0.5f );

            //
            CC_Dic.Add(CCType.Stun,0.0f);
            CC_Dic.Add(CCType.Snare, 0.0f);
            CC_Dic.Add(CCType.KnockDown, 0.0f);
        }

        private void Update()
        {
            if (jumpTimer < jumpCooldown) jumpTimer += Time.deltaTime;

            //RECEIVE INPUT
            bool inputCrounch = false;
            bool inputMoveModifier = false;
            bool inputJump = false;
            float inputH = 0.0f;
            float inputV = 0.0f;
            bool inputAttack = false;
            bool inputAttackContinuous = false;

            bool inputSkill = false;
            bool inputSkillContinuous = false;

            //Todo : 이거 문제요소 있음
            bool pointerOverUI = EventSystem.current && EventSystem.current.IsPointerOverGameObject();
            if (!pointerOverUI)
            {
                inputCrounch = Input.GetKey(crouchKey);
                inputMoveModifier = Input.GetKey(moveModifierKey);
                inputJump = Input.GetKey(jumpKey);
                inputAttack = Input.GetKeyDown(attackKey);
                inputAttackContinuous = Input.GetKey(attackKey);

                inputSkill = Input.GetKeyDown(skillKey);
                inputSkillContinuous = Input.GetKey(skillKey);
            }

            if (Input.GetKeyDown(interactKey))  Interact();

            if (Input.GetKey(crouchKey) && Input.GetKeyDown(jumpKey))
            {
                hit2D = Physics2D.Raycast(transform.position, Vector2.down, 1f);
                if (hit2D)
                {
                    RaycastHit2D[] hit2Ds = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - 1f), Vector2.down, 3f, LayerMask.GetMask("Obstacle"));
                    if (hit2Ds != null)
                    {
                        for (int i = 0; i < hit2Ds.Length; i++)
                        {
                            if (hit2Ds[i].transform.tag == "Floor")
                            {
                                //if (hit2Ds[i].distance <= 1.0f) break;

                                SpeedKeep = jumpSpeed;
                                jumpSpeed = 1.0f;
                                StartCoroutine(BottomJump());
                                break;
                            }
                        }
                    }
                }
            }

            if (Input.GetKey(leftKey)) inputH = -1.0f;
            else
            if (Input.GetKey(rightKey)) inputH = 1.0f;
            else inputH = 0.0f;

            bool inputRun = false;
            if (defaultMovement == MovementType.Walk && inputMoveModifier) inputRun = true;
            if (defaultMovement == MovementType.Run && !inputMoveModifier) inputRun = true;

            //Todo: cc기 추가시 이거 다시 볼것...
            if (CC_Dic[CCType.Stun]==0.0f && CC_Dic[CCType.KnockDown]==0.0f)
            {
                //PERFORM MOVE OR ATTACK BASED ON INPUT
                Attack(inputAttack, inputAttackContinuous);
                Skill(inputSkill, inputSkillContinuous);
                if (CC_Dic[CCType.Snare]==0.0f) Move(inputH, inputCrounch, inputRun, inputJump);
            }

            if (!isLadder)
            {
                isGrounded = false;
                Vector2 worldPos = transform.position;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos + posBot, groundCheckRadius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].isTrigger) continue;
                    if (colliders[i].gameObject != gameObject) isGrounded = true;
                }
            }

            //CHECK IF THE CHARACTER IS ON GROUND
            /*isGrounded = false;
            Vector2 worldPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos + posBot, groundCheckRadius);
            for (int i = 0; i < colliders.Length; i++ )
            {
                if ( colliders[i].isTrigger ) continue;
                if ( colliders[i].gameObject != gameObject ) isGrounded = true;
            }*/
        }

        public void Attack( bool inputAttack , bool inputAttackContinuous)
        {
            if (inputAttack && attackAble)
            {
                
                fx.Attack();
                pctrl.curWeapon.doAttack();
                StartCoroutine(attackDelay(pctrl.curWeapon.itsAttackSpeed));
            }
            //fx.IsAttacking = inputAttackContinuous;
        }

        public void Skill(bool inputSkill, bool inputSkillContinuous)
        {
            if (!pctrl.curWeapon.isSkillOn()) 
                return;
            if (inputSkill)
            {
                GameUI.instance.CoolTimeUI();
                pctrl.curWeapon.doSkill();
                //캐스팅 시간이 있는 스킬 경우 행동 불가능한 코드-지금은 임시방편
            }

        }

        private IEnumerator attackDelay(float time)
        {
            attackAble = false;
            yield return new WaitForSeconds(time);
            attackAble = true;
        }

        public override void CCApply(CCType cType, float time, Vector2 vec)
        {
            fx.InjuredFront();
            rb2d.velocity = vec;
            fx.MovingBlend = 0;

            if (!CC_Dic.ContainsKey(cType))
            {
                CC_Dic.Add(cType, 0.0f);
            }

            if (CC_Dic[cType] <= 0.0f)
            {
                CC_Dic[cType] = time;
                StartCoroutine(CCDelay(cType, time));
            }
            else
            {
                CC_Dic[cType] = time;
            }
            
        }

        private IEnumerator CCDelay(CCType cType, float time)
        {
            if (cType == CCType.KnockDown)
            {
                fx.IsDead = true;
            }

            while (CC_Dic[cType] > 0.0f)
            {
                yield return new WaitForFixedUpdate();
                CC_Dic[cType] -= Time.deltaTime;
                //Debug.Log("이게 맞나 싶다 : " + CC_Dic[cType]);
            }
            CC_Dic[cType] = 0.0f;
            if (cType == CCType.KnockDown)
            {
                fx.IsDead = false;
            }
            /*
            bool actAbletmp = true;
            foreach (float f in CC_Dic.Values)
            {
                actAbletmp = actAbletmp && (f == 0.0f);
            }
            actAble = actAbletmp;
            */
        }


        public void Move(float inputH, bool inputCrouch, bool inputRunning, bool inputJump)
        {
            if (isDead) return;

            //GET CURRENT SPEED FROM RIGIDBODY
            curVel = rb2d.velocity;

            //SET ACCELERATION AND MAX SPEED BASE ON CONDITION
            float acc = 0.0f;
            float max = 0.0f;
            float brakeAcc = 0.0f;

            if (isGrounded)
            {
                acc = inputRunning ? runAcc : walkAcc;
                max = inputRunning ? runSpeedMax : walkSpeedMax;
                brakeAcc = groundBrakeAcc;

                if (inputCrouch)
                {
                    acc = crouchAcc;
                    max = crouchSpeedMax;
                }
            }
            else
            {
                acc = airAcc;
                max = airSpeedMax;
                brakeAcc = airBrakeAcc;
            }


            //HANDLE HORIZONTAL MOVEMENT
            //has horizontal movement input
            if (Mathf.Abs(inputH) > 0.01f)
            {
                //if current horizontal speed is out of allowed range, let it fall to the allowed range
                bool shouldMove = true;
                if (inputH > 0 && curVel.x >= max)
                {
                    curVel.x = Mathf.MoveTowards(curVel.x, max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }
                if (inputH < 0 && curVel.x <= -max)
                {
                    curVel.x = Mathf.MoveTowards(curVel.x, -max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }

                //otherwise, add movement acceleration to cureent velocity
                if (shouldMove) curVel.x += acc * Time.deltaTime * inputH;
            }
            //no horizontal movement input, brake to speed zero
            else
            {
                curVel.x = Mathf.MoveTowards(curVel.x, 0.0f, brakeAcc * Time.deltaTime);
            }

            //JUMP
            if (isGrounded && inputJump && jumpTimer >= jumpCooldown)
            {
                isGrounded = false;
                jumpTimer = 0.0f;
                curVel.y += jumpSpeed;
            }
            if ( inputJump && curVel.y > 0)
            {
                curVel.y += Physics.gravity.y * (jumpGravityMutiplier -1.0f) * Time.deltaTime;
            }
            else if (curVel.y > 0)
            {
                curVel.y += Physics.gravity.y * ( fallGravityMutiplier - 1.0f) * Time.deltaTime;
            }


            rb2d.velocity = curVel;
            if (!isLadder)
            {
                float movingBlend = Mathf.Abs(curVel.x) / runSpeedMax;
                fx.MovingBlend = Mathf.Abs(curVel.x) / runSpeedMax;
            }
            /*float movingBlend = Mathf.Abs(curVel.x) / runSpeedMax;
            fx.MovingBlend = Mathf.Abs(curVel.x) / runSpeedMax;*/

            if (isGrounded && !isLadder) fx.IsCrouching = inputCrouch;

            if (!isLadder) fx.SpeedVertical = curVel.y;
            fx.Facing = Mathf.RoundToInt(inputH);
            fx.IsGrounded = isGrounded;
        }

        public void Interact()
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + 1), 3f, LayerMask.GetMask("Interact"));

            if(collider2D != null)
            {
                for(int i = 0; i < collider2D.Length; i++)
                {
                    if(collider2D[i].tag == "ItemBox" && pctrl.key_many > 0)
                    {
                        Debug.Log("아이템박스_상호작용");
                        pctrl.key_many--;
                        collider2D[i].transform.GetComponent<ItemBox>().BoxOpen();
                        collider2D[i].enabled = false;
                        break;
                    }
                }
            }
        }

        IEnumerator BottomJump()
        {
            pctrl.col2.isTrigger = true;
            yield return new WaitForSeconds(0.4f);
            pctrl.col2.isTrigger = false;
            jumpSpeed = SpeedKeep;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                isLadder = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                isLadder = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                if (Input.GetKey(climbKey))
                {
                    inputV = 1.0f;
                    rb2d.gravityScale = 0f;
                    fx.SpeedVertical = 0f;
                    fx.MovingBlend = 0f;
                    ClimbBreakAcc = groundBrakeAcc;
                }
                else if (Input.GetKey(crouchKey))
                {
                    inputV = -1.0f;
                    rb2d.gravityScale = 0f;
                    fx.SpeedVertical = 0f;
                    fx.MovingBlend = 0f;
                    ClimbBreakAcc = groundBrakeAcc;
                }
                else
                {
                    inputV = 0.0f;
                    ClimbBreakAcc = 0;
                }

                Climb(inputV, ClimbBreakAcc);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                if (Input.GetKey(climbKey))
                {
                    inputV = 1.0f;
                    rb2d.gravityScale = 0f;
                    fx.SpeedVertical = 0f;
                    fx.MovingBlend = 0f;
                    ClimbBreakAcc = groundBrakeAcc;
                }
                else if (Input.GetKey(crouchKey))
                {
                    inputV = -1.0f;
                    rb2d.gravityScale = 0f;
                    fx.SpeedVertical = 0f;
                    fx.MovingBlend = 0f;
                    ClimbBreakAcc = groundBrakeAcc;
                }
                else
                {
                    inputV = 0.0f;
                    ClimbBreakAcc = 0;
                }

                Climb(inputV, ClimbBreakAcc);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                isLadder = false;
                rb2d.gravityScale = 1.0f;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ladder"))
            {
                isLadder = false;
                rb2d.gravityScale = 1.0f;
            }
        }

        public void Climb(float input, float BreakAcc)
        {
            if (isDead) return;

            //GET CURRENT SPEED FROM RIGIDBODY
            curVel = rb2d.velocity;

            //SET ACCELERATION AND MAX SPEED BASE ON CONDITION
            float acc = crouchAcc;
            float max = crouchSpeedMax;
            float brakeAcc = BreakAcc;

            //HANDLE HORIZONTAL MOVEMENT
            //has horizontal movement input
            if (Mathf.Abs(input) > 0.01f)
            {
                //if current horizontal speed is out of allowed range, let it fall to the allowed range
                bool shouldMove = true;
                if (input > 0 && curVel.y >= max)
                {
                    curVel.y = Mathf.MoveTowards(curVel.y, max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }
                if (input < 0 && curVel.y <= -max)
                {
                    curVel.y = Mathf.MoveTowards(curVel.y, -max, brakeAcc * Time.deltaTime);
                    shouldMove = false;
                }

                //otherwise, add movement acceleration to cureent velocity
                if (shouldMove) curVel.y += acc * Time.deltaTime * input;
            }
            //no horizontal movement input, brake to speed zero
            else
            {
                curVel.y = Mathf.MoveTowards(curVel.y, 0.0f, brakeAcc * Time.deltaTime);
            }

            rb2d.velocity = curVel;

            float movingBlend = Mathf.Abs(curVel.y) / runSpeedMax;
            fx.MovingBlend = Mathf.Abs(curVel.y) / runSpeedMax;

            fx.SpeedVertical = 0;
            fx.Facing = Mathf.RoundToInt(input);
            fx.IsGrounded = isGrounded;
        }

        public enum MovementType
        {
            Walk,
            Run
        }

        private void OnDrawGizmosSelected()
        {
            //Draw the ground detection circle
            Gizmos.color = Color.white;
            Vector2 worldPos = transform.position;
            Gizmos.DrawWireSphere(worldPos + posBot, groundCheckRadius);
        }

    }
}



