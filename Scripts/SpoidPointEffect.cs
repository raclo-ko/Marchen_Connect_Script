using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoidPointEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startpos;
    void Awake()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(startDestroy());
        startpos = this.transform.position;
    }

    private IEnumerator startDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        Managers.Resource.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(25, Screen.height - 25)), 20 * Time.deltaTime);
    }
}
