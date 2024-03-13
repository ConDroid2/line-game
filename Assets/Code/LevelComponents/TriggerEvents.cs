using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{

    public bool JustPlayer = false;

    public UnityEvent<Collider2D> OnTriggerEnter;
    public UnityEvent<Collider2D> OnTriggerExit;
    public UnityEvent<Collider2D> OnTriggerStay;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (JustPlayer && collision.CompareTag("Player") == false) return;

        OnTriggerEnter.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (JustPlayer && collision.CompareTag("Player") == false) return;

        OnTriggerExit.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (JustPlayer && collision.CompareTag("Player") == false) return;

        OnTriggerStay.Invoke(collision);
    }
}
