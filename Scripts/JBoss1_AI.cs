using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBoss1_AI : AI
{
    Color _GizmoColor = Color.green;
    public State state = State.Idle;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;
    EnemyCtrl ctrl;
    Rigidbody2D rgbd2;
    public LayerMask layer;

    public static bool j_Boss_dead = false;

    private float _range = 2f;
    private float _sight = 5f;
    private float _attackSpeed = 1f;
    private bool _attackAble = true;
    private Collider2D _col2;

    private Skill _skill1;
    private Skill _skill2;

    private float skillRange = 3.0f;

    private Vector3 position
    {
        get
        {
            if (_col2 != null)
                return new Vector3(transform.position.x + _col2.offset.x, transform.position.y + _col2.offset.y, 0);
            else
                return transform.position;
        }
    }
    private Vector3 playerpos
    {
        get
        {
            return new Vector3(Managers.Player.GamePlayer.transform.position.x + Managers.Player.PCtrl.col2.offset.x, Managers.Player.GamePlayer.transform.position.y + Managers.Player.PCtrl.col2.offset.y, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GetComponent<EnemyCtrl>();
        controller = (Cainos.PixelArtMonster_Dungeon.MonsterController)ctrl.controller;
        if (controller == null)
            Debug.LogError("Controller Error!!!!");
        layer = 2 << LayerMask.NameToLayer("Obstacle") - 1;
        _sight = ctrl.stat.Sight;
        _range = ctrl.stat.Range;
        _attackSpeed = ctrl.stat.atkSpeed;
        _col2 = this.GetComponent<Collider2D>();

        _skill1 = (Skill)gameObject.AddComponent(System.Type.GetType(ctrl.stat.Skill1));
        _skill2 = (Skill)gameObject.AddComponent(System.Type.GetType(ctrl.stat.Skill2));
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
        controller.inputMove = Vector2.zero;
    }

    private void UpdateChasing()
    {
        _GizmoColor = Color.yellow;
        Vector3 vec = playerpos - position;
        if (CheckSkill(vec)) return;

        controller.pm.Facing = vec.x >= 0 ? 1 : -1;
        if (vec.magnitude <= _range)
        {
            state = State.Attack;
            controller.inputMove = Vector2.zero;
            return;
        }

        //내 머리위에서 내려와...
        if (Mathf.Abs(vec.x) < _range)
        {
            controller.inputMove = Vector2.zero;
            return;
        }

        controller.inputMove = vec.normalized;
    }

    private void UpdateAttack()
    {
        _GizmoColor = Color.red;
        Vector3 vec = playerpos - position;

        if (CheckSkill(vec)) return;

        if (vec.magnitude > _range)
        {
            state = State.Chasing;
            controller.inputAttack = false;
            return;
        }

        if (_attackAble)
        {
            controller.pm.Facing = vec.x >= 0 ? 1 : -1;
            controller.inputAttack = _attackAble;
            StartCoroutine(attackDelay(_attackSpeed));
            return;
        }

    }
    private bool CheckSkill(Vector2 vec)
    {

        if (_skill1.isCasting || _skill2.isCasting)
            return true;

        if (!_skill1.isSkillOn() && !_skill2.isSkillOn())
        {
            return false;
        }

        if (vec.magnitude > skillRange)
        {
            if (_skill2.doSkill()) return true;
        }
        else
        {
            if (_skill1.doSkill()) return true;
        }

        return false;
    }

    public override void UnderAttack()
    {
        controller.pm.Facing = (playerpos - position).x >= 0 ? 1 : -1;
        this.state = State.Chasing;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            controller.pm.Facing *= -1;
        }
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
        yield return new WaitForSeconds(time);
        _attackAble = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _GizmoColor;
        Gizmos.DrawWireSphere(position, _sight);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(position, _range);
    }

    public void OnDestroy()
    {
        j_Boss_dead = true;
    }

    public enum State
    {
        Die,
        Idle,
        Chasing,
        Attack,
    }
}
