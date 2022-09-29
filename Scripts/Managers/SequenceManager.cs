using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SequenceManager : MonoBehaviour
{
    private GameObject[] Portals;
    private List<Portal> PortalsList = new List<Portal>();
    private Portal RespawnPortal;
    private int RespawnPoint;

    private int NowPortalScene;

    public GameObject Fade;
    private float Fadetime = 0.5f;

    private float Safetime = 1.0f;
    private bool Sequencesafe;

    public static SequenceManager Sequence = null;

    private void Awake()
    {
        if (Sequence == null)
        {
            Sequence = this;
        }
        else if (Sequence != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        DontDestroyOnLoad(Sequence);

        Sequencesafe = true;
    }

    public void init()
    {
        //GameObject.Find("@Managers").AddComponent<SequenceManager>();
    }

    public void LoadNextScene(int now, int next)
    {
        if (Sequencesafe)
        {
            Sequencesafe = false;
            NowPortalScene = now;

            BGMManager.instance.audioSource.Pause();

            StartCoroutine(SequenceSafe());
            StartCoroutine(LoadNextSceneAsync(next));
        }
        //SceneManager.LoadScene(next);
    }

    //테스트 버전용 함수, 게임오버 화면 제작시 그걸로 변경할 것
    public void RespawnPlayer()
    {
        Portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject @object in Portals)
        {
            PortalsList.Add(@object.GetComponent<Portal>());
            //@object.GetComponent<Portal>();
        }

        RespawnPortal = PortalsList[0];

        for (int i = 0; i < PortalsList.Count; i++)
        {
            if(RespawnPortal.nextScene >= PortalsList[i].nextScene)
            {
                RespawnPoint = i;
            }
        }

        //Managers.Player.GamePlayer.transform.localPosition = PortalsList[RespawnPoint].transform.position;
        Managers.Player.GamePlayer.transform.position = new Vector3(PortalsList[RespawnPoint].transform.position.x, PortalsList[RespawnPoint].transform.position.y, -1);
        Managers.Player.PCtrl.curHp = Managers.Player.PCtrl.maxHp;

        PortalsList.RemoveRange(0, PortalsList.Count);
        for (int i = 0; i < Portals.Length; i++)
        {
            Portals[i] = null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode level)
    {
        Portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject @object in Portals)
        {
            PortalsList.Add(@object.GetComponent<Portal>());
            //@object.GetComponent<Portal>();
        }
        foreach(Portal portal in PortalsList)
        {
            if(portal.nextScene == NowPortalScene)
            {
                //Managers.Player.GamePlayer.transform.localPosition = portal.transform.position;
                Managers.Player.GamePlayer.transform.position = new Vector3(portal.transform.position.x, portal.transform.position.y, -1);
                break;
            }
        }
        PortalsList.RemoveRange(0, PortalsList.Count);
        for (int i = 0; i < Portals.Length ; i++)
        {
            Portals[i] = null;
        }
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator LoadNextSceneAsync(int next)
    {
        AsyncOperation async = 
            SceneManager.LoadSceneAsync(next);

        //StartCoroutine(SequenceSafe());
        Fade.SetActive(true);

        while (!async.isDone)
        {
            yield return null;
        }

        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        yield return new WaitForSeconds(Fadetime);

        Fade.SetActive(false);
    }

    IEnumerator SequenceSafe()
    {
        yield return new WaitForSeconds(Safetime);

        Sequencesafe = true;
    }
}
