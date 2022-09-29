using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying_AI : AI
{
    Color _GizmoColor = Color.green;
    public State state = State.Idle;
    Cainos.PixelArtMonster_Dungeon.MonsterFlyingController controller;
    EnemyCtrl ctrl;
    Rigidbody2D rgbd2;
    public LayerMask layer;

    public Vector2Int wan1;
    public Vector2Int wan2;

    private float _range = 2f;
    private float _sight = 5f;
    private float _attackSpeed = 1f;
    private bool _attackAble = true;
    private Collider2D _col2;

    private Vector3 wanderervec;
    private bool iswandertime=true;
    private const float _RunAwayF=4f;
    private const float _RunAwayTime = 0.5f;

    private Vector3 position
    {
        get
        {
            if (_col2 != null)
                return new Vector3(transform.position.x + _col2.offset.x, transform.position.y + _col2.offset.y, transform.position.z);
            else
                return transform.position;
        }
    }
    private Vector3 playerpos
    {
        get
        {
            return new Vector3(Managers.Player.GamePlayer.transform.position.x + Managers.Player.PCtrl.col2.offset.x, Managers.Player.GamePlayer.transform.position.y + Managers.Player.PCtrl.col2.offset.y, Managers.Player.GamePlayer.transform.position.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GetComponent<EnemyCtrl>();
        controller = (Cainos.PixelArtMonster_Dungeon.MonsterFlyingController) ctrl.controller;
        if (controller == null)
            Debug.LogError("Controller Error!!!!");
        layer = 2 << LayerMask.NameToLayer("Obstacle") - 1;
        _sight = ctrl.stat.Sight;
        _range = ctrl.stat.Range;
        _attackSpeed = ctrl.stat.atkSpeed;
        _col2 = this.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            controller.pm.Facing *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Chasing:
                UpdateChasing();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.RunAway:
                RunAway();
                break;
            default:
                break;
        }
    }

    private void UpdateIdle()
    {
        _GizmoColor = Color.green;
        Vector3 disvec = playerpos - position;
        if (disvec.magnitude <= _sight)
        {
            if (controller.pm.Facing * disvec.x < 0 || isBlocked(disvec))
                return;

            state = State.Chasing;
            return;
        }

        if (iswandertime)
        {
            float time = Random.Range(1f, 3f);
            StartCoroutine(WandererTime(time));
            wanderervec = new Vector3(Random.Range(wan1.x, wan2.x), Random.Range(wan1.y, wan2.y), 0);
            controller.inputMove = Vector3.zero;
        }
        if ((wanderervec - position).magnitude >= 1f)
            controller.inputMove = (wanderervec - position).normalized;
        else
            controller.inputMove = Vector2.zero;
    }

    private IEnumerator WandererTime(float time)
    {
        iswandertime = false;
        yield return new WaitForSeconds(time);
        iswandertime = true;
    }

    private void UpdateChasing()
    {
        _GizmoColor = Color.yellow;
        Vector3 vec = playerpos - position;
        controller.pm.Facing = vec.x >= 0 ? 1 : -1;
        if (vec.magnitude <= _range)
        {
            state = State.Attack;
            controller.inputMove = Vector2.zero;
            return;
        }

        if (vec.magnitude > _sight)
        {
            state = State.Idle;
            controller.inputMove = Vector2.zero;
            return;
        }


        controller.inputMove = vec.normalized;
    }

    private void UpdateAttack()
    {
        _GizmoColor = Color.red;
        controller.inputMove = Vector2.zero;

        //todo: AttackEvent부분하고 연결지을것,
        if (_attackAble)
        {
            Vector3 vec = playerpos - position;
            if (vec.magnitude > _range)
            {
                state = State.Chasing;
                controller.inputAttack = false;
                return;
            }
            else
            {
                controller.pm.Facing = vec.x >= 0 ? 1 : -1;
                controller.inputAttack = true;
                StartCoroutine(attackDelay(_attackSpeed));
                
            }
        }

    }

    private void RunAway()
    {
        Vector3 vec = position - playerpos;
        if (vec.magnitude <= _RunAwayF)
        {
            controller.inputMove = vec.normalized;
        }
        else
        {
            state = State.Chasing;
        }
    }

    public override void UnderAttack()
    {
        controller.pm.Facing = (playerpos - position).x >= 0 ? 1 : -1;
        this.state = State.Chasing;

    }

    private bool isBlocked(Vector3 dir)
    {
        //주의!
        return Physics2D.Raycast(position, dir.normalized, dir.magnitude, layer);

    }

    private bool isBlocked()
    {
        //Debug.Log(_col2.ClosestPoint(position + controller.pm.Facing * Vector3.right * _sight));

        return Physics2D.Raycast(position, controller.pm.Facing * Vector2.right, Vector3.Distance(_col2.ClosestPoint(position + controller.pm.Facing * Vector3.right * _sight), position) + 0.2f, layer) ||
                    !Physics2D.Raycast(position, (controller.pm.Facing * Vector2.right + Vector2.down).normalized,
                    Vector3.Distance(_col2.ClosestPoint((position + controller.pm.Facing * Vector3.right + Vector3.down) * _sight), position) + 1.4f, layer);
    }

    private IEnumerator attackDelay(float time)
    {
        _attackAble = false;
        yield return new WaitForSeconds(_RunAwayTime);
        state = State.RunAway;
        yield return new WaitForSeconds(time- _RunAwayTime);
        _attackAble = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _GizmoColor;
        Gizmos.DrawWireSphere(position, _sight);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(position, _range);
    }


    public enum State
    {
        Die,
        Idle,
        Chasing,
        Attack,
        RunAway,
    }
}
