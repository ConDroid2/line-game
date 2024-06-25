using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private TrackAsset _currentTrack;

    [SerializeField] private AudioSource _audioSourceIntro;
    [SerializeField] private AudioSource _audioSourceLoop;

    private enum TrackType { Intro, Loop, None }

    private TrackType _trackType = TrackType.None;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_trackType == TrackType.Intro)
        {
            if(_audioSourceIntro.isPlaying == false  && Application.isFocused)
            {
                Debug.Log("Starting loop");
                Debug.Log($"Is Intro Virtual: {_audioSourceIntro.isVirtual}");
                _audioSourceLoop.Play();
                _trackType = TrackType.Loop;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"Intro Playing: {_audioSourceIntro.isPlaying}");
            Debug.Log($"Loop Playing: {_audioSourceLoop.isPlaying}");
        }
    }

    public void SetNewTrack(TrackAsset newTrack)
    {
        if (_currentTrack == newTrack) return;

        _currentTrack = newTrack;

        _audioSourceIntro.clip = _currentTrack.IntroTrack;
        _audioSourceLoop.clip = _currentTrack.LoopTrack;

        _audioSourceIntro.Play();
        _trackType = TrackType.Intro;
    }
}
