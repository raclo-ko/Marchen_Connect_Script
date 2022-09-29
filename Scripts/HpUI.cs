using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    private Image[] contents;
    private PlayerCtrl playerctrl;
    private float mycurHp;
    private float mymaxHp;
    // Start is called before the first frame update
    void Start()
    {
        contents = GetComponentsInChildren<Image>();
        playerctrl = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mycurHp != playerctrl.curHp || mymaxHp != playerctrl.maxHp)
        {
            mycurHp = playerctrl.curHp;
            mymaxHp = playerctrl.maxHp;
            for (int i = 0; i < contents.Length; i++)
            {
                float tmp = mycurHp / 2 - i;
                contents[i].fillAmount = tmp >= 1 ? 1f:tmp;
            }
        }
    }
}
