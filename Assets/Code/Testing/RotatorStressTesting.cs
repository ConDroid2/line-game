using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorStressTesting : MonoBehaviour
{
    [SerializeField] LineRotator _lineRotator;
    [SerializeField] LineController _lineController;

    int numberOfRotations = 0;

    // Update is called once per frame
    void Update()
    {
        _lineRotator.Rotate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Rotations: {numberOfRotations}");

            Debug.Log($"{_lineController.CurrentA.y}");
            Debug.Log($"{System.Math.Round((decimal)_lineController.CurrentA.y, 2)}");
        }
    }

    public void HandleRotationDone()
    {
        numberOfRotations++;
    }
}
