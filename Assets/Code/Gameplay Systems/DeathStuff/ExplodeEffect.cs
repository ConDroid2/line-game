using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplodeEffect : OnDeathEffect
{
    [SerializeField] private GameObject _turnOff;
    [SerializeField] private ParticleSystem _particleSystem;
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

    public override void TriggerEffect()
    {
        if(_turnOff != null)
        {
            _turnOff.SetActive(false);
        }

        _particleSystem.Play();

        _playing = true;
        OnExplodeTriggered?.Invoke();
    }

    public override bool IsPlaying()
    {
        return _playing;
    }
}
