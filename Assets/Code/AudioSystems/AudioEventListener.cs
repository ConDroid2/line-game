using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventListener : MonoBehaviour
{
    [SerializeField] private bool _listenOnBeat = false;
    [SerializeField] private bool _listenOnBar = false;

    [Header("Which Beats")]
    [SerializeField] private List<int> _beatsToListenTo = new List<int>();

    public UnityEvent OnAudioEvent;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        if (_listenOnBeat == true) AudioManager.Instance.OnBeat += HandleBeatEvent;
        if (_listenOnBar == true) AudioManager.Instance.OnBar += HandleBarEvent;
    }

    private void OnDisable()
    {
        AudioManager.Instance.OnBeat -= HandleBeatEvent;
        AudioManager.Instance.OnBar -= HandleBarEvent;
    }

    public void HandleBeatEvent(int beat)
    {
        if (_beatsToListenTo.Contains(beat) || _beatsToListenTo.Count == 0)
        {
            OnAudioEvent?.Invoke();
        }
    }

    public void HandleBarEvent()
    {
        OnAudioEvent?.Invoke();
    }


}
