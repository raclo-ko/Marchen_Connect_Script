using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTrapHp : MonoBehaviour
{
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("PlayerAttack"))
        {
            Debug.Log("워프_트랩_피격");
            hp--;
        }
    }
}
