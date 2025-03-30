using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorLine : MonoBehaviour
{
    [SerializeField] private LineController _lineController;
    [SerializeField] private float _magnitude;
    [SerializeField] [Range(-1, 1)] private int _direction;
    [SerializeField] private int _visualsFrequency;

    [SerializeField] private GameObject _directionVisualsPrefab;

    private List<Transform> _directionVisuals = new List<Transform>();
    private int _numberOfVisuals;
    private float _distanceBetweenVisuals;

    private bool _lineIsRotating = false;
    

    private void Start()
    {
        //_lineController = GetComponent<LineController>();

        _numberOfVisuals = (int)Mathf.Floor(_lineController.Length / _visualsFrequency);

        _distanceBetweenVisuals = _lineController.Length / _numberOfVisuals;

        for(int i = 0; i < _numberOfVisuals; i++)
        {
            GameObject newVisual = Instantiate(_directionVisualsPrefab, transform);

            _directionVisuals.Add(newVisual.transform);

            //Vector3 position = _lineController.CurrentA + ( (Vector3)(_lineController.Slope) * (_distanceBetweenVisuals * i));

            //newVisual.transform.position = position;
            //newVisual.transform.up = _lineController.Slope * _direction;
        }

        CalculateVisualPositionsAndPlace();
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

    private void LateUpdate()
    {
        if (_lineIsRotating)
        {
            CalculateVisualPositionsAndPlace();
        }
    }

    private void CalculateVisualPositionsAndPlace()
    {
        for(int i = 0; i <  _directionVisuals.Count; i++)
        {
            Transform visual = _directionVisuals[i];
            Vector3 position = _lineController.CurrentA + ((Vector3)(_lineController.Slope) * (_distanceBetweenVisuals * i));

            visual.transform.position = position;
            visual.transform.up = _lineController.Slope * _direction;
        }
    }

    public void HandleLineStartRotating()
    {
        _lineIsRotating = true;
    }

    public void HandleLineDoneRotating()
    {
        _lineIsRotating = false;
        CalculateVisualPositionsAndPlace();
    }
}
