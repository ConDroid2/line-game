using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public GameObject SoundPlayer;

    public System.Action OnBeat;
    public System.Action OnBar;

    public float Volume { get; private set; } = 0.5f;


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
        OnBeat?.Invoke();
    }

    public void FireOnBar()
    {
        OnBar?.Invoke();
    }

    public void SetVolume(float newVolume)
    {
        Volume = newVolume;
    }
}
