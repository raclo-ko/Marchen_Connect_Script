using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class Snare : MonoBehaviour
{
    public float time=1;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Todo : CC������ �̷���? ���� �����غ���
            col.gameObject.GetComponent<PixelCharacterController>().CCApply(CCType.Snare, time, Vector2.zero);
        }
    }
}
