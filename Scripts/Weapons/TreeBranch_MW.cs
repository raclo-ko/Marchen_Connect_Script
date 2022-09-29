using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreeBranch_MW : MeleeWeapon
{
    public GameObject skillprefab;
    public Guid skillGuid;
    public float skillCT;
    public float skillCastingTime;
    public float skillDamage;

    private void Awake()
    {
        init();
        skillGuid = Guid.NewGuid();
    }


    public override void doSkill()
    {
        Managers.Time.ApplyCooltime(skillGuid, skillCT);
        GameObject go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab.name));
        go.transform.position = Managers.Player.GamePlayer.transform.position;
        AttackCol atc = go.GetComponent<AttackCol>();
        atc.init(skillDamage, itsForce, itsColor, this,3f, SkillAudio, SkillHitEffect);
        atc.Handlers.EventHit = Managers.Skill.snare2_1sec;
        StartCoroutine(CastingSkill());
    }

    private IEnumerator CastingSkill()
    {
        isCasting = true;
        yield return new WaitForSecondsRealtime(skillCastingTime);
        isCasting = false;
    }


    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~TreeBranch_MW()
    {
        Managers.Pool.remove(skillprefab.name);
    }
}
