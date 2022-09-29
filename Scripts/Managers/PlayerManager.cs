using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager
{
    public GameObject GamePlayer { get { init(); return _GamePlayer; }}
    private GameObject _GamePlayer;
    public PlayerCtrl PCtrl { get { init(); return _PCtrl; } }
    private PlayerCtrl _PCtrl;

    public Rigidbody2D PRigid { get { init(); return _PRigid; } }
    private Rigidbody2D _PRigid;


    public AudioSource PAudio { get { init(); return _PAudio; } }
    private AudioSource _PAudio;

    public void init()
    {
        if (_GamePlayer == null)
        {
            _GamePlayer = GameObject.FindWithTag("Player");
            if (_GamePlayer == null)
            {
                return;
                //_GamePlayer = Managers.Resource.Instantiate("Player");
            }
            _PCtrl = GamePlayer.GetComponent<PlayerCtrl>();
            _PRigid = GamePlayer.GetComponent<Rigidbody2D>();
            _PAudio = GamePlayer.GetComponent<AudioSource>();
            Managers.DontDestroyOnLoad(_GamePlayer);
        }
    }

    public void PlayAuidio(AudioClip clip)
    {
        if (clip == null)
            return;

        _PAudio.clip = clip;
        _PAudio.Play();
    }
}
