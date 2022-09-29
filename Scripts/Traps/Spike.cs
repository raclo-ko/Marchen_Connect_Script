using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float damage=1;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Todo: 체력 감소는 PlayerCtrl내에서만 처리되도록 바꿀것
            col.gameObject.GetComponent<PlayerCtrl>().curHp -= damage;
        }
    }
}
