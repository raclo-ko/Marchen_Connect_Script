using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//플레이어 hp, 색관리 등을 관리하는 클래스
public class PlayerCtrl : CharacterCtrl
{
    public EColor playerColor;

    public float maxHp = 6;
    public float curHp;

    public int maxspoidPoint = 100;
    public int spoidePoint = 0;
    public Weapon curWeapon;
    public Transform standardPoint;
    public Collider2D col2;
    
    Dictionary<Guid, float> hit_dic;

    private Vector2 MousePosition;
    private Camera Camera;

    public int key_many=0;

    private int CC_DownCount = 0;
    private float CC_DownTime=0;
    private Coroutine CC_DownC=null;
    private Guid CC_DownG;

    void OnSceneLoaded(Scene scene, LoadSceneMode level)
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if(Managers.Player.GamePlayer != this.gameObject)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void Start()
    {
        curHp = maxHp;
        controller = GetComponent<Controller>();
        hit_dic = new Dictionary<Guid, float>();
        col2 = GetComponent<Collider2D>();
        CC_DownG = Guid.NewGuid();
        //Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -10)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SequenceManager.Sequence.RespawnPlayer();
        }
        if (curHp <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SequenceManager.Sequence.RespawnPlayer();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.ScreenToWorldPoint(MousePosition);

            //transform.position = MousePosition;
            transform.position = new Vector3(MousePosition.x, MousePosition.y, -1);
            Debug.Log(MousePosition);
        }
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnemyAttack"))
        {
            if (!Managers.Time.isCooltimeOn(CC_DownG))
                return;

            CC_DownCount++;
            AttackGuid atkg = col.GetComponent<AttackGuid>();
            Guid g = atkg.myGuid;
            float damage = atkg.damage;
            float duration = col.GetComponent<AttackGuid>().duration;
            if (hit_dic.ContainsKey(g))
            {
                return;
            }

            hit_dic.Add(g, duration);
            StartCoroutine(reset_hitGuid(g,duration));
            curHp -= damage;
            Vector2 power=gameObject.transform.position - col.gameObject.transform.position;
            if (atkg.EventHit != null) atkg.EventHit(col.gameObject);
            //Todo: 얼마만큼의 힘???-변수로 만들수있으면 그렇게 할것
            power = power.normalized * 2f;
            //Todo: 경직시간 얼마??
            if(CC_DownCount <= 3)
            {
                controller.CCApply(CCType.Stun, 0.5f, power);
                if (CC_DownC == null) CC_DownC = StartCoroutine(reset_DownCount(2f));
                CC_DownTime = 2f;
            }
            else
            {
                controller.CCApply(CCType.KnockDown, 1f, Vector2.zero);
                Managers.Time.ApplyCooltime(CC_DownG, 1.6f);
                CC_DownCount = 0;
            }
        }
    }
    private IEnumerator reset_hitGuid(Guid g,float time)
    {
        if (time <= 0)
            yield break;
        yield return new WaitForSeconds(time);
        hit_dic.Remove(g);
    }

    private IEnumerator reset_DownCount(float time)
    {
        CC_DownTime = time;
        while (CC_DownTime > 0.0f)
        {
            yield return new WaitForFixedUpdate();
            CC_DownTime -= Time.deltaTime;
        }
        CC_DownTime = 0f;
        CC_DownCount = 0;
        CC_DownC = null;
    }

    public void Hprecover(float f)
    {
        curHp += f;
        if (curHp > maxHp)
            curHp = maxHp;
    }
}

