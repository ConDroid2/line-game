using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHub : MonoBehaviour
{
    public UnityEvent OnEventHubFired;

    public void FireEventHub()
    {
        OnEventHubFired?.Invoke();
    }
}
