using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform sphereTransform; 
    public int numberOfPositions = 50; 
    public float distanceBetweenPoints = 1.0f;
    public List<AudioClip> allSound = new List<AudioClip>();
    public CameraController camControl;
    public UIController ui;
    public Transform earth;
    public List<Transform> zombieCreatePos = new List<Transform>();
    public ZombieController zombie;
    public FireController fireControler;
    private List<Vector3> surfacePositions = new List<Vector3>();
    [SerializeField] private Button playBtn;
    [HideInInspector]public int totalHealth=0;
    public List<Transform> positionAllForZombiCreate;
    private Button dialogueTwoBtn;
    [SerializeField] private GameObject tempZombie;
    [SerializeField] public int pefabSCore=10;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        camControl.AssignToCam(earth);
        SoundManager.instance.PlaySfxSound(allSound[4], true);

        ui.highScore = PlayerPrefs.GetInt("PlayerScore", 0);
    }

    private void OnEnable()
    {
        ui.DestroyCall += ScrollCamActive;
    }
    private void OnDisable()
    {
        ui.DestroyCall -= ScrollCamActive;
    }

    void Start()
    {
        playBtn.onClick.AddListener(TapPlay); 
        playBtn.enabled = false;
        ActivePlayButton();
    }


    private void ActivePlayButton()
    {
        playBtn.enabled = true;
    }

    private void TapPlay()
    {
        playBtn.transform.parent.gameObject.SetActive(false);
        SoundManager.instance.StopSfx(allSound[4]);
        SoundManager.instance.PlaySfxSound(allSound[5]);
        SoundManager.instance.PlaySfxSound(allSound[7]);
        ui.dialouge_0.gameObject.SetActive(true);
        ui.dialouge_0.onClick.AddListener(TouchFirstDialouge);
    }

    private void TouchFirstDialouge()
    {
        camControl.ZombieCam(true);
        Invoke("Delay", 1f);
    }


    private void Delay()
    {
        SoundManager.instance.StopSfx(allSound[7]);
        SoundManager.instance.StopSfx(allSound[5]);
        SoundManager.instance.PlaySfxSound(allSound[8]);
        SoundManager.instance.PlaySfxSound(allSound[5],true);
        ui.dialouge_0.gameObject.SetActive(false);
        ui.dialogue_1.gameObject.SetActive(true);
        ui.dialogue_1.onClick.AddListener(TouchSecondDialouge);
    }

    private void TouchSecondDialouge()
    {
        ui.dialogue_1.gameObject.SetActive(false);
        SoundManager.instance.StopSfx(allSound[8]);
        StartTheGame();
    }

    private void StartTheGame()
    {
        //camera Must be implemented
        camControl.ZombieCam(false);
        CalculateSphereSurfacePositions();
        surfacePositions = surfacePositions.OrderBy(x => Guid.NewGuid()).ToList();
        StartCoroutine(CreateZombieFire());
        tempZombie.SetActive(false);
        ui.IsHealthScoreActive(true);
    }


    private void CreateFire()
    {
        if (surfacePositions.Count != 0)
        {
            int ran = UnityEngine.Random.Range(2, 4);
           
            for (int i = 0; i < ran; i++)
            {
                int random = UnityEngine.Random.Range(0, surfacePositions.Count);
                Vector3 tempPos = surfacePositions[random];
                FireController fc = Instantiate(fireControler, tempPos, Quaternion.identity);
                totalHealth++;
                ui.UpdateHealthBar(totalHealth);
            }

        }
    }

    private void ZombieCreate()
    {


        if (surfacePositions.Count != 0)
        {
            int ran = UnityEngine.Random.Range(2, 4);

            for (int i = 0; i < ran; i++)
            {
                int random = UnityEngine.Random.Range(0, positionAllForZombiCreate.Count);
                Transform tempPos = positionAllForZombiCreate[random];
                ZombieController tempZombie = Instantiate(zombie, tempPos.position, tempPos.rotation);

                //Vector3 upVector = Vector3.Normalize(new Vector3(0f,0f,0f)); // Assuming the normal is already normalized
                //Vector3 forwardVector = Vector3.Cross(Vector3.right, upVector); // Calculate forward vector
                //Quaternion rotation = Quaternion.LookRotation(forwardVector, upVector);
                //tempZombie.transform.rotation = rotation;

                tempZombie.ScalManage(15f);
                totalHealth++;
                ui.UpdateHealthBar(totalHealth);
            }
        }
    }
    
    IEnumerator CreateZombieFire()
    {
       
        yield return new WaitForSeconds(2);
        ZombieCreate();
        CreateFire();
        yield return new WaitForSeconds(17);
        StartCoroutine(CreateZombieFire());
    }

    public void JombeEffectDestroy(bool bf, ZombieController go)
    {
        ui.IsJombiUIActive(bf,go.gameObject,"KILL");
        camControl.IsFocus(true);
        // ZombiCreate();   
    }

    public void FiretDestroy(bool bf, GameObject go)
    {
        ui.IsFireUiActive(bf, go, "Water");
        camControl.IsFocus(true);
        // ZombiCreate();
    }

    private void ScrollCamActive()
    {
        camControl.IsFocus(false);
    }

    private void CalculateSphereSurfacePositions()
    {
        // Ensure the sphereTransform reference is set
        if (sphereTransform == null)
        {
            Debug.LogError("Sphere transform reference is not set!");
            return;
        }

        // Calculate evenly distributed points on the sphere's surface
        for (int i = 0; i < numberOfPositions; i++)
        {
            float theta = 2 * Mathf.PI * i / numberOfPositions;
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / numberOfPositions);

            float x = Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = Mathf.Cos(phi);

            Vector3 position = sphereTransform.position + sphereTransform.localScale.x * 0.5f * new Vector3(x, y, z);
            surfacePositions.Add(position);
        }
    }
}
