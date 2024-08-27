using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkSwitch2D : MonoBehaviour
{
    public AK.Wwise.Switch Switch;

    [SerializeField] private bool _onStart = false;
    [SerializeField] private bool _onTriggerEnter = false;

    private void Start()
    {
        if (_onStart && AudioManager.Instance != null)
        {
            Switch.SetValue(AudioManager.Instance.SoundPlayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onTriggerEnter && AudioManager.Instance != null)
        {
            Switch.SetValue(AudioManager.Instance.SoundPlayer);
        }
    }
}
