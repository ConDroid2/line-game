using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkSwitch2D : MonoBehaviour
{
    [SerializeField] private bool _saveTrackToSaveFile;
    public AK.Wwise.Switch Switch;
    public AK.Wwise.Switch SecondarySwitch;

    [SerializeField] private bool _onStart = false;
    [SerializeField] private bool _onTriggerEnter = false;

    private void Start()
    {
        Debug.Log(Switch.Name);
        if (_onStart && AudioManager.Instance != null)
        {
            SetSwitchesValue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onTriggerEnter && AudioManager.Instance != null)
        {
            SetSwitchesValue();
        }
    }

    public void ForceSwitch()
    {
        if (AudioManager.Instance != null)
        {
            SetSwitchesValue();
        }
    }

    public void SetSwitchesValue()
    {
        Switch.SetValue(AudioManager.Instance.SoundPlayer);

        if (SecondarySwitch.Name != "") SecondarySwitch.SetValue(AudioManager.Instance.SoundPlayer);

        if (_saveTrackToSaveFile && GameManager.Instance != null)
        {
            GameManager.Instance.PrimaryTrackData = new SaveSlot.WwiseSwitchData
            {
                SwitchGroup = Switch.WwiseObjectReference.GroupObjectReference.ObjectName,
                SwitchState = Switch.WwiseObjectReference.ObjectName
            };

            GameManager.Instance.SecondaryTrackData = new SaveSlot.WwiseSwitchData
            {
                SwitchGroup = SecondarySwitch.WwiseObjectReference.GroupObjectReference.ObjectName,
                SwitchState = SecondarySwitch.WwiseObjectReference.ObjectName
            };
        }
    }
}
