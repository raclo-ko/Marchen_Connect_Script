using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private Equip upequip;
    private Equip downequip;

    public void OnDrop(PointerEventData eventData)
    {
        //교체할 아이템이 없는 경우
        if (transform.childCount == 0)
        {
            if(this.tag == "AmuletSlot" && Drag.draggingItem.transform.tag == "Amulet")
            {
                Drag.draggingItem.transform.SetParent(Drag.parentTr);
            }
            //무기를 빈 아이템칸에 놓을 경우
            else if(Drag.parentTr.tag == "WeaponSlot")
            {
                Drag.draggingItem.transform.SetParent(Drag.parentTr);
            }
            else if(this.tag != "AmuletSlot")
            {
                Drag.draggingItem.transform.SetParent(this.transform);
                if(this.tag == "WeaponSlot")
                {
                    this.gameObject.GetComponent<Equip>().OnEquip();
                }
            }
        }
        //교체할 아이템이 있는 경우
        else if (transform.childCount >= 1)
        {
            this.transform.GetChild(0).gameObject.transform.SetParent(Drag.parentTr);
            Drag.draggingItem.transform.SetParent(this.transform);
            
            //아이템이 원래 있던 칸이 무기 칸일 경우
            if (Drag.parentTr.tag == "WeaponSlot" && this.tag == "WeaponSlot")
            {
                //Drag.parentTr.gameObject.GetComponent<Equip>().OnEquip();
                upequip = Drag.parentTr.gameObject.GetComponent<Equip>();
                downequip = this.gameObject.GetComponent<Equip>();

                downequip.WeaponSwitch(upequip.eColor, upequip.Curweapon);
                upequip.WeaponSwitch(downequip.eColor, downequip.Curweapon);
                upequip.CharacterChange();
                downequip.CharacterChange();

                if(!GameUI.instance.UIWeapon[upequip.eColor].isSkillOn())
                {
                    GameUI.instance.weapon = downequip.Curweapon;
                    
                    switch (upequip.eColor)
                    {
                        case EColor.Red:
                            StartCoroutine(GameUI.instance.RedCoolTimeUI());
                            break;
                        case EColor.Blue:
                            StartCoroutine(GameUI.instance.BlueCoolTimeUI());
                            break;
                        case EColor.Yellow:
                            StartCoroutine(GameUI.instance.YellowCoolTimeUI());
                            break;
                    }
                }
                if (!GameUI.instance.UIWeapon[downequip.eColor].isSkillOn())
                {
                    GameUI.instance.weapon = upequip.Curweapon;
                    
                    switch (downequip.eColor)
                    {
                        case EColor.Red:
                            StartCoroutine(GameUI.instance.RedCoolTimeUI());
                            break;
                        case EColor.Blue:
                            StartCoroutine(GameUI.instance.BlueCoolTimeUI());
                            break;
                        case EColor.Yellow:
                            StartCoroutine(GameUI.instance.YellowCoolTimeUI());
                            break;
                    }
                }

                GameUI.instance.keeptime = GameUI.instance.UITime[upequip.eColor];
                GameUI.instance.UITime[upequip.eColor] = GameUI.instance.UITime[downequip.eColor];
                GameUI.instance.UITime[downequip.eColor] = GameUI.instance.keeptime;

                upequip.WeaponSynch();
                downequip.WeaponSynch();
            }
            //교체한 칸이 무기칸일 경우
            else if (this.tag == "WeaponSlot")
            {
                this.gameObject.GetComponent<Equip>().OnEquip();
            }
        }
    }
}
