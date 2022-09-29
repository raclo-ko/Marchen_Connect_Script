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
            Debug.Log("워프_트랩_작동");
            //함정 활성화
            coll.transform.position = WarpPoint.position;
        }
    }
}
