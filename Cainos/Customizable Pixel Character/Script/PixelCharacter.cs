using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    //script used to control the character's apperance.
    //using custom editor script PixelCharacterEditor to organize variables into foldouts and expose porperties in inspector.
    public class PixelCharacter : MonoBehaviour
    {

        //reference to objects inside the character prefab
        #region OBJECTS
        public Animator animator;

        public Renderer hat;
        public Renderer hair;
        public Renderer hairClipped;
        public Renderer eye;
        public Renderer eyeBase;
        public Renderer facewear;
        public Renderer cloth;
        public Renderer skirt;
        public Renderer pants;
        public Renderer socks;
        public Renderer shoes;
        public Renderer back;
        public Renderer expression;
        public Renderer body;

        public Transform weaponSlot;
        #endregion

        //those parameters should only be changed in runtime, mainly wrappers for animator parameters
        #region RUNTIME

        //set the ramp texture of hair material
        //only use this for changing hair color in runtime
        public Texture HairRampTexture
        {
            get { return hairRampTexture; }
            set
            {
                hairRampTexture = value;
                MPBHair.SetTexture("_RampTex", hairRampTexture);

                hair.SetPropertyBlock(MPBHair);
                hairClipped.SetPropertyBlock(MPBHair);
            }
        }
        private Texture hairRampTexture;

        //the current weapon object
        public GameObject Weapon
        {
            get
            {
                if (weaponSlot.childCount <= 0) return null;
                return weaponSlot.GetChild(0).gameObject;
            }
        }

        //the character's expression
        [ExposeProperty]
        public ExpressionType Expression
        {
            get { return _expression; }
            set
            {
                _expression = value;

                animator.SetInteger("Expression", (int)_expression);
            }
        }
        [SerializeField, HideInInspector]
        private ExpressionType _expression = ExpressionType.Normal;

        [ExposeProperty]
        public AttackActionType AttackAction
        {
            get { return attackAction; }
            set
            {
                attackAction = value;

                animator.SetInteger("AttackAction", (int)attackAction);
            }
        }
        private AttackActionType attackAction = AttackActionType.Swipe;

        //character facing  1:facing right   -1:facing left
        [ExposeProperty]
        public int Facing
        {
            get { return facing; }
            set
            {
                if (value == 0) return;
                facing = value;

                animator.transform.localScale = new Vector3(1.0f, 1.0f, facing);

                Vector3 pos = animator.transform.localPosition;
                pos.x = 0.064f * -facing;
                animator.transform.localPosition = pos;
            }
        }
        [SerializeField, HideInInspector]
        private int facing = 1;

        //is the character crouching?
        [ExposeProperty]
        public bool IsCrouching
        {
            get { return isCrouching; }
            set
            {
                isCrouching = value;
                animator.SetBool("IsCrouching", isCrouching);
            }
        }
        [SerializeField, HideInInspector]
        private bool isCrouching;

        //is the character on ground?
        [ExposeProperty]
        public bool IsGrounded
        {
            get { return isGrounded; }
            set
            {
                isGrounded = value;
                animator.SetBool("IsGrounded", isGrounded);
            }
        }
        [SerializeField, HideInInspector]
        private bool isGrounded;

        //is the character dead?
        [ExposeProperty]
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                animator.SetBool("IsDead", isDead);
            }
        }
        [SerializeField, HideInInspector]
        private bool isDead;

        //is the character performing attack action?
        //works for AttackActionType.POinting and AttackActionType.Summoning
        [ExposeProperty]
        public bool IsAttacking
        {
            get { return isAttacking; }
            set
            {
                isAttacking = value;
                animator.SetBool("IsAttacking", isAttacking);
            }
        }
        [SerializeField, HideInInspector]
        private bool isAttacking;

        //moving animation blend
        //0.0:idle,  0.5:walk,  1.0:run
        [ExposeProperty]
        public float MovingBlend
        {
            get
            {
                return movingBlend;
            }
            set
            {
                movingBlend = value;
                animator.SetFloat("MovingBlend", movingBlend);
            }
        }
        [SerializeField, HideInInspector]
        private float movingBlend;

        //vertical speed
        //determines whether the animation should be jumping or falling
        public float SpeedVertical
        {
            get { return speedVertical; }
            set
            {
                speedVertical = value;
                animator.SetFloat("SpeedVertical", speedVertical);
            }
        }
        private float speedVertical;

        //when character get injured from front or back
        public void InjuredFront()
        {
            animator.SetTrigger("InjuredFront");
        }
        public void InjuredBack()
        {
            animator.SetTrigger("InjuredBack");
        }

        //perform an attack
        //works for AttackActionType.Swipe and AttackActionType.Stab
        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        //detach weapon to a seperate object from the charater
        //the weapon needs a Collider2D and Rigidbody2D component attached to it
        public void DropWeapon()
        {
            if (weaponSlot.childCount <= 0) return;
            Transform weapon = weaponSlot.GetChild(0);

            var c = weapon.GetComponent<Collider2D>();
            if (!c) return;
            c.isTrigger = false;

            var r = weapon.GetComponent<Rigidbody2D>();
            if (!r) return;
            r.bodyType = RigidbodyType2D.Dynamic;

            weapon.transform.parent = null;
        }

        //clear out everything in weapon slot
        public void ClearWeapon ()
        {
           for ( int i = 0; i < weaponSlot.childCount; i++)
            {
                var w = weaponSlot.GetChild(i);
                Destroy(w.gameObject);
            }
        }

        //instantiate a new weapon into weapon slot
        public void AddWeapon( GameObject weaponPrefab , bool clearOld = true )
        {
            if (clearOld) ClearWeapon();
            if (weaponPrefab == null) return;

            var weapon = Instantiate(weaponPrefab);
            weapon.transform.parent = weaponSlot;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }


    #endregion
  
        //parameters for tweaking the character's appearance
        #region APPEARANCE
        [ExposeProperty]
        public Material HatMaterial
        {
            get
            {
                if (!hat) return null;
                return hat.sharedMaterial;
            }
            set
            {
                if (!hat) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(hat, "Modify Hat Material");
                #endif

                hat.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material HairMaterial
        {
            get
            {
                if (!hair) return null;
                return hair.sharedMaterial;
            }
            set
            {
                if (!hair) return;
                if (!hairClipped) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Modify Hair Material");
                #endif

                hair.sharedMaterial = value;
                hairClipped.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material EyeMaterial
        {
            get
            {
                if (!eye) return null;
                return eye.sharedMaterial;
            }
            set
            {
                if (!eye) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(eye, "Modify Eye Material");
                #endif

                eye.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material EyeBaseMaterial
        {
            get
            {
                if (!eyeBase) return null;
                return eyeBase.sharedMaterial;
            }
            set
            {
                if (!eyeBase) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(eyeBase, "Modify Eye Base Material");
                #endif

                eyeBase.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material FacewearMaterial
        {
            get
            {
                if (!facewear) return null;
                return facewear.sharedMaterial;
            }
            set
            {
                if (!facewear) return;

                #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(facewear, "Modify Facewear Material");
                #endif

                facewear.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material ClothMaterial
        {
            get
            {
                if (!cloth) return null;
                return cloth.sharedMaterial;
            }
            set
            {
                if (!cloth) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(cloth, "Modify Cloth Material");
                #endif

                cloth.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material PantsMaterial
        {
            get
            {
                if (!pants) return null;
                return pants.sharedMaterial;
            }
            set
            {
                if (!pants) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { pants, skirt }, "Modify Pants Material");
                #endif

                pants.sharedMaterial = value;
                skirt.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material SocksMaterial
        {
            get
            {
                if (!socks) return null;
                return socks.sharedMaterial;
            }
            set
            {
                if (!socks) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(socks, "Modify Socks Material");
                #endif

                socks.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material ShoesMaterial
        {
            get
            {
                if (!shoes) return null;
                return shoes.sharedMaterial;
            }
            set
            {
                if (!shoes) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(shoes, "Modify Shoes Material");
                #endif

                shoes.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material BackMaterial
        {
            get
            {
                if (!back) return null;
                return back.sharedMaterial;
            }
            set
            {
                if (!back) return;

                #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(back, "Modify Back Material");
                #endif

                back.sharedMaterial = value;
            }
        }

        [ExposeProperty]
        public Material BodyMaterial
        {
            get
            {
                if (!body) return null;
                return body.sharedMaterial;
            }
            set
            {
                if (!body) return;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObject(body, "Modify Body Material");
                #endif

                body.sharedMaterial = value;
            }
        }

        //to hide part of the hair when wearing curtain hat
        [ExposeProperty]
        public bool ClipHair
        {
            get { return clipHair; }
            set
            {
                clipHair = value;

                #if UNITY_EDITOR
                    UnityEditor.Undo.RecordObjects(new Object[] { hair, hairClipped }, "Toggle Clip Hair");
                #endif

                hair.enabled = !clipHair;
                hairClipped.enabled = clipHair;
            }
        }
        [SerializeField, HideInInspector]
        private bool clipHair;

        //the interval for the character to blink, random between x and y
        public Vector2 blinkInterval = new Vector2(0.5f, 5.0f);
        #endregion


        #region OTHER

        private float blinkTimer;

        private MaterialPropertyBlock MPBHair
        {
            get
            {
                if (mpbHair == null) mpbHair = new MaterialPropertyBlock();
                return mpbHair;
            }
        }
        private MaterialPropertyBlock mpbHair;

        private void Start()
        {
            if (Application.isPlaying == false) return;

            blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
            if (blinkTimer < 0.1f) blinkTimer = 0.1f;
        }

        private void Update()
        {
            if (Application.isPlaying == false) return;

            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0.0f)
            {
                blinkTimer = Random.Range(blinkInterval.x, blinkInterval.y);
                if (blinkTimer < 0.1f) blinkTimer = 0.1f;

                if (Expression == ExpressionType.Normal || Expression == ExpressionType.Shy)
                    animator.SetTrigger("Blink");
            }
        }

        public enum ExpressionType
        {
            Normal,
            Injured,
            Dead,
            Shocked,
            Happy,
            Sad,
            Shy,
            Sick,
            CatFace
        }

        public enum AttackActionType
        {
            Swipe,
            Stab,
            Point,
            Summon
        }

        #endregion

    }
}
