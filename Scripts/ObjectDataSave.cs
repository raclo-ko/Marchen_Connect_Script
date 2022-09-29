using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDataSave : MonoBehaviour
{
    public List<GameObject> ObjectDate = new List<GameObject>();

    private void Awake()
    {
        ObjectSave();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.instance.ObjectLoad(ObjectDate);

        if (!SaveManager.instance.dic_ObjectSaveDate.ContainsKey(SceneManager.GetActiveScene().name))
        {
            SaveManager.instance.ObjectSave(ObjectDate);
        }
    }

    private void OnDisable()
    {
        ObjectDate.Clear();
        ObjectSave();
        if (ObjectDate.Count > 0)
        {
            SaveManager.instance.ObjectSave(ObjectDate);
        }
    }

    public void ObjectSave()
    {
        if (this.transform.childCount > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if(this.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    ObjectDate.Add(this.transform.GetChild(i).gameObject);
                }
                //ObjectDate.Add(this.transform.GetChild(i).gameObject);
            }
        }
    }
}
