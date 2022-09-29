using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Weapon : MonoBehaviour
{

    public Transform standardPoint { get { return Managers.Player.PCtrl.standardPoint; } }

    public string IDX;
    public EColor itsColor;
    //default
    public float itsDamage = 1.0f;
    public float itsAttackSpeed = 1.0f;
    public float itsForce;
    public GameObject prefab_ = null;

    public bool isCasting = false;


    public AudioClip attackAudio;
    public AudioClip SkillAudio;
    public GameObject SkillHitEffect;

    [HideInInspector]
    public string icon_path;
    [HideInInspector]
    public string prefab_path;
    public string skill_IDX = "";
    [HideInInspector]
    public int maxOwned;
    [HideInInspector]
    public int minOwned;

    [HideInInspector]
    public WeaponType itsweaponType;
    public DataClass.WeaponSkill skilldata;

    public virtual void init()
    {
        StartCoroutine(wait());
    }

    public virtual void doAttack() { }
    public virtual void doSkill() { }
    public virtual bool isSkillOn() { return false; }

    public IEnumerator wait()
    {
        yield return new WaitUntil(() => Managers.Data.isDone);

        initStat();
        Managers.Data.WeaponSkillDic.TryGetValue(skill_IDX, out skilldata);
        initSkillStat();
    }
    public void initStat()
    {
        DataClass.Weapon data = null;
        Managers.Data.WeaponDic.TryGetValue(IDX, out data);
        if (data == null)
            return;

        itsweaponType = data.Weapon_Type;
        itsAttackSpeed = data.Weapon_ATK_Time;
        itsDamage = data.Weapon_ATK;
        icon_path = data.Weapon_Icon;
        prefab_path = data.Weapon_Prefab;
        skill_IDX = data.Weapon_Skill;
        maxOwned = data.Weapon_Max_Owned;
        minOwned = data.Weapon_Min_Owned;
    }
    public virtual void initSkillStat()
    {
        return;
    }
}

public enum WeaponType
{
    Close_range,
    Medium_range,
    Long_range,
    Magic
}