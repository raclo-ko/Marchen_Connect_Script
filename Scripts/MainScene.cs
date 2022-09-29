using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public GameObject Fade;

    private void Start()
    {
        SetResolution();
    }

    public void OnStart()
    {
        StartCoroutine(GameStart());
        //Fade.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    IEnumerator GameStart()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        Fade.SetActive(true);

        while (!async.isDone)
        {
            yield return null;
        }
    }
    public void SetResolution()
    {
        //int setWidth = 1920;
        //int setHeight = 1080;

        int setWidth = 1280;
        int setHeight = 720;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        //Screen.SetResolution(setWidth, setHeight, true);
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), false);
    }
}
