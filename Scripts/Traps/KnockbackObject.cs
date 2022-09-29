using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class KnockbackObject : MonoBehaviour
{
    public float speed = 0f;
    public float speedcheck = 0f;
    public float time = 1f;

    void Awake()
    {
        speed = 0.01f;
        //장애물 비활성화
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //장애물 이동
        transform.position = new Vector2(transform.position.x - speed, transform.position.y);

        speedcheck += speed;

        if (speedcheck >= 50)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        //플레이어 넉백 및 방애물 파괴
        if (coll.CompareTag("Player"))
        {
            Debug.Log("넉백");
            Vector2 tmp = coll.transform.position - this.gameObject.transform.position;
            //tmp = tmp.normalized * 2;
            coll.gameObject.GetComponent<PixelCharacterController>().CCApply(CCType.Stun, time, tmp);
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
