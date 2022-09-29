using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollapse : MonoBehaviour
{
    private float time = 0f;
    private bool bActive = false;

    public Transform playerTr;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //ÇÃ·¹ÀÌ¾î Á¢ÃË ½Ã°£
        if (bActive)
        {
            time += Time.deltaTime;
        }
        else time = 0f;

        //0.3ÃÊ ÈÄ ÆÄ±«
        if(time >= 0.3f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && playerTr.position.y >= this.transform.position.y)
        {
            bActive = true;
            Debug.Log("ºØ±«½ÃÀÛ");
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            bActive = false;
            Debug.Log("ºØ±«ÁßÁö");
        }
    }
}
