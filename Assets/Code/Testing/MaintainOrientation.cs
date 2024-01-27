using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainOrientation : MonoBehaviour
{
    Vector3 initialUp;
    private void Awake()
    {
        initialUp = transform.up;
    }

    private void Update()
    {
        transform.up = initialUp;
    }
}
