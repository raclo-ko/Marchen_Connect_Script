using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyCtrl : CharacterCtrl
{
    public string MonsterId;

    public float maxHp;
    public float curHp;
    public float curDm;
    public float curBar;

    Dictionary<Guid, float> hit_dic;

    public List<EColor> vulColorsList = null;
    private LinkedList<EColor> vulColors = null;
    private LinkedListNode<EColor> curColor_Node;

    public SpriteRenderer VulCurColor_;
    public SpriteRenderer VulNextColor_;
    public Transform VulCurHp_;
    public SpriteRenderer accumColor_;
    private EColor accColor;

    public List<AttackGuid> weapons;
    public DataClass.Stat stat;
    private AI _AI;

    void Awake()
    {
        if (!Managers.Data.StatDic.TryGetValue(MonsterId, out stat)) return;
        weapons = GetComponentsInChildren<AttackGuid>(true).OfType<AttackGuid>().ToList();
        if (weapons != null)
            weapons.RemoveAll(delegate (AttackGuid col) { return !col.gameObject.CompareTag("EnemyAttack"); });
        _AI = GetComponent<AI>();

        maxHp = stat.Hp;
        //ai.attackTime = stat.atkSpeed;
        
        //ai.GetComponent<CircleCollider2D>().radius = stat.Sight;
        foreach(AttackGuid g in weapons)
        {
            g.damage = stat.Dmg;
        }


        accColor = EColor.None;
        vulColors = new LinkedList<EColor>();
        ClearColorList();
        curHp = maxHp;
        changeColor_sprite(VulCurColor_, vulColors.First.Value);
        if (vulColors.First.Next != null)
        {
            changeColor_sprite(VulNextColor_, vulColors.First.Next.Value);
        }
        else {
            changeColor_sprite(VulNextColor_, vulColors.First.Value);
        }
        
        accumColor_.gameObject.SetActive(false);
        
        hit_dic = new Dictionary<Guid, float>();
        controller = GetComponent<Controller>();
        controller.SetStat(stat);
    }


    void Update()
    {
        if (curHp <= 0)
        {
            //사망            
            Destroy(gameObject);

        }
    }


    private void ClearColorList()
    {
        vulColors.Clear();
        foreach (var value in vulColorsList)
        {
            vulColors.AddLast(value);
        }
        curColor_Node = vulColors.First;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerAttack"))
        {
            AttackCol attackcol;
            if (!col.TryGetComponent<AttackCol>(out attackcol))
                return;
            
            
            Guid g = attackcol.myGuid;
            float duration = attackcol.duration;
            if (hit_dic.ContainsKey(g))
            {
                return;
            }
            
            hit_dic.Add(g, duration);
            StartCoroutine(reset_hitGuid(g, duration));
            DecreaseHp(attackcol.damage, attackcol.color);
            if (attackcol.Handlers.EventHit!=null) attackcol.Handlers.EventHit(attackcol,this.gameObject);
            Managers.Player.PlayAuidio(attackcol.hitAudio);
            GameObject effect = Managers.Resource.Instantiate(attackcol.effect);
            effect.transform.position = col.transform.position;

            if (_AI != null) _AI.UnderAttack();
        }
    }

    private IEnumerator reset_hitGuid(Guid g, float time)
    {
        if (time <= 0)
            yield break;
        yield return new WaitForSeconds(time);
        hit_dic.Remove(g);
    }

    public void DecreaseHp(float dmg, EColor c)
    {
        float totalDMG = dmg;

        if (totalDMG <= 0)
            return;

        if (vulColors != null && ((accColor ^ c) & curColor_Node.Value) == (accColor ^ c))
        {
            accColor |= c;

            accumColor_.gameObject.SetActive(true);
            changeColor_sprite(accumColor_, accColor);
            if ((accColor | curColor_Node.Value) == accColor)
            {
                //totalDMG += 0.5f;
                changeNextColor();
                Managers.Player.PCtrl.spoidePoint += 2;
                accColor = EColor.None;
                //changeColor_sprite(accumColor_, EColor.None);
                controller.CCApply(CCType.Stun, 1f, Vector2.zero);
                GameObject ef = Managers.Resource.Instantiate(Managers.Resource.SpoidPointEffectPath);
                ef.transform.position = this.transform.position;
                accumColor_.gameObject.SetActive(false);
            }
        }
        curHp -= totalDMG;
        if(VulCurHp_ != null) changeHpBar();
        Debug.Log(totalDMG + " 데미지를 입었습니다");
    }
    public void changeNextColor()
    {
        if (curColor_Node.Next != null)
        {
            curColor_Node = curColor_Node.Next;
        }
        else
        {
            ClearColorList();
        }

        changeColor_sprite(VulCurColor_, curColor_Node.Value);

        if (curColor_Node.Next != null)
        {
            changeColor_sprite(VulNextColor_, curColor_Node.Next.Value);
        }
        else
        {
            changeColor_sprite(VulNextColor_, vulColors.First.Value);
        }
    }

    public void changeColor_sprite(SpriteRenderer target, EColor c)
    {
        //Todo: 안전장치용으로 부모자식 여부 확인하는게 좋아보임
        target.color = c.toColor();
    }

    public void changeHpBar()
    {
        curDm = maxHp - curHp;
        if(curDm != 0) curDm = curDm / maxHp * 2;

        curBar = 1;
        curBar -= curDm / 2;

        VulCurHp_.localScale = new Vector3(curDm, VulCurHp_.localScale.y, VulCurHp_.localScale.z);
        VulCurHp_.localPosition = new Vector3(curBar, VulCurHp_.localPosition.y, VulCurHp_.localPosition.z);
        Debug.Log(curDm + ", " + curBar);
    }
}
