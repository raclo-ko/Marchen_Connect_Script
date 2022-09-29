using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DataClass;


using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class DataManager
{
    public Dictionary<string, Stat> StatDic { get; private set; } = new Dictionary<string, Stat>();
    public Dictionary<string, DataClass.Weapon> WeaponDic { get; private set; } = new Dictionary<string, DataClass.Weapon>();
    public Dictionary<string, WeaponSkill> WeaponSkillDic { get; private set; } = new Dictionary<string, WeaponSkill>();

    public bool isDone = false;
    public void init()
    {
        

        TextAsset textAsset =Managers.Resource.Load<TextAsset>($"Data/StatData");
        StatData statdata = JsonUtility.FromJson<StatData>(textAsset.text);
        
        foreach (Stat i in statdata.stats)
        {
            StatDic.Add(i.MobType, i);
        }

        textAsset = Managers.Resource.Load<TextAsset>($"Data/WeaponData");
        WeaponData weapondata = JsonConvert.DeserializeObject<WeaponData>(textAsset.text);

        textAsset = Managers.Resource.Load<TextAsset>($"Data/WeaponSkillData");
        WeaponSkillData weaponskilldata = JsonUtility.FromJson<WeaponSkillData>(textAsset.text);


        foreach (DataClass.Weapon i in weapondata.weapons)
        {
            WeaponDic.Add(i.IDX, i);
            //Debug.Log($"{i.IDX}'s type is {i.Weapon_Type}");
        }
        foreach (WeaponSkill i in weaponskilldata.weaponskills)
        {
            WeaponSkillDic.Add(i.IDX, i);
        }

        isDone = true;
    }

}


namespace DataClass
{


    [Serializable]
    public class Stat
    {
        public string MobType;
        public int Hp;
        public float Dmg;
        public float atkSpeed;
        public float movSpeed;
        public float Sight;
        public float Range;
        public string Skill1;
        public string Skill2;
        public string Skill3;
    }

    [Serializable]
    public class Weapon
    {
        public string IDX;
        public string Weapon_Icon;
        public string Weapon_Prefab;
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponType Weapon_Type;
        public int Weapon_ATK;
        public float Weapon_ATK_Time;
        public string Weapon_Skill;
        public int Weapon_Min_Owned;
        public int Weapon_Max_Owned;
    }

    [Serializable]
    public class WeaponSkill
    {
        public string IDX;
        public int Skill_Damage;
        public float Multi_Attack_Time;
        public float Skill_Creation_Time;
        public float Skill_Duration;
        public int Crowd_Control_Time;
        public int HP;
        public int PP;
        public int Player_Speed;
        public int Projectile_Speed;
        public int Cool_Down;
    }


    [Serializable]
    public class StatData
    {
        public List<Stat> stats = new List<Stat>();
    }

    [Serializable]
    public class WeaponData
    {
        public List<Weapon> weapons = new List<Weapon>();
    }
    [Serializable]
    public class WeaponSkillData
    {
        public List<WeaponSkill> weaponskills = new List<WeaponSkill>();
    }
}
