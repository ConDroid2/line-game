using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLine : MonoBehaviour
{
    [SerializeField] private LineController _lineController;
    [SerializeField] private float _magnitude;
    [SerializeField] [Range(-1, 1)] private int _direction;

    [SerializeField] private Transform _directionVisuals;

    private void Start()
    {
        _lineController = GetComponent<LineController>();

        _directionVisuals.up = _lineController.Slope * _direction;

        Debug.Log($"Slope and direction = {_lineController.Slope * _direction}");
        Debug.Log($"Direction visuals up = {_directionVisuals.up}");
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
    }
}
