using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public GameObject Hp_Full;
    public GameObject Hp_Half;
    public GameObject Hp_None;

    private void Awake()
    {
        Hp_Full.SetActive(true);
        Hp_Half.SetActive(false);
        Hp_None.SetActive(false);
    }
    public void setFull()
    {
        Hp_Full.SetActive(true);
        Hp_Half.SetActive(false);
        Hp_None.SetActive(false);
    }
    public void setHalf()
    {
        Hp_Full.SetActive(false);
        Hp_Half.SetActive(true);
        Hp_None.SetActive(false);
    }

    public void setNone()
    {
        Hp_Full.SetActive(false);
        Hp_Half.SetActive(false);
        Hp_None.SetActive(true);
    }
}
