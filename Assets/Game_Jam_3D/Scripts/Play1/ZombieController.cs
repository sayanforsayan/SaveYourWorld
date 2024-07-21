using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ZombieController : MonoBehaviour
{
    //public static PlayEvent1Controller instance;
    public JombyScripts player;
    public List<Transform> posAll;
    int count = 0;
    [HideInInspector]public int destroyCount = 0;
   
    void Start()
    {
        //CreateCharacter();   
    }

    public void CreateCharacter()
    {
        StartCoroutine(DelayPlayerIns());
    }

    public void OnMouseDown()
    {
        SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[2]);
        GameManager.instance.JombeEffectDestroy(true,this);
    }

    IEnumerator DelayPlayerIns()
    {
        if (count < posAll.Count)
        {
            JombyScripts js=Instantiate(player, posAll[count].position, posAll[count].rotation, transform);
            js.onDestroyJomby += DestroyObjectEvent;
            count++;
            yield return new WaitForSeconds(5);
            StartCoroutine(DelayPlayerIns());

        }
        else
        {
            StopAllCoroutines();
        }   
    }
    private void DestroyObjectEvent()
    {
        destroyCount++;
        if(destroyCount<posAll.Count)
        {
             Debug.Log(destroyCount);

        }
    }

    public void ScalManage(float scalFactor)
    {
        transform.localScale = new Vector3(scalFactor, scalFactor, scalFactor);
    }

}
