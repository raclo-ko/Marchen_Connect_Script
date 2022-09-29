using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpoidUI : MonoBehaviour
{
    private Image content;
    private float mycurSpoid;
    private PlayerCtrl playerctrl;

    public float lerpSpeed = 0.5f;
    public static GameObject SPUI;
    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
        playerctrl = Managers.Player.PCtrl;
        mycurSpoid = 0;
        if (SPUI == null)
            SPUI = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerctrl == null)
            return;
        //Debug.Log(this.transform.position);
        if(mycurSpoid!= playerctrl.spoidePoint)
        {
            mycurSpoid = Mathf.Lerp(mycurSpoid, playerctrl.spoidePoint, Time.deltaTime * lerpSpeed);
            content.fillAmount = mycurSpoid / playerctrl.maxspoidPoint;
        }
    }
}
