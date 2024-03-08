using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour
{
    public void HandleKeyInRange(Collider2D collider)
    {
        if (collider.GetComponent<KeyObject>() != null)
        {
            Debug.Log("Key In Range");
        }
    }
}
