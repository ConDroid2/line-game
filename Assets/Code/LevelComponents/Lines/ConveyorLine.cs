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

    private List<Transform> _directionVisuals;
    

    private void Start()
    {
        //_lineController = GetComponent<LineController>();

        int numberOfVisuals = (int)Mathf.Floor(_lineController.Length / _visualsFrequency);

        float distanceBetweenVisuals = _lineController.Length / numberOfVisuals;

        for(int i = 0; i < numberOfVisuals; i++)
        {
            GameObject newVisual = Instantiate(_directionVisualsPrefab, transform);

            Vector3 position = _lineController.CurrentA + ( (Vector3)(_lineController.Slope) * (distanceBetweenVisuals * i));

            newVisual.transform.position = position;
            newVisual.transform.up = _lineController.Slope * _direction;
        }
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
