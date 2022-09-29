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
        //��ü�� �������� ���� ���
        if (transform.childCount == 0)
        {
            if(this.tag == "AmuletSlot" && Drag.draggingItem.transform.tag == "Amulet")
            {
                Drag.draggingItem.transform.SetParent(Drag.parentTr);
            }
            //���⸦ �� ������ĭ�� ���� ���
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
        //��ü�� �������� �ִ� ���
        else if (transform.childCount >= 1)
        {
            this.transform.GetChild(0).gameObject.transform.SetParent(Drag.parentTr);
            Drag.draggingItem.transform.SetParent(this.transform);
            
            //�������� ���� �ִ� ĭ�� ���� ĭ�� ���
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
            //��ü�� ĭ�� ����ĭ�� ���
            else if (this.tag == "WeaponSlot")
            {
                this.gameObject.GetComponent<Equip>().OnEquip();
            }
        }
    }
}
