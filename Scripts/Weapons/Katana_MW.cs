using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Katana_MW : MeleeWeapon
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
        go.transform.parent = Managers.Player.GamePlayer.transform;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<AttackCol>().init(skillDamage, itsForce, itsColor, this, 1, SkillAudio,SkillHitEffect);
        Rigidbody2D rigid = go.GetComponent<Rigidbody2D>();

        StartCoroutine(CastingSkill(go));
    }
    private void FixedUpdate()
    {
        if (isCasting)
        {
            //rigid.MovePosition(rigid.transform.position + standardPoint.right * 6 * Time.deltaTime / skillCastingTime);
        }
    }
    private IEnumerator CastingSkill(GameObject go)
    {

        isCasting = true;
        yield return new WaitForSecondsRealtime(skillCastingTime);
        Managers.Resource.Destroy(go);
        isCasting = false;
    }


    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~Katana_MW()
    {
        Managers.Pool.remove(skillprefab.name);
    }
}
