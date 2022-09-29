using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmeraldStaff_RW : RangedWeapon
{
    public Guid skillGuid;
    public float skillCT;
    public float skillCastingTime;
    public float skillDamage;
    public int hprecover;

    private void Start()
    {
        IDX= "Emerald_Staff";
        init();
        skillGuid = Guid.NewGuid();
        
    }
    private void OnEnable()
    {
        init();
    }

    public override void initSkillStat()
    {
        if (skilldata == null)
            return;
        hprecover = skilldata.HP;
        skillCT = skilldata.Cool_Down;
    }

    public override void doSkill()
    {
        Managers.Time.ApplyCooltime(skillGuid, skillCT);
       if(SkillAudio!=null)
            Managers.Player.PlayAuidio(SkillAudio);
        if (SkillHitEffect != null)
        {
            GameObject effect = Managers.Resource.Instantiate(SkillHitEffect);
            effect.transform.position = transform.position;
        }

        Managers.Player.PCtrl.curHp+=hprecover;
    }

    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~EmeraldStaff_RW()
    {

    }
}
