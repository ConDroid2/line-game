using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyHole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Events")]
    public UnityEvent OnUnlocked;
    private bool _unlocked = false;

    public void HandleKeyInRange(Collider2D collider)
    {
        if (_unlocked == false && collider.GetComponent<KeyObject>() != null)
        {
            collider.GetComponent<KeyObject>().Use();
            UnlockKeyHole();
        }
    }

    public void UnlockKeyHole()
    {
        OnUnlocked.Invoke();
        _animator.SetTrigger("Unlock");
        _unlocked = true;
    }
}
