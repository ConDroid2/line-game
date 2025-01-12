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
        //Debug.Log($"Volume set to {newVolume}");
        //_wwiseMainVolume.SetValue(SoundPlayer, Volume);
        AkSoundEngine.SetRTPCValue(_wwiseMainVolume.Name, newVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
        //Debug.Log($"MusicVolume set to {newVolume}");
        //_wwiseMusicVolume.SetValue(SoundPlayer, MusicVolume);
        AkSoundEngine.SetRTPCValue(_wwiseMusicVolume.Name, newVolume);
    }

    public void SetSfxVolume(float newVolume)
    {
        SfxVolume = newVolume;
        //Debug.Log($"Sfx Volume set to {newVolume}");
        //_wwiseSFXVolume.SetValue(SoundPlayer, SfxVolume);
        AkSoundEngine.SetRTPCValue(_wwiseSFXVolume.Name, newVolume);
    }

}
