using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTrap : MonoBehaviour
{
    public Transform WarpPoint;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("����_Ʈ��_�۵�");
            //���� Ȱ��ȭ
            coll.transform.position = WarpPoint.position;
        }
    }
}
