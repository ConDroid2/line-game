using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkSwitch2D : MonoBehaviour
{
    public AK.Wwise.Switch Switch;
    public AK.Wwise.Switch SecondarySwitch;

    [SerializeField] private bool _onStart = false;
    [SerializeField] private bool _onTriggerEnter = false;

    private void Start()
    {
        Debug.Log(Switch.Name);
        if (_onStart && AudioManager.Instance != null)
        {
            Switch.SetValue(AudioManager.Instance.SoundPlayer);

            if (SecondarySwitch.Name != "") SecondarySwitch.SetValue(AudioManager.Instance.SoundPlayer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onTriggerEnter && AudioManager.Instance != null)
        {
            Switch.SetValue(AudioManager.Instance.SoundPlayer);

            if (SecondarySwitch.Name != "") SecondarySwitch.SetValue(AudioManager.Instance.SoundPlayer);
        }
    }
}
