using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager
{
    Dictionary<Guid, Bfloat> cool_Dic=new Dictionary<Guid, Bfloat>();
    
    delegate void _delkey(float f);
    bool isCT_ON = false;
    _delkey delegatekey;
    
    private class Bfloat
    {
        public Guid key;
        public float value;

        public void init(Guid k, float f) { value = f; key = k; Managers.Time.delegatekey += this.subValue; }
        public void subValue(float f) { if (value > 0) value -= f; else deletethis(); }
        public void deletethis() { Managers.Time.delegatekey -= this.subValue; Managers.Time.cool_Dic.Remove(key);}
    }

    public void init()
    {
        if(cool_Dic==null) cool_Dic = new Dictionary<Guid, Bfloat>();
    }

    public void ApplyCooltime(Guid key, float time)
    {
        if (!cool_Dic.ContainsKey(key))
        {
            Bfloat b = new Bfloat();
            b.init(key, time);
            cool_Dic.Add(key, b);
        }
        else
        {
            cool_Dic[key].value = time;
        }

        if (!isCT_ON)
        {
            isCT_ON = true;
            Managers.Instance.StartCoroutine(CTDelay());
        }
    }
    
    public bool isCooltimeOn(Guid key)
    {
        return !cool_Dic.ContainsKey(key);
    }

    private IEnumerator CTDelay()
    {
        while (cool_Dic.Count > 0)
        {
            delegatekey(Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        isCT_ON = false;
        System.GC.Collect();
    }
}
