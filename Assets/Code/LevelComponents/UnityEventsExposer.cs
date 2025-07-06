using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventsExposer : MonoBehaviour
{

    

    [Header("Physics Trigger Events")]
    public bool JustPlayer = false;
    public UnityEvent<Collider2D> OnTriggerEnter;
    public UnityEvent<Collider2D> OnTriggerExit;
    public UnityEvent<Collider2D> OnTriggerStay;

    [Header("Game Time Events")]
    public UnityEvent OnStart;
    public UnityEvent OnAwake;
    public UnityEvent OnEnabled;
    public UnityEvent OnDisabled;

    private void Start()
    {
        OnStart?.Invoke();
    }

    private void Awake()
    {
        OnAwake?.Invoke();
    }

    private void OnEnable()
    {
        OnEnabled?.Invoke();
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }


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
