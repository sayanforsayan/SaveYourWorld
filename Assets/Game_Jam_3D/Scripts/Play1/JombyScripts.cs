using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JombyScripts : MonoBehaviour
{
    public event Action onDestroyJomby=delegate { };
    private void OnMouseDown()
    {
       
        onDestroyJomby();
        //Debug.Log(s);
        Destroy(transform.gameObject);

    }
}
