using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLine : MonoBehaviour
{
    [SerializeField] private LineController _lineController;
    [SerializeField] private float _magnitude;
    [SerializeField] [Range(-1, 1)] private int _direction;

    [SerializeField] private Shapes.Line _lineVisual;

    private bool _lineIsRotating = false;

    private void Awake()
    {
        _lineVisual.DashShapeModifier = _direction;
    }

    private void Update()
    {
        foreach(OnLineController onLine in _lineController.OnLineControllers)
        {
            if(onLine.TryGetComponent(out LineMovementController movementController))
            {
                movementController.AddForce(new Force(_magnitude, _lineController.Slope * _direction, 0f, gameObject), true);
            }
        }

        _lineVisual.DashOffset += _magnitude * Time.deltaTime * _direction;
    }


    public void HandleLineStartRotating()
    {
        _lineIsRotating = true;
    }

    public void HandleLineDoneRotating()
    {
        _lineIsRotating = false;
    }
}
