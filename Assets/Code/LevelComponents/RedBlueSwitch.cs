using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedBlueSwitch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Settings")]
    [SerializeField] private Sprite _redSprite;
    [SerializeField] private Sprite _blueSprite;

    private bool redOnBlueOff = true;


    public UnityEvent OnActivateRed;
    public UnityEvent OnActivateBlue;


    public void Toggle()
    {
        if (redOnBlueOff)
        {
            ActivateBlue();
        }
        else
        {
            ActivateRed();
        }
    }

    public void ActivateRed(bool sendEvent = true)
    {
        if(redOnBlueOff == false)
        {
            redOnBlueOff = true;
            _sprite.sprite = _redSprite;

            if (sendEvent)
            {
                OnActivateRed?.Invoke();
            }          
        }
    }

    public void ActivateBlue(bool sendEvent = true)
    {
        if(redOnBlueOff == true)
        {
            redOnBlueOff = false;
            _sprite.sprite = _blueSprite;

            if (sendEvent)
            {
                OnActivateBlue?.Invoke();
            }
        }
    }

    public void SetVisualsRed()
    {
        _sprite.sprite = _redSprite;
    }

    public void SetVisualsBlue()
    {
        _sprite.sprite = _blueSprite;
    }
}
