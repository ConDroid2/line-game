using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateScript : MonoBehaviour
{

    public Collider2D thisCollider;

    public UnityEvent OnPressed;

    private bool isActivated = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("triggered");

        if (other.tag == "ActivatesPlate")
        {
            if (CheckIfEncapsulated(other))
            {
                Debug.Log("Pressed!");
                if (!isActivated)
                {
                    OnPressed.Invoke();
                    isActivated = true;
                }
            }
        }
    }

    private bool CheckIfEncapsulated(Collider2D other)
    {
        if (thisCollider.bounds.Contains(other.bounds.max) && thisCollider.bounds.Contains(other.bounds.min))
        {
            return true;
        }
        else
        {
            isActivated = false;
            return false;
        }
    }
}
