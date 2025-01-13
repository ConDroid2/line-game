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

    public void ActivateRed()
    {
        if(redOnBlueOff == false)
        {
            redOnBlueOff = true;
            _sprite.sprite = _redSprite;
            OnActivateRed?.Invoke();
        }
    }

    public void ActivateBlue()
    {
        if(redOnBlueOff == true)
        {
            redOnBlueOff = false;
            _sprite.sprite = _blueSprite;
            OnActivateBlue?.Invoke();
        }
    }
}
