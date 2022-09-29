using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool isUsing;
    public void goback_Queue()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
