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
            //Todo : CC적용을 이렇게? 좀더 생각해볼것
            col.gameObject.GetComponent<PixelCharacterController>().CCApply(CCType.Snare, time, Vector2.zero);
        }
    }
}
