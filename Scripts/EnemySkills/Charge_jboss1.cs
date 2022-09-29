using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Charge_jboss1 : Skill
{
    float damage = 3.0f;
    float cooltime = 21.0f;
    float speed = 12.0f;
    Guid skillGuid;
    public string skillprefab = "Charge_jboss1";
    GameObject go = null;
    public LayerMask layer;

    JBoss1_AI _AI;
    EnemyCtrl ctrl;
    private Rigidbody2D rgbd2;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;
    Collider2D _col2;

    private float skillprotime = 2.0f;
    private float totaltime = 0.0f;

    private float delaytime = 1.2f;
    private bool endcharge = false;
    // Start is called before the first frame update
    void Start()
    {
        skillGuid = Guid.NewGuid();
        rgbd2 = GetComponent<Rigidbody2D>();
        ctrl = GetComponent<EnemyCtrl>();
        controller = (Cainos.PixelArtMonster_Dungeon.MonsterController)ctrl.controller;
        layer = 2 << LayerMask.NameToLayer("Obstacle") - 1;
        _AI = GetComponent<JBoss1_AI>();
    }

    public override bool doSkill()
    {
        if (!isSkillOn())
        {
            return false;
        }
        
        isCasting = true;
        controller.inputMove = Vector2.zero;

        float tmp = controller.runSpeedMax;
        controller.runSpeedMax = speed;
        speed = tmp;

        go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab));
        go.transform.parent = gameObject.transform;
        go.transform.localPosition = new Vector3(0, 1.5f, 3);
        AttackGuid guid = go.GetComponent<AttackGuid>();
        guid.myGuid = Guid.NewGuid();
        guid.EventHit = isHit;
        guid.damage = damage;
        return true;
    }

    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCasting && !endcharge)
        {
            totaltime += Time.deltaTime;
            if (totaltime >= skillprotime)
                Charge();
        }
        else
        {
            totaltime = 0;
        }
    }

    private void Charge()
    {
        controller.inputMove = Vector2.right * controller.pm.Facing;
        //ray
        if(Physics2D.Raycast(transform.position + Vector3.up, controller.pm.Facing * Vector2.right, 1.5f, layer))
        {
            endCharge();
            controller.CCApply(CCType.Stun, 3.0f, Vector2.zero);
            //_AI.state = JBoss1_AI.State.Idle;
            controller.inputMove = Vector2.zero;
            
        }
    }

    private void isHit(GameObject g)
    {
        endCharge();
        Vector3 vec = Managers.Player.GamePlayer.transform.position - transform.position;
        vec.z = 0;
        vec.y = 0;
        vec.Normalize();
        //vec = Managers.Player.GamePlayer.transform.position + vec * 2;
        Managers.Player.GamePlayer.transform.position = transform.position - controller.pm.Facing * Vector3.right * 2;
        Managers.Player.PRigid.AddForce(-vec * Managers.Player.PRigid.mass * 1000);
    }

    private void endCharge()
    {
        endcharge = true;
        Managers.Resource.Destroy(go);
        StartCoroutine(DelayEndSkill(delaytime));
        Managers.Time.ApplyCooltime(skillGuid, cooltime);
        float tmp = controller.runSpeedMax;
        controller.runSpeedMax = speed;
        speed = tmp;

    }

    private IEnumerator DelayEndSkill(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        isCasting = false;
        endcharge = false;
        _AI.state = JBoss1_AI.State.Chasing;
    }
}