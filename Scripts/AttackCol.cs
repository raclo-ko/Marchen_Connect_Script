using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackCol : MonoBehaviour
{
    //player's attack attribute

    public float damage;
    public EColor color;
    public float force;
    private Weapon home;
    public WeaponType type;

    public Guid myGuid;
    public float duration = 1f;
    public AudioClip hitAudio;
    public GameObject effect;
    public delegate void EventHandler_atk(AttackCol atk);
    public delegate void EventHandler_atk_to(AttackCol atk, GameObject tobj);

    public class EventHandlers {
        public EventHandler_atk EventOnable;
        public EventHandler_atk_to EventCollision;
        public EventHandler_atk_to EventHit;
        public EventHandler_atk EventDisable;
    }

    public EventHandlers Handlers { get { if (_handlers == null) _handlers = new EventHandlers(); return _handlers; } }
    private EventHandlers _handlers = new EventHandlers();
    public void init(float damage_, float force_, EColor c_, Weapon home_, float _duration = 1f, AudioClip hitAudio_= null, GameObject effect_ = null)
    {
        damage = damage_;
        force = force_;
        home = home_;
        color = c_;
        myGuid = Guid.NewGuid();
        duration = _duration;
        hitAudio = hitAudio_;
        effect = effect_;
        if (effect==null)
            effect = Managers.Resource.Load<GameObject>("Prefabs/hitwhite");
        
        StartCoroutine(disableCol(duration));

        if (_handlers == null)
            _handlers = new EventHandlers();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_handlers.EventCollision != null)
            _handlers.EventCollision(this, col.gameObject);

        if (type == WeaponType.Long_range && col.CompareTag("Floor"))
            Managers.Resource.Destroy(this.gameObject);

        //EventHit은 EnemyCtrl에서 실행
    }

    private void OnEnable()
    {
        if(_handlers.EventOnable!=null) _handlers.EventOnable(this);
        
    }
    private void OnDisable()
    {
        if (_handlers.EventDisable != null) _handlers.EventDisable(this);
    }

    private IEnumerator disableCol(float time)
    {
        if (time < 0) yield break;

        yield return new WaitForSeconds(time);
        Managers.Resource.Destroy(this.gameObject);
    }
}
