using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventListener : MonoBehaviour
{
    [SerializeField] private bool _listenOnBeat = false;
    [SerializeField] private bool _listenOnBar = false;

    public UnityEvent OnAudioEvent;

    private void Awake()
    {
        if (AudioManager.Instance == null) return;

        if (_listenOnBeat == true) AudioManager.Instance.OnBeat += HandleEvent;
        if (_listenOnBar == true) AudioManager.Instance.OnBar += HandleEvent;
    }

    public void HandleEvent()
    {
        OnAudioEvent?.Invoke();
    }


}
