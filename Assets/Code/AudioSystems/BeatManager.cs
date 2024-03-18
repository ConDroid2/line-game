using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private TrackAsset _trackAsset;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _bpm;
    [SerializeField] private float _secondsPerBeat;
    [SerializeField] private int _currentBeat = 0; // typically 1 - 4
    private float _songPositionInSeconds;
    public float SongPositionInBeats;
    private float _totalBeatsInSong;
    // Keep track of how many times we've looped

    private float _dspStartTime;

    public UnityEvent<int> OnBeatChange;
    public UnityEvent<string> OnBeatChangeString;

    private bool _songGoing = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SetTrackAsset(_trackAsset);
        _audioSource.Play();
        _songGoing = true;
        _dspStartTime = (float)AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_songGoing == false)
            {
                _audioSource.Play();
                _songGoing = true;
                _dspStartTime = (float)AudioSettings.dspTime;
            }

            else
            {
                _songGoing = false;
                _audioSource.Stop();
            }
            

        }
        if (_songGoing)
        {
            SongPositionInBeats = _audioSource.timeSamples / (_audioSource.clip.frequency * _secondsPerBeat);

            // Store SongPositionInBeats in _previousSongPositionInBeats
            // If _previous... > SongPosition
            // ---- DeltaBeats = Song
            // DeltaBeats = CurrentBeats - PreviousBeats



            int newBeat = (Mathf.FloorToInt(SongPositionInBeats) % _trackAsset.TimeSignature) + 1;

            if (_currentBeat != newBeat)
            {
                _currentBeat = newBeat;
                OnBeatChange.Invoke(_currentBeat);
                OnBeatChangeString.Invoke(_currentBeat.ToString());
            }
        }


    }

    public void SetTrackAsset(TrackAsset newTrack)
    {
        _trackAsset = newTrack;

        // Set up audio source
        _audioSource.clip = _trackAsset.AudioClip;

        // Set up manager
        _bpm = _trackAsset.BPM;
        _secondsPerBeat = 60f / _bpm;
        _totalBeatsInSong = _trackAsset.AudioClip.length / _secondsPerBeat;
        Debug.Log(_totalBeatsInSong);
    }
}
