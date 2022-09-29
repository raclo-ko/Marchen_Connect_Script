using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    public void init()
    {

    }


    public void remove(AttackCol atkc, GameObject to = null)
    {
        Managers.Resource.Destroy(atkc.gameObject);
    }

    public void recover(AttackCol atkc=null, GameObject to=null)
    {
        Managers.Player.PCtrl.curHp ++;
    }

    public void snare2_1sec(AttackCol atkc, GameObject tobj)
    {
        Controller controller = tobj.GetComponent<Controller>();
        if (controller != null) controller.CCApply(CCType.Snare, 1.0f, Vector2.zero);
    }
    public void Esnare_2sec(GameObject go)
    {
        Managers.Player.PCtrl.controller.CCApply(CCType.Snare,2.0f,Vector2.zero);
    }
}
