using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    [SerializeField] private bool _active;
    public void DebugLog(object message)
    {
        if (_active)
        {
            Debug.Log(message);
        }
    }
}
