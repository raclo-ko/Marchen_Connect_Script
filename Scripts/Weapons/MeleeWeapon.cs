using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    
    private void Awake()
    {
        init();
    }
    public override void init()
    {
        base.init();

        Managers.Pool.CreatePool(prefab_, 2);
        //standardPoint = Managers.Player.PCtrl.standardPoint;
    }


    public override void doAttack()
    {

        GameObject g = Managers.Pool.Pop(prefab_).gameObject;
        g.GetComponent<AttackCol>().init(itsDamage, itsForce, itsColor, this,1, attackAudio);
        g.transform.position = standardPoint.position;
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