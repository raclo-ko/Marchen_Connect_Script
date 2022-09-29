using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangedWeapon : Weapon
{
    //default
    public float itsRange=1.0f;
    public float projectileSpeed=5.0f;

    private void Awake()
    {
        init();
    }

    public override void init()
    {
        base.init();
        Managers.Pool.CreatePool(prefab_, 5);
        //standardPoint = Managers.Player.PCtrl.standardPoint;
    }
    public override void doAttack()
    {
        GameObject g = Managers.Pool.Pop(prefab_).gameObject;
        AttackCol atc = g.GetComponent<AttackCol>();
        atc.init(itsDamage, itsForce, itsColor, this, 2, attackAudio);
        atc.Handlers.EventHit = Managers.Skill.remove;


        g.transform.position = standardPoint.position;
        Vector2 v = standardPoint.position - transform.position;
        v.y = 0;
        v.Normalize();
        g.GetComponent<Rigidbody2D>().velocity = v*projectileSpeed;
    }
    public override void doSkill()
    {
        return;
    }
    public override bool isSkillOn()
    {
        return false;
    }
}
