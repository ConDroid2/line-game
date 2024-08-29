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
        currentBeat++;

        Debug.Log(currentBeat);
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
    }
}
