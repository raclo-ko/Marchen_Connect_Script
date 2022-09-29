using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();
        delegate void _del();
        _del gobackQueue;

        public void init(GameObject original, int count = 1)
        {
            Original = original;
            Root = new GameObject().transform; // 위에서 Root를 Transform으로 설정해놔서
            Root.name = $"{original.name}_Root";
            gobackQueue = null;
            Object.DontDestroyOnLoad(Root);
            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            Object.DontDestroyOnLoad(go);

            go.name = Original.name;
            Poolable p=go.GetComponent<Poolable>();
            if (p == null)
                p = go.AddComponent<Poolable>();

            return p;

        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
            {
                return;
                // 없다면 바로 끝낸다.
            }
            poolable.transform.parent = Root;

            // 영상 꺼놓는 부분
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            // 이렇게까지해서 설정이 완료되었으니 stack에 넣어주면된다.
            _poolStack.Push(poolable);
            gobackQueue += poolable.goback_Queue;
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            Debug.Log("count : "+_poolStack.Count + " : " + Root.name);
            
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();
            if (poolable == null)
                Debug.Log("poolable is null 73" + " : " + Root.name);

            poolable.gameObject.SetActive(true);

            if (parent != null)
            {
                poolable.transform.parent = parent;
            }
            
            poolable.isUsing = true;

            return poolable;
        }

        public int count()
        {
            return _poolStack.Count;
        }

        public void Clear()
        {
            gobackQueue();
            _poolStack.Clear();
            Managers.Resource.Destroy(Root.gameObject);
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();

    Transform _root;

    public void init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }
    public void CreatePool(GameObject original, int count = 1)
    {
        if (_pool.ContainsKey(original.name))
        {
            int diff = _pool[original.name].count() - count;
            if (diff > 0)
            {
                _pool[original.name].init(original, diff);
            }
            else return;
        }
        Pool pool = new Pool(); // 새로운 class생성
        pool.init(original, count);
        pool.Root.parent = _root;
        // 현재 _root가 Transform 이니까 pool.Root.parent = _root.trnasform이랑 같은 말이다.

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
        {
            return null;
        }

        return _pool[name].Original;
    }

    public void remove(string name)
    {
        if (_pool.ContainsKey(name) == false)
        {
            return;
        }
        _pool[name].Clear();
        _pool.Remove(name);
    }

    public void ClearAll()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
