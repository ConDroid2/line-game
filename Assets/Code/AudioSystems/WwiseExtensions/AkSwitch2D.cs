using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkSwitch2D : MonoBehaviour
{
    public AK.Wwise.Switch Switch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Switch.SetValue(gameObject);
    }
}
