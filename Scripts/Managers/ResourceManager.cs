using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public string SpoidPointEffectPath = "sp_circle";

    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject)) //if T=prefab
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {

        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Fail to Load Prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;
        return go;

    }
    public GameObject Instantiate(GameObject original, Transform parent = null)
    {
        if (original == null)
        {
            Debug.Log($"Fail to Load Prefab : {original.name}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;
        return go;

    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public string skillpath(string name)
    {
        return $"Skills/{name}";
    }
}

public class Skill : MonoBehaviour
{
    public bool isCasting = false;
    public virtual bool doSkill() { return false; }
    public virtual bool isSkillOn() { return false; }
}