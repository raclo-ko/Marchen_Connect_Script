using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    Golden,
    Iron,
    Silver,
    Wooden
}

public class ItemBox : MonoBehaviour
{
    public BoxType box;

    private Dictionary<BoxType, string> EnemyName = new Dictionary<BoxType, string>();

    private float closetime;
    private int itemcount;
    private bool isbreak;

    public bool isMimic;

    private Animator animator;

    private DataClass.Weapon weapon;

    private List<Transform> inventoryTr = new List<Transform>();

    private GameObject instance;

    void Awake()
    {
        isbreak = false;
        closetime = 1.5f;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryTr.Add(GameObject.Find("ItemList_1").transform);
        inventoryTr.Add(GameObject.Find("ItemList_2").transform);

        EnemyName.Add(BoxType.Golden, "PF Mimic - Golden");
        EnemyName.Add(BoxType.Iron, "PF Mimic - Iron");
        EnemyName.Add(BoxType.Silver, "PF Mimic - Silver");
        EnemyName.Add(BoxType.Wooden, "PF Mimic - Wooden");
    }

    public void BoxOpen()
    {
        animator.SetBool("IsOpen", true);

        if (!isMimic)
        {
            StartCoroutine(AddItem());
        }
        else if (isMimic)
        {
            Managers.Player.PCtrl.key_many++;
            StartCoroutine(SumonMimic());
        }
    }

    IEnumerator AddItem()
    {
        yield return new WaitForSeconds(closetime);

        for (int i = 0; i < inventoryTr.Count; i++)
        {
            Debug.Log("for_1_clear");
            for (int j = 0; j < inventoryTr[i].childCount; j++)
            {
                Debug.Log("for_2_clear");
                if (inventoryTr[i].GetChild(j).childCount == 0 && inventoryTr[i].GetChild(j).childCount != 1)
                {
                    Debug.Log("if_1_clear");
                    do
                    {
                        //itemcount = Random.RandomRange(0, SaveManager.instance.WeaponId.Count);
                        itemcount = Random.Range(0, SaveManager.instance.WeaponId.Count);
                        weapon = SaveManager.instance.WeaponItem[SaveManager.instance.WeaponId[itemcount]];
                    } while (weapon.Weapon_Min_Owned != 0);
                    Debug.Log("while_1_clear");
                    if (weapon.Weapon_Min_Owned < weapon.Weapon_Max_Owned)
                    {
                        Debug.Log("if_2_clear");
                        instance = Managers.Resource.Instantiate("Item/" + weapon.Weapon_Prefab);
                        instance.transform.parent = inventoryTr[i].GetChild(j).transform;
                        instance.transform.localScale = new Vector3(1, 1, 1);
                        weapon.Weapon_Min_Owned++;
                        isbreak = !isbreak;
                    }
                    break;
                }
            }
            if (isbreak)
            {
                isbreak = !isbreak;
                break;
            }
        }

        Destroy(this.gameObject);
    }

    IEnumerator SumonMimic()
    {
        yield return new WaitForSeconds(closetime);

        instance = Managers.Resource.Instantiate("Enemy/" + EnemyName[box]);
        instance.transform.position = this.transform.position;
        instance.transform.localScale = new Vector3(1, 1, 1);

        Destroy(this.gameObject);
    }
}
