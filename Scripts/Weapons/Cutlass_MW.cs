using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cutlass_MW : MeleeWeapon
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
        GameObject[] gos = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            gos[i] = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab.name));
            gos[i].transform.parent = Managers.Player.GamePlayer.transform;
            gos[i].transform.localPosition = new Vector3(0, 3.5f, -1) + standardPoint.right*(i+1);
            gos[i].GetComponent<AttackCol>().init(skillDamage, itsForce, itsColor, this, 1, SkillAudio, SkillHitEffect);
        }
        StartCoroutine(CastingSkill(gos));
    }

    private IEnumerator CastingSkill(GameObject[] gos)
    {
        isCasting = true;
        yield return new WaitForSecondsRealtime(skillCastingTime);
        foreach(GameObject g in gos)
            Managers.Resource.Destroy(g);
        isCasting = false;
    }


    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~Cutlass_MW()
    {
        Managers.Pool.remove(skillprefab.name);
    }
}
