using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Final_mboss1 : Skill
{
    float damage = 2.0f;
    float cooltime = 1.0f;
    float projected_speed = 4.0f * 2;
    float skill_make = 3.0f;
    float casting_time = 3.0f;
    Vector3 skillPosition;
    Guid skillGuid;
    EnemyCtrl ctrl;
    Cainos.PixelArtMonster_Dungeon.MonsterController controller;

    private string skillprefab = "Final_mboss1";

    private float delaytime = 1.5f;

    private LinkedList<GameObject> gos;

    // Start is called before the first frame update
    void Start()
    {
        ctrl = GetComponent<EnemyCtrl>();
        controller = (Cainos.PixelArtMonster_Dungeon.MonsterController)ctrl.controller;
        skillGuid = Guid.NewGuid();
        skillPosition = transform.position;
        gos = new LinkedList<GameObject>();
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
        yield return new WaitForSeconds(skill_make);
        transform.position = skillPosition;


        float count = 0;
        while (count < 10)
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Resource.skillpath(skillprefab));
            go.transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
            go.transform.localScale = new Vector3(1, 1, 1);
            
            AttackGuid g = go.GetComponent<AttackGuid>();
            g.damage = damage;
            if(g.EventHit == null) g.EventHit = Managers.Resource.Destroy;
            gos.AddLast(go);
            controller.pm.Attack();
            yield return new WaitForSeconds(0.3f);
            count++;
        }

        Debug.Log("fianl");

        StartCoroutine(DelayEndSkill(delaytime));
    }
    public void FixedUpdate()
    {
        if (gos.Count <= 0)
            return;

        foreach (GameObject g in gos)
        {
            if(g.transform.localScale.x <= 10)
                g.transform.localScale += new Vector3(1,1,1) * projected_speed * Time.deltaTime;
            else
            {
                gos.Remove(g);
                Managers.Resource.Destroy(g);
                return;
            }
        }
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
