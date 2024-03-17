using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private TrackAsset _currentTrack;

    [SerializeField] private AudioSource _audioSource1;
    [SerializeField] private AudioSource _audioSource2;


    private void Awake()
    {
        Instance = this;
    }

    public void SetNewTrack(TrackAsset newTrack)
    {
        if (_currentTrack == newTrack) return;

        _currentTrack = newTrack;

        _audioSource1.Stop();
        _audioSource1.clip = _currentTrack.AudioClip;
        _audioSource1.Play();
    }
}
