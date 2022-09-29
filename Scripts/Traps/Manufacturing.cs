using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manufacturing : MonoBehaviour
{
    public int maxhp=6;
    public int hp;
    
    public List<GameObject> traps;
    // Start is called before the first frame update
    private void Awake()
    {
        hp = maxhp;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerAttack")) {
            hp--;
        }
    }

    private void Update()
    {
        if (hp <= 0)
        {
            foreach(GameObject t in traps)
            {
                t.SetActive(false);
            }
            this.gameObject.SetActive(false);

        }
    }
}
