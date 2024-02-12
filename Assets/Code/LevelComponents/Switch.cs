using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private Color _deactivatedColor;
    [SerializeField] private Color _activatedColor;
    public Enums.SwitchType SwitchType = Enums.SwitchType.Default;

    [Header("Events")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    // Variables to keep track of things
    private bool _active = false;

    private void Awake()
    {
        _sprite.color = _deactivatedColor;
    }

    // Activate the Switch based on conditions
    public void Activate()
    {
        if (_active == false)
        {
            _active = true;
            _sprite.color = _activatedColor;
            OnActivated.Invoke();
        }
        else if(_active == true && SwitchType == Enums.SwitchType.Toggle)
        {
            Deactivate();
        }
    }

    // Deactive the Switch
    public void Deactivate()
    {
        if (_active == true)
        {
            _active = false;
            _sprite.color = _deactivatedColor;
            OnDeactivated.Invoke();
        }
    }
}
