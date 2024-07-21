using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> fireAll;
    int count = 0;
    void Start()
    {
        for (int i = 0; i < fireAll.Count; i++)
        {
            fireAll[i].SetActive(false);
        }
        CreateCharacter();

    }

    public void CreateCharacter()
    {
        StartCoroutine(DelayPlayerIns());
    }
    public void OnMouseDown()
    {
        SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[0], false, true);
        GameManager.instance.FiretDestroy(true, this.gameObject);
    }

    IEnumerator DelayPlayerIns()
    {
        if (count < fireAll.Count)
        {

            yield return new WaitForSeconds(2);
            fireAll[count].SetActive(true);
            count++;
            yield return new WaitForSeconds(5);
            StartCoroutine(DelayPlayerIns());

        }
        else
        {
            StopAllCoroutines();
        }

    }
   
    public void ScalManage(float scalFactor)
    {
        transform.localScale = new Vector3(scalFactor, scalFactor, scalFactor);
    }


}
