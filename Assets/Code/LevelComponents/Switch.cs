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
    [SerializeField] private Sprite _deactivatedSprite;
    [SerializeField] private Sprite _activatedSprite;
    public Enums.SwitchType SwitchType = Enums.SwitchType.Default;

    [Header("Events")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    // Variables to keep track of things
    private bool _active = false;

    private void Awake()
    {
        _sprite.sprite = _deactivatedSprite;
    }

    // Activate the Switch based on conditions
    public void Activate()
    {
        if (_active == false)
        {
            Debug.Log("Activating");
            _active = true;
            _sprite.sprite = _activatedSprite;
            OnActivated.Invoke();
        }
        else if(_active == true && SwitchType == Enums.SwitchType.Toggle)
        {
            Debug.Log("Deactivating");
            Deactivate();
        }
    }

    // Deactive the Switch
    public void Deactivate()
    {
        if (_active == true)
        {
            _active = false;
            _sprite.sprite = _deactivatedSprite;
            OnDeactivated.Invoke();
        }
    }

    public void SetActivatedColor(bool setActiveColor)
    {
        _sprite.color = setActiveColor ? _activatedColor : _deactivatedColor;
    }
}
