using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsObject : MonoBehaviour
{
    public GameObject TrapObject;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("�˹�_Ʈ��_�۵�");
            //���� Ȱ��ȭ
            TrapObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
