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
            Debug.Log("넉백_트랩_작동");
            //함정 활성화
            TrapObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
