using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;

    public Dictionary<string, Dictionary<string, bool>> dic_EnemySaveDate = new Dictionary<string, Dictionary<string, bool>>();
    public Dictionary<string, Dictionary<string, bool>> dic_ObjectSaveDate = new Dictionary<string, Dictionary<string, bool>>();

    public Dictionary<string, DataClass.Weapon> WeaponItem { get; private set; } = new Dictionary<string, DataClass.Weapon>();
    public List<string> WeaponId = new List<string>();

    private string SceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);

        ItemSave();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode level)
    {
        SceneName = SceneManager.GetActiveScene().name;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void EnemySave(List<GameObject> list)
    {
        //맵의 에너미가 저장되있지 않으면 저장
        if (!dic_EnemySaveDate.ContainsKey(SceneName))
        {
            dic_EnemySaveDate[SceneName] = new Dictionary<string, bool>();
            foreach (GameObject obj in list)
            {
                dic_EnemySaveDate[SceneName][obj.name] = false;
                //Debug.Log(SceneName + "_EnemySave_" + obj.name + "_" + dic_EnemySaveDate[SceneName][obj.name]);
            }
        }
        //맵의 에너미가 저장되 있으면 살아남은 에너미 저장
        else
        {
            foreach (GameObject obj in list)
            {
                dic_EnemySaveDate[SceneName][obj.name] = true;
                //Debug.Log(SceneName + "_EnemySave_" + obj.name + "_" + dic_EnemySaveDate[SceneName][obj.name]);
            }
        }
    }

    public void ObjectSave(List<GameObject> list)
    {
        //맵의 오브젝트가 저장되있지 않으면 저장
        if (!dic_ObjectSaveDate.ContainsKey(SceneName))
        {
            dic_ObjectSaveDate[SceneName] = new Dictionary<string, bool>();
            foreach (GameObject obj in list)
            {
                dic_ObjectSaveDate[SceneName][obj.name] = false;
                Debug.Log(SceneName + "_ObjectSave_" + obj.name + "_" + dic_ObjectSaveDate[SceneName][obj.name]);
            }
        }
        //맵의 오브젝트가 저장되 있으면 살아남은 에너미 저장
        else
        {
            foreach (GameObject obj in list)
            {
                dic_ObjectSaveDate[SceneName][obj.name] = true;
                Debug.Log(SceneName + "_ObjectSave_" + obj.name + "_" + dic_ObjectSaveDate[SceneName][obj.name]);
            }
        }
    }

    public void EnemyLoad(List<GameObject> list)
    {
        //맵의 에너미 정보가 있으면 데이터 로드
        if (dic_EnemySaveDate.ContainsKey(SceneName))
        {
            foreach (GameObject obj in list)
            {
                if (!dic_EnemySaveDate[SceneName][obj.name])
                {
                    Destroy(obj);
                    //Debug.Log("Enemy_LOAD");
                }
                dic_EnemySaveDate[SceneName][obj.name] = false;
            }
        }
    }

    public void ObjectLoad(List<GameObject> list)
    {
        //맵의 오브젝트 정보가 있으면 데이터 로드
        if (dic_ObjectSaveDate.ContainsKey(SceneName))
        {
            foreach (GameObject obj in list)
            {
                if (!dic_ObjectSaveDate[SceneName][obj.name])
                {
                    Destroy(obj);
                    Debug.Log("Object_LOAD");
                }
                dic_ObjectSaveDate[SceneName][obj.name] = false;
            }
        }
    }

    private void ItemSave()
    {
        DataClass.Weapon weapon;
        string weaponId;

        weaponId = "Short_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Bow";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Hammer";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Fork";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Golden_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Iron_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Wooden_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Pickaxe";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Pirate_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Umbrella";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Quill";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Axe";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Shovel";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Emerald_Staff";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Sapphire_Staff";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Battle_Axe";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Magic_Wand";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Wooden_Staff";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Fish";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Spear";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Broom";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Katana";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Flying_Broom";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Silver_Sword";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Knife";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Ruby_Staff";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "School_Bag";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Topaz_Staff";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);

        weaponId = "Tree_Branch";
        if (!Managers.Data.WeaponDic.TryGetValue(weaponId, out weapon)) return;
        else WeaponItem.Add(weaponId, weapon);
        WeaponId.Add(weaponId);
    }
}
