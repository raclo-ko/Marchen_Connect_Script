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
            //Todo: ü�� ���Ҵ� PlayerCtrl�������� ó���ǵ��� �ٲܰ�
            col.gameObject.GetComponent<PlayerCtrl>().curHp -= damage;
        }
    }
}
