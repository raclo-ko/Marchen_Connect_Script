using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance { get { init(); return s_instance; } }

    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    TimeManager _time = new TimeManager();
    PlayerManager _player = new PlayerManager();
    SkillManager _skill = new SkillManager();
    //SequenceManager _sequence = new SequenceManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static PlayerManager Player { get { return Instance._player; } }
    public static SkillManager Skill { get { return Instance._skill; } }
    //public static SequenceManager Sequence { get { return Instance._sequence; } }

    private void Start()
    {
        init();
    }

    private void Update()
    {
        
    }

    static void init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            
            s_instance = go.GetComponent<Managers>();
            DontDestroyOnLoad(go);

            s_instance._data.init();
            s_instance._pool.init();
            s_instance._time.init();
            s_instance._player.init();
            s_instance._skill.init();
            //s_instance._sequence.init();
            
        }
    }
}
public enum CCType
{
    Stun,
    Snare,
    KnockDown,
}