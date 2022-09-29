using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance = null;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            if(BGMManager.instance.audioSource.clip != audioSource.clip)
            {
                BGMManager.instance.audioSource.clip = audioSource.clip;
                BGMManager.instance.audioSource.time = 0;
            }

            GameObject.Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode level)
    {
        audioSource.Play();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
