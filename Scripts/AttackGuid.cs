using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackGuid : MonoBehaviour
{
    //enemy's attack attribute

    public Guid myGuid;
    //다단히트가 아닌 공격일 경우 -1또는 0
    public float duration = -1f;
    public float damage = 1;

    public delegate void delv(GameObject g);
    public delv EventOnEnable;
    public delv EventHit;

    // Start is called before the first frame update
    void Start()
    {
        myGuid = Guid.NewGuid();
    }

    public void resetGuid()
    {
        myGuid = Guid.NewGuid();
        System.GC.Collect();
    }
    public void OnEnable()
    {
        resetGuid();
        if (EventOnEnable != null) EventOnEnable(this.gameObject);
    }
    public IEnumerator disableg(float time)
    {
        if (time < 0) yield break;

        yield return new WaitForSeconds(time);
        Managers.Resource.Destroy(this.gameObject);
    }
}