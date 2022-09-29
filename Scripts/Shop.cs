using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text SpoidText;
    private int mycurSpoid;
    private int mymaxSpoid;
    private PlayerCtrl playerctrl;

    private const int PRICE_SPD_TO_HP = 25;
    private const int PRICE_SPD_TO_KEY = 50;

    public Button btn_SpdToHp;
    public Button btn_SpdToKey;


    // Start is called before the first frame update
    void Start()
    {
        mycurSpoid = 0;
        mymaxSpoid = 0;
        playerctrl = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();

        btn_SpdToHp.onClick.AddListener(buy_SpdToHp);
        btn_SpdToKey.onClick.AddListener(buy_SpdToKey);
    }

    private void buy_SpdToHp()
    {
        //Debug.Log("buySpoid");
        if (playerctrl.spoidePoint >= PRICE_SPD_TO_HP)
        {
            playerctrl.spoidePoint -= PRICE_SPD_TO_HP;
            playerctrl.curHp++;
        }
    }
    private void buy_SpdToKey()
    {
        if (playerctrl.spoidePoint >= PRICE_SPD_TO_KEY)
        {
            playerctrl.spoidePoint -= PRICE_SPD_TO_KEY;
            playerctrl.key_many++;
            Debug.Log("buyKey");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mycurSpoid != playerctrl.spoidePoint || mymaxSpoid != playerctrl.maxspoidPoint)
        {
            mycurSpoid = playerctrl.spoidePoint;
            mymaxSpoid = playerctrl.maxspoidPoint;
            SpoidText.text = mycurSpoid+"/"+mymaxSpoid;

            if (mycurSpoid < PRICE_SPD_TO_HP) btn_SpdToHp.interactable = false;
            else btn_SpdToHp.interactable = true;

            if (mycurSpoid < PRICE_SPD_TO_KEY) btn_SpdToKey.interactable = false;
            else btn_SpdToKey.interactable = true;
        }

    }

}
