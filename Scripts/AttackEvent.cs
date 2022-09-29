using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public List<AttackGuid> list;

    
    void Start()
    {
        list = gameObject.transform.parent.GetComponent<EnemyCtrl>().weapons;
    }

    //Todo:
    public void AttackStart()
    {
        foreach (AttackGuid c in list)
        {
            c.resetGuid();
            c.gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    public void AttackStop()
    {
        foreach (AttackGuid c in list)
        {
            c.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
