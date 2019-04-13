using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public AudioSource efectSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPinchRange = .95f;
    public float hightPinchRange = 1.05f;
    public static float StarPinch = 0.95f;

    private float musicVolume = 1f;
    public static bool HraHudba = true;
    public UndyingCanvasScrip und;

    void Start()
    {
        und = FindObjectOfType(typeof(UndyingCanvasScrip)) as UndyingCanvasScrip;
    }

    void Awake() {
        if (instance == null)
        {
            Debug.Log("Som t8to in3tancia");
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Umierammmm, bleee");
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPinchRange, hightPinchRange);

        efectSource.pitch = randomPitch;
        efectSource.clip = clip;
        efectSource.Play();
    }

    public void PlaySingleStar(AudioClip clip)
    {
        efectSource.pitch = StarPinch;
        StarPinch = StarPinch + 0.05f;
        efectSource.clip = clip;
        efectSource.Play();
    }

    public void RandomizeEfectSound(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPinchRange, hightPinchRange);

        efectSource.pitch = randomPitch;
        efectSource.clip = clips[randomIndex];
        efectSource.Play();
    }

    public void ZastavHudbu()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            HraHudba = false;
        }
        else
        {
            musicSource.UnPause();
            HraHudba = true;
        }
    }

    void Update()
    {
        musicSource.volume = musicVolume;
    }

    public void SetVolume( float volume)
    {
        musicVolume = volume;

        if (volume == 0f)
        {
            musicSource.Pause();
            und.ZmenObrazok();
        }
        else
        {
            if (!(musicSource.isPlaying))
            { musicSource.UnPause();
                und.ZmenObrazok();
            }
        }
    }

}
