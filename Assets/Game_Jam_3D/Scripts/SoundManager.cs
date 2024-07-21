using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;


    #region AudioSource area

    private AudioSource asForButtons;
    private AudioSource asForBackground;
    private List<AudioSource> asForAnySFX = new();

    #endregion


    #region variable area

    [HideInInspector] public bool _soundToggle = true;
    [HideInInspector] public bool _musicToggle = true;
    #endregion


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetupAudioSouce();
    }

    private void SetupAudioSouce()
    {
        asForBackground = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        asForBackground.loop = true;
        asForBackground.playOnAwake = false;

        asForButtons = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        asForButtons.playOnAwake = false;

        for (int i = 0; i < 5; i++)
        {
            AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            source.playOnAwake = false;
            asForAnySFX.Add(source);
        }
    }

    public void ToggleSound()
    {
        // PlayButtonSound(0);
        // _soundToggle = !_soundToggle;
        //// gameData.isSoundOn = _soundToggle;
        // GameManager.instance.SaveGameData();
    }

    public void ToggleMusic()
    {
        //PlayButtonSound(0);
        //_musicToggle = !_musicToggle;
        //gameData.isMusicOn = _musicToggle;
        //GameManager.instance.SaveGameData();
        //if (_musicToggle)
        //{
        //    PlayBackgroundMusic(oldmcdSound, true);//index-2
        //}
        //else
        //{
        //    StopBgMusic();
        //}
    }



    //public void PlayButtonSound()
    //{
    //    try
    //    {
    //        if (_soundToggle)
    //        {
    //            asForButtons.clip = defaultButtonSound;
    //            asForButtons.Play();
    //        }
    //    }
    //    catch (System.Exception e)
    //    {

    //        Debug.LogError("SoundManager.PlayBackgroundMusic: " + e);
    //    }

    //}

    public void PlayButtonSound(AudioClip a_Clip)
    {
        if (_soundToggle)
        {
            asForButtons.clip = a_Clip;
            asForButtons.Play();
        }
    }

    public void PlaySFX(AudioSource audioSource)
    {
        if (_soundToggle)
        {
            audioSource.Play();
        }
    }

    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {
        if (_soundToggle)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void SetMusicVolume(float value)
    {
        asForBackground.volume = value;
    }

    public void PlayBackgroundMusic(AudioClip clip, bool isLoop)
    {
        try
        {
            if (_musicToggle)
            {
                asForBackground.clip = clip;
                asForBackground.loop = isLoop;
                asForBackground.Play();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("SoundManager.PlayBackgroundMusic: " + e);
        }

    }

    public void StopBgMusic()
    {
        asForBackground.Stop();
    }

    public void StopSfx(AudioSource audioSource)
    {
        audioSource.Stop();
    }


    public void PlaySfxSound(AudioClip _audio, bool isLoop = false, bool canPlaySameAudio = false, float delay = 0)
    {
        try
        {
            if (_soundToggle)
            {
                AudioSource audioSource = CheckFreeAudioSource();
                if (!canPlaySameAudio)
                {
                    if (IsAnyOnePlayingSameAudio(_audio))
                    {
                        return;
                    }
                }
                audioSource.clip = _audio;
                audioSource.loop = isLoop;
                audioSource.PlayDelayed(delay);
            }
        }
        catch (System.Exception e)
        {

            Debug.LogError("SoundManager.PlaySfxSound: " + e);
        }

    }


    public void StopSfx(AudioClip clip)
    {
        try
        {
            for (int i = 0; i < asForAnySFX.Count; i++)
            {
                if (asForAnySFX[i].isPlaying && asForAnySFX[i].clip == clip)
                {
                    asForAnySFX[i].loop = false;
                    asForAnySFX[i].Stop();

                }
            }
        }
        catch (System.Exception e)
        {

            Debug.LogError("SoundManager.StopSfx: " + e);
        }

    }

    private AudioSource CheckFreeAudioSource()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            if (!asForAnySFX[i].isPlaying)
            {
                return asForAnySFX[i];
            }
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        asForAnySFX.Add(audioSource);
        return asForAnySFX[asForAnySFX.Count - 1];
    }
    public bool IsAnyOnePlayingSameAudio(AudioClip clip)
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            if (asForAnySFX[i].isPlaying && asForAnySFX[i].clip == clip)
            {
                return true;
            }
        }
        return false;
    }

    public void StopAllSfxAndButtonSound()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            asForAnySFX[i].Stop();
            asForAnySFX[i].enabled = false;
        }
        asForButtons.Stop();
        Invoke(nameof(EnableAllAudioSource), .5f);
    }
    private void EnableAllAudioSource()
    {
        for (int i = 0; i < asForAnySFX.Count; i++)
        {
            asForAnySFX[i].enabled = true;
        }
    }
}
