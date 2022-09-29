using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fish_MW : MeleeWeapon
{
    public GameObject skillprefab;
    public Guid skillGuid;
    public float skillCT;
    public float skillCastingTime;
    public float skillDamage;
    private GameObject go;
    public int Projectile_Speed;
    Vector3 project_vec;
    private void Awake()
    {
        IDX= "Fish";
        init();
        skillGuid = Guid.NewGuid();
        //project_vec = standardPoint.right;
    }
    private void OnEnable()
    {
        init();
    }

    public override void initSkillStat()
    {
        if (skilldata == null)
            return;
        skillCT = skilldata.Cool_Down;
        skillCastingTime = skilldata.Skill_Duration;
        skillDamage = skilldata.Skill_Damage;
        Projectile_Speed = skilldata.Projectile_Speed;
        project_vec = standardPoint.right;
    }

    public override void doSkill()
    {
        Managers.Time.ApplyCooltime(skillGuid, skilldata.Cool_Down);
        go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab.name));
        go.transform.position = standardPoint.position;
        go.GetComponent<AttackCol>().init(skillDamage, itsForce, itsColor, this,-1, SkillAudio, SkillHitEffect);
        project_vec = standardPoint.right;
        StartCoroutine(CastingSkill(go));
    }
    private void FixedUpdate()
    {
        if (isCasting)
        {
            Rigidbody2D rigid = go.GetComponent<Rigidbody2D>();
            rigid.MovePosition(rigid.transform.position + project_vec * Projectile_Speed * Time.deltaTime);
        }
    }
    private IEnumerator CastingSkill(GameObject go)
    {
        
        isCasting = true;
        yield return new WaitForSeconds(skillCastingTime);
        Managers.Resource.Destroy(go);
        isCasting = false;
    }


    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~Fish_MW()
    {
        Managers.Pool.remove(skillprefab.name);
    }
}
