using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _bpm;
    [SerializeField] private float _secondsPerBeat;
    [SerializeField] private int _currentBeat = 0; // typically 1 - 4
    private float _songPositionInSeconds;
    public float SongPositionInBeats;

    private float _dspStartTime;

    public UnityEvent<int> OnBeatChange;
    public UnityEvent<string> OnBeatChangeString;

    private bool _songGoing = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _secondsPerBeat = 60f / (_bpm);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
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



            int newBeat = (Mathf.FloorToInt(SongPositionInBeats) % 4) + 1;

            if (_currentBeat != newBeat)
            {
                _currentBeat = newBeat;
                OnBeatChange.Invoke(_currentBeat);
                OnBeatChangeString.Invoke(_currentBeat.ToString());
            }
        }


    }
}
