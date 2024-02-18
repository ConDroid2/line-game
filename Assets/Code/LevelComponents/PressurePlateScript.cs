using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateScript : MonoBehaviour
{

    public Collider2D thisCollider;

    

    private bool isActivated = false;

    public UnityEvent OnPressed;
    public UnityEvent OnDeactivated;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CheckIfEncapsulated(other))
        {
            if (!isActivated)
            {
                Debug.Log("Pressed!");
                OnPressed.Invoke();
                isActivated = true;
            }
        }
    }

    private bool CheckIfEncapsulated(Collider2D other)
    {
        if (thisCollider.bounds.Contains(other.bounds.max) && thisCollider.bounds.Contains(other.bounds.min))
        {
            return true;
        }
        else if(isActivated)
        {
            isActivated = false;
            OnDeactivated.Invoke();
            return false;
        }
        else
        {
            return false;
        }
    }
}
