using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Smite_jboss1 : Skill
{
    float damage = 2.0f;
    float cooltime = 6.0f;
    float projected_speed = 5.0f;
    float skill_make = 1.7f;
    float casting_time = 1.3f;
    Guid skillGuid;
    EnemyCtrl ctrl;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;

    public string skillprefab = "Smite_jboss1";
    GameObject go = null;

    private float delaytime = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GetComponent<EnemyCtrl>();
        controller = (Cainos.PixelArtMonster_Dungeon.MonsterController)ctrl.controller;
        skillGuid = Guid.NewGuid();
    }

    public override bool doSkill()
    {
        if (!isSkillOn())
        {
            return false;
        }

        Managers.Time.ApplyCooltime(skillGuid, cooltime);
        StartCoroutine(CastingSkill());
        return true;
    }

    private IEnumerator CastingSkill()
    {
        isCasting = true;
        controller.pm.Attack();
        go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab));
        go.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        AttackGuid guid = go.GetComponent<AttackGuid>();
        guid.myGuid = Guid.NewGuid();
        guid.damage = damage;

        guid.EventHit = isHit;
        yield return new WaitForSeconds(casting_time);
        
        if (go!=null) Managers.Resource.Destroy(go);
        StartCoroutine(DelayEndSkill(delaytime));
    }

    private void isHit(GameObject g)
    {
        if (go != null) Managers.Resource.Destroy(go);
        Vector3 vec = Managers.Player.GamePlayer.transform.position - transform.position;
        vec.z = 0;
        vec.y = 0;
        vec.Normalize();
        //vec = Managers.Player.GamePlayer.transform.position + vec * 2;
        Managers.Player.PRigid.AddForce(vec * Managers.Player.PRigid.mass * 400);
    }

    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCasting)
        {
            if (go == null)
                return;
            go.transform.position = new Vector3(go.transform.position.x + controller.pm.Facing * projected_speed * Time.deltaTime, go.transform.position.y, go.transform.position.z);
        }
    }

    private IEnumerator DelayEndSkill(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        isCasting = false;
    }
}
