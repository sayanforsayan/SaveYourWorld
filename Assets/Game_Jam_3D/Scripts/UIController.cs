using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class UIController : MonoBehaviour
{
    public event Action DestroyCall = delegate { };
    [SerializeField] private GameObject jombiUI, fireUI;
    [SerializeField] private TMP_Text bottonText, bottonTextFire;
    [SerializeField] private GameObject totalhealth, scoreParent;
    private GameObject goObj;
    public Image overAllStatus;
    public GameObject GameOverUI;
    public GameObject zombiDestroyParticleSyatem, waterParticle;
    public Button dialouge_0, dialogue_1;
    [SerializeField]
    private TMP_Text highScoreText, totalScoreText;
    private int killScore = 0;
    public int highScore = 0;
    private void Start()
    {
        IsHealthScoreActive(false);
        dialouge_0.gameObject.SetActive(false);
        dialogue_1.gameObject.SetActive(false);
        jombiUI.SetActive(false);
        fireUI.SetActive(false);
        GameOverUI.SetActive(false);
        overAllStatus.rectTransform.localScale = new Vector3(0f, 0.9f, 1);
        // highScore = GameManager.instance.pefabSCore;
        //// highScore = 10;
        totalScoreText.text = "Current Score: 0";
        highScoreText.text = "High Score: " + highScore.ToString();
    }


    public void IsHealthScoreActive(bool isActive)
    {
        totalhealth.SetActive(isActive);
        scoreParent.SetActive(isActive);
    }
    public void IsJombiUIActive(bool isActive, GameObject go, string btnText)
    {
        goObj = go;
        bottonText.text = btnText;
        jombiUI.SetActive(isActive);

    }
    public void IsFireUiActive(bool isActive, GameObject go, string btnText)
    {
        goObj = go;
        bottonTextFire.text = btnText;
        fireUI.SetActive(isActive);
    }
    public void DestroyJombi()
    {
        SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[3], false, true);
        Instantiate(zombiDestroyParticleSyatem, goObj.transform.position, goObj.transform.rotation);
        if (goObj != null)
        {
            goObj.SetActive(false);

        }
        jombiUI.SetActive(false);

        DestroyCall();
        if (healthBarStatus > 0)
        {
            healthBarStatus--;
            killScore++;
            UpdateKillScore(killScore);
            UpdateHealthBar(healthBarStatus);
        }
    }

    public void DestroyFire()
    {
        SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[1]);
        Instantiate(waterParticle, goObj.transform.position, goObj.transform.rotation);
        if (goObj != null)
        {
            goObj.SetActive(false);

        }
        fireUI.SetActive(false);
        DestroyCall();
        if (healthBarStatus > 0)
        {
            healthBarStatus--;
            killScore++;
            UpdateKillScore(killScore);
            UpdateHealthBar(healthBarStatus);
        }
    }
    float healthBarStatus;
    public void UpdateHealthBar(float barHealth)
    {
        Debug.Log("Health : " + healthBarStatus);
        if(healthBarStatus < 50)
        {
            SoundManager.instance.StopSfx(GameManager.instance.allSound[6]);
            SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[5],true);
        }
        else
        {
            SoundManager.instance.StopSfx(GameManager.instance.allSound[5]);
            SoundManager.instance.PlaySfxSound(GameManager.instance.allSound[6], true);
        }

        if (healthBarStatus > 91)
        {
            GameOverUI.SetActive(true);
        }
        else
        {
            healthBarStatus = barHealth;
            float scaleHealth = healthBarStatus * 0.01f;
            overAllStatus.rectTransform.localScale = new Vector3(scaleHealth, 0.9f, 1);
        }
    }
    public void UpdateHighScore()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
        PlayerPrefs.SetInt("PlayerScore", highScore);
        PlayerPrefs.Save();
    }

    public void UpdateKillScore(int score)
    {
        totalScoreText.text = "Current Score: " + score.ToString();
        if (score > highScore)
        {
            highScore = score;
            UpdateHighScore();
        }
    }
}
