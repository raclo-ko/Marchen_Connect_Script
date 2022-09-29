using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDisapearBlock : MonoBehaviour
{
    public float total_period = 10;
    public float start_disapear = 0;
    public float disapear_period = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Disapear");
    }

    IEnumerator Disapear()
    {
        
        yield return new WaitForSeconds(start_disapear);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(disapear_period);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(total_period - start_disapear - disapear_period);
        StartCoroutine("Disapear");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
