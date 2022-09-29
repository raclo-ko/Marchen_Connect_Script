using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Jump_mboss1 : Skill
{
    float damage = 1.0f;
    float cooltime = 13.0f;
    float projected_speed = 5.0f;
    float skill_make = 1.6f;
    float casting_time = 1.4f;
    float jumpH = 10.0f;
    string skillprefab = "Jump_mboss1";

    Guid skillGuid;
    EnemyCtrl ctrl;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;

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
        controller.inputMove = Vector2.zero;
        yield return new WaitForSeconds(skill_make);
        Vector2 vec = Managers.Player.GamePlayer.transform.position - transform.position;
        vec.y = 0;
        vec.Normalize();
        controller.inputJump = true;
        
        float tmp = controller.jumpSpeed;
        controller.jumpSpeed = jumpH;
        jumpH = tmp;

        Debug.Log("zhkdzhkd");
        controller.inputMove = vec;
        yield return new WaitUntil(() => !controller.isGrounded);
        yield return new WaitUntil(() => controller.isGrounded);

        controller.inputJump = false;
        GameObject go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab));
        go.transform.position = transform.position;
        AttackGuid g = go.GetComponent<AttackGuid>();
        g.damage = damage;
        g.EventHit = Managers.Skill.Esnare_2sec;
        StartCoroutine(g.disableg(0.5f));

        tmp = controller.jumpSpeed;
        jumpH = tmp;
        controller.airSpeedMax /= 2;
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
