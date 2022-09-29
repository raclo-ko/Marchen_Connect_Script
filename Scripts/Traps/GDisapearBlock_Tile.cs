using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDisapearBlock_Tile : MonoBehaviour
{
    public float total_period = 10;
    public float start_disapear = 0;
    public float disapear_period = 5;
    // Start is called before the first frame update


    private Renderer r;
    private Collider2D col;
    private float changetoll =1f;
    private float acctime;
    void Start()
    {
        r = gameObject.GetComponent<Renderer>();
        col = gameObject.GetComponent<Collider2D>();
        acctime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(acctime <= start_disapear)
        {
            if(acctime <= changetoll)
            {
                Color c = r.material.color;
                c.a += 1 * Time.deltaTime / changetoll;
                c.a = Mathf.Min(c.a, 1);
                r.material.color = c;
            }
            else
            {
                col.enabled = true;
            }
        }
        else if(acctime <= start_disapear + disapear_period)
        {
            if (acctime <= start_disapear + changetoll)
            {
                Color c = r.material.color;
                c.a -= 1 * Time.deltaTime / changetoll;
                c.a = Mathf.Max(c.a, 0);
                r.material.color = c;
            }
            else
            {
                col.enabled = false;
            }
        }
        else
        {
            if (acctime <= start_disapear + disapear_period + changetoll)
            {
                Color c = r.material.color;
                c.a += 1 * Time.deltaTime / changetoll;
                c.a = Mathf.Min(c.a, 1);
                r.material.color = c;
            }
            else
            {
                col.enabled = true;
            }
        }


        acctime += Time.deltaTime;
        acctime %= total_period;
    }
}
