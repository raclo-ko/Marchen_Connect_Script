using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAI : MonoBehaviour
{
    //right=1, left=-1
    public int dir = 1;
    public GameObject body;

    public Cainos.PixelArtMonster_Dungeon.MonsterController controll;
    private bool attackAble = true;
    public float attackTime = 2.0f;

    float detectObstacle_Range = 1f;
    //수정여지 있음 픽셀 최소간격?
    float detectFloor_Range = 3f;
    public LayerMask layer;

    //0 : 정찰, 1 : 추적
    public int phase;


    Vector2 obj_pos;
    float tol = 2f;

    

    void Awake()
    {
        phase = 0;
        body = this.transform.parent.gameObject;
        controll = body.GetComponent<Cainos.PixelArtMonster_Dungeon.MonsterController>();
        layer = 2 << LayerMask.NameToLayer("Obstacle") - 1;
    }



    private void Update()
    {
        switch (phase)
        {
            case 0:
                move_recon();
                break;
            case 1:
                move_chase();
                break;

            default:
                break;
        }
    }

    private bool isBlocked()
    {
        return Physics2D.Raycast(transform.position, dir * Vector2.right, detectObstacle_Range, layer) ||
                    !Physics2D.Raycast(transform.position, (dir * Vector2.right + Vector2.down).normalized, detectFloor_Range, layer);
    }

    //정찰
    private void move_recon()
    {
        controll.inputMove = dir * Vector2.right;

        if (isBlocked())
        {
            if (dir < 0)
                dir = 1;
            else
                dir = -1;

        }

        /*Vector2 movevec = Vector2.zero;
        if (dir == -1)
        {
            movevec = Vector2.left;
        }
        if (dir == 1)
        {
            movevec = Vector2.right;
        }
        body.GetComponent<Rigidbody2D>().velocity = speed * movevec;
        */
    }

    private void move_chase()
    {
        Vector2 vec = Vector2.right;
        if (obj_pos.x - body.transform.position.x > 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }

        if (Mathf.Abs(obj_pos.x - body.transform.position.x) <= tol)
        {
            vec.x = 0;
            if (attackAble)
            {
                controll.inputAttack = true;
                StartCoroutine(attackDelay(attackTime));
                //List<Collider2D> list = body.GetComponent<EnemyCtrl>().weapons;
                /*if (list != null && list.Count != 0)
                {
                    StartCoroutine(attackDelay(attackTime, 0.5f, list));
                }
                else
                {
                    StartCoroutine(attackDelay(attackTime));
                }*/
            }
        }

        if (isBlocked())
        {
            vec.x = 0;
        }

        controll.inputMove = dir * vec;
    }

    private IEnumerator attackDelay(float time)
    {

        attackAble = false;
        yield return new WaitForSeconds(time);
        attackAble = true;
    }
    /*
    private IEnumerator attackDelay(float time, float pretime, List<Collider2D> list)
    {
        StartCoroutine(attackDelay(time));

        if (pretime > time) { yield break; }

        yield return new WaitForSeconds(pretime);

        foreach (Collider2D c in list)
        {
            c.enabled = true;
        }
        yield return new WaitForSeconds(pretime);   //collider 유지시간 0.5 초로 변경
        foreach (Collider2D c in list)
        {
            c.enabled = false;
        }
    }*/


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            obj_pos = col.transform.position;
            
            //시야각 60도
            Vector2 local_objpos = obj_pos;
            local_objpos.x -= body.transform.position.x;
            local_objpos.y -= body.transform.position.y;
            float cos_angle=Vector2.Dot(local_objpos, Vector2.right * dir);
            cos_angle /= local_objpos.magnitude;
            if (cos_angle >= Mathf.Cos(Mathf.Deg2Rad * 30))
            {
                phase = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            phase = 0;
        }
    }
}
