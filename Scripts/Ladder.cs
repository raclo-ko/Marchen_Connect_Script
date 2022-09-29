using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlatformEffector2D effector2D;
    private BoxCollider2D BoxCollider2D;

    private void Awake()
    {
        effector2D = this.gameObject.GetComponent<PlatformEffector2D>();
        BoxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.S))
            {
                effector2D.enabled = false;

                BoxCollider2D.usedByEffector = false;
                BoxCollider2D.isTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BoxCollider2D.isTrigger = false;
            BoxCollider2D.usedByEffector = true;

            effector2D.enabled = true;
        }
    }
}
