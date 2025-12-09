using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnGameQuit : MonoBehaviour
{
    public UnityEvent GameQuit;

    public void FireEvent()
    {
        GameQuit?.Invoke();
    }
}
