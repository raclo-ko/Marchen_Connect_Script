using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.CustomizablePixelCharacter;
using System;
//색상 변경 관련
public class ChangeColor : MonoBehaviour
{
    private PlayerCtrl playerCtrl;

    private GameUI gameUI;

    public GameObject t_object_r;
    public GameObject t_object_b;
    public GameObject t_object_y;
    public Dictionary<EColor, GameObject> m_dic = new Dictionary<EColor,GameObject>();
    public Dictionary<EColor, Equip> e_dic = new Dictionary<EColor, Equip>();
    public GameObject currentColor;

    private Guid changeGuid;
    public float changeCT = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = Managers.Player.PCtrl;
        gameUI = GameObject.FindWithTag("UI").GetComponent<GameUI>();

        t_object_r.SetActive(false);
        t_object_b.SetActive(false);
        t_object_y.SetActive(false);

        m_dic.Add(EColor.Red, t_object_r);
        m_dic.Add(EColor.Blue, t_object_b);
        m_dic.Add(EColor.Yellow, t_object_y);

        currentColor = SetList(EColor.Red);
        playerCtrl.curWeapon = currentColor.GetComponent<Weapon>();
        

        e_dic.Add(EColor.Red, GameObject.Find("RedItemSlot").GetComponent<Equip>());
        e_dic.Add(EColor.Blue, GameObject.Find("BlueItemSlot").GetComponent<Equip>());
        e_dic.Add(EColor.Yellow, GameObject.Find("YellowItemSlot").GetComponent<Equip>());

        changeGuid = Guid.NewGuid();
    }

    //색 바꿈-애니메이터와 무기도 같이 변경
    public GameObject SetList(EColor c)
    {
        GameObject obj;
        m_dic.TryGetValue(c,out obj);
        obj.SetActive(true);
        playerCtrl.playerColor = c;
        //playerCtrl.curAnimator = obj.GetComponentInChildren<Animator>();
        playerCtrl.curWeapon = obj.GetComponent<Weapon>();
        return obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl.curWeapon==null || playerCtrl.curWeapon.isCasting)
            return;

        if (!Managers.Time.isCooltimeOn(changeGuid))
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
            Managers.Time.ApplyCooltime(changeGuid, changeCT);

        if (Input.GetKeyDown(KeyCode.Alpha1) && playerCtrl.playerColor!= EColor.Red) {
            //playerCtrl.curAnimator.Rebind();
            currentColor.SetActive(false);
            currentColor = SetList(EColor.Red);
            currentColor.GetComponent<Weapon>().itsColor = EColor.Red;
            e_dic[EColor.Red].CharacterChange();
            gameUI.RedUI();
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).attackKey = KeyCode.J;
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).skillKey = KeyCode.U;
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && playerCtrl.playerColor != EColor.Blue)
        {
            //playerCtrl.curAnimator.Rebind();
            currentColor.SetActive(false);
            currentColor = SetList(EColor.Blue);
            currentColor.GetComponent<Weapon>().itsColor = EColor.Blue;
            e_dic[EColor.Blue].CharacterChange();
            gameUI.BlueUI();
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).attackKey = KeyCode.K;
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).skillKey = KeyCode.I;
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && playerCtrl.playerColor != EColor.Yellow)
        {
            //playerCtrl.curAnimator.Rebind();
            currentColor.SetActive(false);
            currentColor = SetList(EColor.Yellow);
            currentColor.GetComponent<Weapon>().itsColor = EColor.Yellow;
            e_dic[EColor.Yellow].CharacterChange();
            gameUI.YellowUI();
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).attackKey = KeyCode.L;
            //((Cainos.CustomizablePixelCharacter.PixelCharacterController)Managers.Player.PCtrl.controller).skillKey = KeyCode.O;
            
        }
        
    }
}
