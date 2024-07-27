using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateScript : MonoBehaviour
{

    public Collider2D thisCollider;

    private List<Collider2D> _collidersInTrigger = new List<Collider2D>();

    private bool _isActivated = false;

    [Header("Setting")]
    [SerializeField] private bool _isHeavy = false;

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _regularActive;
    [SerializeField] private Sprite _regularInactive;
    [SerializeField] private Sprite _heavyActive;
    [SerializeField] private Sprite _heavyInactive;


    // Events
    public UnityEvent OnPressed;
    public UnityEvent OnDeactivated;

    private void FixedUpdate()
    {
        // Find out if pressure plate should be active
        bool shouldBeActive = false;

        foreach(Collider2D collider in _collidersInTrigger)
        {
            if (_isHeavy && collider.CompareTag("Pushable") == false) 
            { 
                continue; 
            }

            shouldBeActive |= CheckIfEncapsulated(collider);
        }

        // If not active, but should be, activate
        if(_isActivated == false && shouldBeActive == true)
        {
            Activate();
        }
        // If active but shouldn't be, deactivate
        else if(_isActivated == true && shouldBeActive == false)
        {
            Deactivate();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_collidersInTrigger.Contains(collision) == false)
        {
            _collidersInTrigger.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_collidersInTrigger.Contains(collision))
        {
            _collidersInTrigger.Remove(collision);
        }
    }

    private bool CheckIfEncapsulated(Collider2D other)
    {
        return thisCollider.bounds.Contains(other.bounds.max) && thisCollider.bounds.Contains(other.bounds.min);
    }

    private void Activate()
    {
        OnPressed.Invoke();
        _isActivated = true;

        _spriteRenderer.sprite = _isHeavy == false ? _regularActive : _heavyActive;
    }

    private void Deactivate()
    {
        OnDeactivated.Invoke();
        _isActivated = false;

        _spriteRenderer.sprite = _isHeavy == false ? _regularInactive : _heavyInactive;
    }

    public void SetCorrectSpriteInEditor()
    {
        _spriteRenderer.sprite = _isHeavy == false ? _regularInactive : _heavyInactive;
    }

#if UNITY_EDITOR
    //private void OnValidate()
    //{
    //    _spriteRenderer.sprite = _isHeavy == false ? _regularInactive : _heavyInactive;
    //}
#endif
}
