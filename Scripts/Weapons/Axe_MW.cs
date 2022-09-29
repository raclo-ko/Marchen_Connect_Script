using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Axe_MW : MeleeWeapon
{
    public GameObject skillprefab;
    public Guid skillGuid;
    public float skillCT;
    public float skillCastingTime;
    public float skillDamage;
    public int hpcost;
    public int speed;

    private void Awake()
    {
        IDX = "Axe";
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
        skillCT = skilldata.Cool_Down;
        skillCastingTime = skilldata.Skill_Duration;
        skillDamage = skilldata.Skill_Damage;
        speed = skilldata.Player_Speed;
        hpcost = skilldata.HP;
    }

    public override void doSkill()
    {
        Managers.Time.ApplyCooltime(skillGuid, skillCT);
        GameObject go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab.name));
        go.transform.parent = Managers.Player.GamePlayer.transform;
        go.transform.localPosition = new Vector3(0, 1, -1);
        AttackCol atc = go.GetComponent<AttackCol>();
        atc.init(skillDamage, itsForce, itsColor, this, 1, SkillAudio, SkillHitEffect);
        atc.Handlers.EventHit = Managers.Skill.recover;

        StartCoroutine(CastingSkill(go));
    }

    private IEnumerator CastingSkill(GameObject go)
    {
        isCasting = true;
        Rigidbody2D rigid = Managers.Player.PRigid;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(skillCastingTime);
        Managers.Resource.Destroy(go);
        isCasting = false;
    }

    private void FixedUpdate()
    {
        if (isCasting)
        {
            Rigidbody2D rigid = Managers.Player.PRigid;
            rigid.MovePosition(rigid.transform.position + standardPoint.right * speed * Time.deltaTime);
        }
    }


    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~Axe_MW()
    {
        Managers.Pool.remove(skillprefab.name);
    }
}
