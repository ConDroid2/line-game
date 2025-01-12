using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public GameObject SoundPlayer;

    public System.Action<int> OnBeat;
    public System.Action OnBar;

    private int currentBeat = 0;

    public AK.Wwise.RTPC _wwiseMusicVolume;
    public AK.Wwise.RTPC _wwiseMainVolume;
    public AK.Wwise.RTPC _wwiseSFXVolume;

    public float Volume {get; private set; } = 50;
    public float MusicVolume { get; private set; } = 50f;
    public float SfxVolume { get; private set; } = 50f;




    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void FireOnBeat()
    {
        currentBeat++;

        // Fire events
        OnBeat?.Invoke(currentBeat);
    }

    public void FireOnBar()
    {
        currentBeat = 0;

        // Fire events
        OnBar?.Invoke();
    }

    public void SetVolume(float newVolume)
    {
        Volume = newVolume;
        _wwiseMainVolume.SetValue(SoundPlayer, Volume);
    }

    public void SetMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
        _wwiseMusicVolume.SetValue(SoundPlayer, Volume);
    }

    public void SetSfxVolume(float newVolume)
    {
        SfxVolume = newVolume;
        _wwiseSFXVolume.SetValue(SoundPlayer, Volume);
    }

}
