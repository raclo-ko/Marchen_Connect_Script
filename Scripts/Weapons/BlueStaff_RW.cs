using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlueStaff_RW : RangedWeapon
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
        StartCoroutine(CastingSkill());
    }



    private IEnumerator CastingSkill()
    {
        isCasting = true;
        GameObject[] gos = new GameObject[3];
        Vector3 zero= Managers.Player.GamePlayer.transform.position + new Vector3(0, 0.5f, -1);
        Vector3 dir = standardPoint.right;
        for (int i = 0; i < 3; i++)
        {       
            gos[i] = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab.name));
            gos[i].transform.position = zero  + dir * (i + 1);
            gos[i].GetComponent<AttackCol>().init(skillDamage, itsForce, itsColor, this, 0.6f + 0.2f*i, SkillAudio, SkillHitEffect);
            StartCoroutine(lightup(gos[i]));
            yield return new WaitForSeconds(0.2f);
        }
        
        yield return new WaitForSeconds(skillCastingTime-0.6f);
        isCasting = false;
    }

    private IEnumerator lightup(GameObject go)
    {
        float time = 0;
        float maxtime = 0.2f;
        Vector3 zero = go.transform.position;
        while (time < maxtime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
            if (go == null)
                yield break;
            go.transform.localScale = new Vector3(go.transform.localScale.x, 1 + time / maxtime, go.transform.localScale.z);
            go.transform.position += new Vector3(0, 0.5f * Time.deltaTime / maxtime, 0);
        }
        go.transform.localScale = new Vector3(go.transform.localScale.x, 2, go.transform.localScale.z);
        go.transform.position = zero + new Vector3(0, 0.5f, 0);
    }

    public override bool isSkillOn()
    {
        return Managers.Time.isCooltimeOn(skillGuid);
    }

    ~BlueStaff_RW()
    {
        //Managers.Pool.remove(skillprefab.name);
    }
}
