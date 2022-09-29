using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Smite_mboss1 : Skill
{
    float damage = 1.0f;
    float cooltime = 7.0f;
    float projected_speed = 5.0f;
    float skill_make = 1.5f;
    float casting_time = 2.0f;
    Guid skillGuid;
    EnemyCtrl ctrl;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;

    private float delaytime = 0.8f;

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
        controller.inputMove = Vector2.zero;

        yield return new WaitForSeconds(skill_make);

        float tmp = ctrl.weapons[0].damage;
        ctrl.weapons[0].damage = damage;
        damage = tmp;
        Debug.Log("1");
        controller.pm.Attack();

        yield return new WaitForSeconds(casting_time - skill_make);
        tmp = ctrl.weapons[0].damage;
        ctrl.weapons[0].damage = damage;
        damage = tmp;
        StartCoroutine(DelayEndSkill(delaytime));
    }

    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    private IEnumerator DelayEndSkill(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        isCasting = false;
    }

}
