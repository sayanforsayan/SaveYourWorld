using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AutoDestroyOn",4);
    }

   private void AutoDestroyOn()
    {
        Destroy(gameObject);
    }
}
