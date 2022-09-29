using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDateSave : MonoBehaviour
{
    public List<GameObject> EnemyDate = new List<GameObject>();

    private void Awake()
    {
        EnemySave();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.instance.EnemyLoad(EnemyDate);

        if (!SaveManager.instance.dic_EnemySaveDate.ContainsKey(SceneManager.GetActiveScene().name))
        {
            SaveManager.instance.EnemySave(EnemyDate);
        }
    }

    private void OnDisable()
    {
        EnemyDate.Clear();
        EnemySave();
        if (EnemyDate.Count > 0)
        {
            SaveManager.instance.EnemySave(EnemyDate);
        }
    }

    public void EnemySave()
    {
        if (this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                EnemyDate.Add(this.transform.GetChild(i).gameObject);
            }
        }
    }
}
