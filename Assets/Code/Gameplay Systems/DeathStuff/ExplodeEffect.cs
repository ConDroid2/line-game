using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeEffect : MonoBehaviour
{
    [SerializeField] private GameObject _turnOff;
    [SerializeField] private ParticleSystem _particleSystem;
    public int Priority = 0;
    public bool ExplodeOnDeath = false;

    private bool _playing = false;

    // public System.Action OnDonePlaying;
    public UnityEvent OnExplodeTriggered;
    public UnityEvent OnExplodeDone;

    private void Awake()
    {
        // TriggerEffect();
    }

    private void Update()
    {
        if (_playing)
        {
            if(_particleSystem.IsAlive() == false)
            {
                _playing = false;
                OnExplodeDone?.Invoke();
                
            }
        }
    }

    public void TriggerEffect()
    {
        if(_turnOff != null)
        {
            _turnOff.SetActive(false);
        }

        _particleSystem.Play();

        _playing = true;
        OnExplodeTriggered?.Invoke();
    }

    public bool IsPlaying()
    {
        return _playing;
    }
}
