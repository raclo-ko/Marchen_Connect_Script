using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int nowScene;
    public int nextScene;

    private void Awake()
    {
        nowScene = SceneManager.
            GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log(nowScene + ", " + nextScene);
                SequenceManager.Sequence.
                    LoadNextScene(nowScene, nextScene);
            }
        }
    }
}
