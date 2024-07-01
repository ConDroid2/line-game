using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _turnOff;
    [SerializeField] private ParticleSystem _particleSystem;
    public int Priority = 0;

    private bool _playing = false;

    public System.Action OnDonePlaying;

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
                OnDonePlaying?.Invoke();
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
    }

    public bool IsPlaying()
    {
        return _playing;
    }
}
