using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDestructible : MonoBehaviour
{
    public UnityEvent OnDestroyed;

    public void Destruct()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
