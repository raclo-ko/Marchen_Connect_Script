using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jbossInteration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (JBoss1_AI.j_Boss_dead)
        {
            Destroy(this.gameObject);
        }
    }
}
