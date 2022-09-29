using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    public GameObject Prefab;
    private GameObject[] hp_obj;
    // Start is called before the first frame update
    void Awake()
    {
       /*hp_obj = new GameObject[3];
        for (int i = 0; i < 3; i++) {
            hp_obj[i] = Instantiate<GameObject>(Prefab);
            hp_obj[i].transform.SetParent(transform);
            hp_obj[i].transform.localPosition = new Vector3(0.3f - 0.3f * i, 0);
            
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void changeHp_sprite(float maxhp, float curhp)
    {
        switch ((int)(6 * curhp / maxhp))
        {
            case 0:
                hp_obj[0].GetComponent<HpBar>().setNone();
                hp_obj[1].GetComponent<HpBar>().setNone();
                hp_obj[2].GetComponent<HpBar>().setNone();
                break;
            case 1:
                hp_obj[0].GetComponent<HpBar>().setNone();
                hp_obj[1].GetComponent<HpBar>().setNone();
                hp_obj[2].GetComponent<HpBar>().setHalf();
                break;
            case 2:
                hp_obj[0].GetComponent<HpBar>().setNone();
                hp_obj[1].GetComponent<HpBar>().setNone();
                hp_obj[2].GetComponent<HpBar>().setFull();
                break;
            case 3:
                hp_obj[0].GetComponent<HpBar>().setNone();
                hp_obj[1].GetComponent<HpBar>().setHalf();
                hp_obj[2].GetComponent<HpBar>().setFull();
                break;
            case 4:
                hp_obj[0].GetComponent<HpBar>().setNone();
                hp_obj[1].GetComponent<HpBar>().setFull();
                hp_obj[2].GetComponent<HpBar>().setFull();
                break;
            case 5:
                hp_obj[0].GetComponent<HpBar>().setHalf();
                hp_obj[1].GetComponent<HpBar>().setFull();
                hp_obj[2].GetComponent<HpBar>().setFull();
                break;
            case 6:
                hp_obj[0].GetComponent<HpBar>().setFull();
                hp_obj[1].GetComponent<HpBar>().setFull();
                hp_obj[2].GetComponent<HpBar>().setFull();
                break;
        }
    }*/
    public void changeColor_sprite(EColor c)
    {
        GetComponent<SpriteRenderer>().color = c.toColor();
    }
}
