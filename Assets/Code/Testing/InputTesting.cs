using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTesting : MonoBehaviour
{
    BaseControls controls;

    private void Awake()
    {
        controls = new BaseControls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = controls.TestMap.Vector2.ReadValue<Vector2>();
        transform.position = newPos;
    }
}
