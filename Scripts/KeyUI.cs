using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    private PlayerCtrl playerctrl;
    int mycurkey;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        playerctrl = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        mycurkey = playerctrl.key_many;
    }

    // Update is called once per frame
    void Update()
    {
        if (mycurkey != playerctrl.key_many)
        {
            mycurkey = playerctrl.key_many;
            text.text = "X " + mycurkey;
        }
    }
}
