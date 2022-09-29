using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public Transform RedweaponTr;
    public Transform BlueweaponTr;
    public Transform YellowweaponTr;

    public Weapon Curweapon;
    public Weapon Switchweapon;

    public EColor eColor;

    private Drag drag;
    private ChangeColor changeColor;
    private Cainos.CustomizablePixelCharacter.PixelCharacter pixelCharacter;

    private Weapon weapon;

    private GameObject pre;

    // Start is called before the first frame update
    void Start()
    {
        RedweaponTr = GameObject.Find("RedWeaponSlot").transform;
        BlueweaponTr = GameObject.Find("BlueWeaponSlot").transform;
        YellowweaponTr = GameObject.Find("YellowWeaponSlot").transform;

        changeColor = Managers.Player.GamePlayer.GetComponent<ChangeColor>();
        pixelCharacter = Managers.Player.GamePlayer.GetComponent<Cainos.CustomizablePixelCharacter.PixelCharacter>();
    }

    public void OnEquip()
    {
        UnEquip();

        drag = this.transform.GetChild(0).GetComponent<Drag>();
        pre = Managers.Resource.Instantiate("Weapons/" + drag.weaponPrefabs.name);
        switch (eColor)
        {
            case EColor.Red:
                pre.transform.parent = RedweaponTr;
                break;
            case EColor.Blue:
                pre.transform.parent = BlueweaponTr;
                break;
            case EColor.Yellow:
                pre.transform.parent = YellowweaponTr;
                break;
        }
        pre.transform.localPosition = Vector3.zero;
        pre.transform.localEulerAngles = Vector3.zero;

        CharacterChange(drag);
        GameUI.instance.ChangeWeapon(eColor, drag.image.sprite);
        weapon = pre.GetComponent<Weapon>();

        if (weapon)
        {
            Curweapon = weapon;

            switch (eColor)
            {
                case EColor.Red:
                    Curweapon.itsColor = EColor.Red;

                    changeColor.t_object_r = pre.gameObject;
                    changeColor.t_object_r.SetActive(false);

                    changeColor.m_dic.Remove(EColor.Red);
                    changeColor.m_dic.Add(EColor.Red, changeColor.t_object_r);

                    changeColor.currentColor.SetActive(false);
                    changeColor.currentColor = changeColor.SetList(EColor.Red);

                    GameUI.instance.UIWeapon[EColor.Red] = Curweapon;
                    break;
                case EColor.Blue:
                    Curweapon.itsColor = EColor.Blue;

                    changeColor.t_object_b = pre.gameObject;
                    changeColor.t_object_b.SetActive(false);

                    changeColor.m_dic.Remove(EColor.Blue);
                    changeColor.m_dic.Add(EColor.Blue, changeColor.t_object_b);

                    changeColor.currentColor.SetActive(false);
                    changeColor.currentColor = changeColor.SetList(EColor.Blue);

                    GameUI.instance.UIWeapon[EColor.Blue] = Curweapon;
                    break;
                case EColor.Yellow:
                    Curweapon.itsColor = EColor.Yellow;

                    changeColor.t_object_y = pre.gameObject;
                    changeColor.t_object_y.SetActive(false);

                    changeColor.m_dic.Remove(EColor.Yellow);
                    changeColor.m_dic.Add(EColor.Yellow, changeColor.t_object_y);

                    changeColor.currentColor.SetActive(false);
                    changeColor.currentColor = changeColor.SetList(EColor.Yellow);

                    GameUI.instance.UIWeapon[EColor.Yellow] = Curweapon;
                    break;
            }
        }
    }

    public void UnEquip()
    {
        if (Curweapon)
        {
            Destroy(Curweapon.gameObject);
        }
    }

    public void CharacterChange()
    {
        //drag = this.transform.GetChild(0).GetComponent<Drag>();
        if (this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                drag = this.transform.GetChild(i).GetComponent<Drag>();
                if(drag != null)
                {
                    break;
                }
            }
        }

        if(drag != null)
        {
            CharacterChange(drag);
            GameUI.instance.ChangeWeapon(eColor, drag.image.sprite);
        }
    }

    public void CharacterChange(Drag drag)
    {
        pixelCharacter.HatMaterial = drag.HatMaterial;
        pixelCharacter.HairMaterial = drag.HairMaterial;
        pixelCharacter.EyeMaterial = drag.EyeMaterial;
        pixelCharacter.EyeBaseMaterial = drag.EyeBaseMaterial;
        pixelCharacter.FacewearMaterial = drag.FacewearMaterial;
        pixelCharacter.ClothMaterial = drag.ClothMaterial;
        pixelCharacter.PantsMaterial = drag.PantsMaterial;
        pixelCharacter.SocksMaterial = drag.SocksMaterial;
        pixelCharacter.ShoesMaterial = drag.ShoesMaterial;
        pixelCharacter.BackMaterial = drag.BackMaterial;
        pixelCharacter.BodyMaterial = drag.BodyMaterial;
        pixelCharacter.ClipHair = drag.ClipHair;
    }

    public void WeaponSwitch(EColor color, Weapon iweapon)
    {
        switch (color)
        {
            case EColor.Red:
                Curweapon.gameObject.transform.parent = RedweaponTr;
                Curweapon.itsColor = EColor.Red;

                changeColor.t_object_r = Curweapon.gameObject;
                changeColor.t_object_r.SetActive(false);

                changeColor.m_dic.Remove(EColor.Red);
                changeColor.m_dic.Add(EColor.Red, changeColor.t_object_r);

                changeColor.currentColor.SetActive(false);
                changeColor.currentColor = changeColor.SetList(EColor.Red);

                GameUI.instance.UIWeapon[EColor.Red] = Curweapon;
                break;
            case EColor.Blue:
                Curweapon.gameObject.transform.parent = BlueweaponTr;
                Curweapon.itsColor = EColor.Blue;

                changeColor.t_object_b = Curweapon.gameObject;
                changeColor.t_object_b.SetActive(false);

                changeColor.m_dic.Remove(EColor.Blue);
                changeColor.m_dic.Add(EColor.Blue, changeColor.t_object_b);

                changeColor.currentColor.SetActive(false);
                changeColor.currentColor = changeColor.SetList(EColor.Blue);

                GameUI.instance.UIWeapon[EColor.Blue] = Curweapon;
                break;
            case EColor.Yellow:
                Curweapon.gameObject.transform.parent = YellowweaponTr;
                Curweapon.itsColor = EColor.Yellow;

                changeColor.t_object_y = Curweapon.gameObject;
                changeColor.t_object_y.SetActive(false);

                changeColor.m_dic.Remove(EColor.Yellow);
                changeColor.m_dic.Add(EColor.Yellow, changeColor.t_object_y);

                changeColor.currentColor.SetActive(false);
                changeColor.currentColor = changeColor.SetList(EColor.Yellow);

                GameUI.instance.UIWeapon[EColor.Yellow] = Curweapon;
                break;
        }

        //CharacterChange();
        Switchweapon = iweapon;
    }

    public void WeaponSynch()
    {
        Curweapon = Switchweapon;
    }
}
