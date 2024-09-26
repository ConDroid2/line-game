using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyHole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _keyCheckRadius;
    [SerializeField] private GameObject _unlockRadius;
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Events")]
    public UnityEvent OnUnlocked;
    private bool _unlocked = false;

    public void HandleKeyInRange(Collider2D collider)
    {
        
        if (_unlocked == false && collider.TryGetComponent(out KeyHolder keyHolder))
        {
            KeyObject keyObject = keyHolder.GetKey();

            if (keyObject == null) return;
                
            keyObject.SpringJoint.connectedBody = _rigidbody;
            Debug.Log("Calling HandleKeyInRange");
            _keyCheckRadius.SetActive(false);
            _unlockRadius.SetActive(true);

            

            
        }
    }

    public void HandleKeyUnlock(Collider2D collider)
    {
        Debug.Log("Calling HandleUnlockKey");
        Debug.Log(collider.name);
        if(_unlocked == false && collider.TryGetComponent(out KeyObject keyObject))
        {
            Debug.Log("Using key");
            keyObject.Use();
            UnlockKeyHole();
        }
        else
        {
            Debug.Log("Didn't find key");
        }
    }

    public void UnlockKeyHole()
    {
        OnUnlocked.Invoke();
        _animator.SetTrigger("Unlock");
        _unlocked = true;
        _unlockRadius.SetActive(false);
        _keyCheckRadius.SetActive(false);
    }
}
