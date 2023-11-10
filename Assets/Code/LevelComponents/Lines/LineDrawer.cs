using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(LineController))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private LineController _controller;


    // Start is called before the first frame update
    void Awake()
    {
        if (_renderer != null)
        {
            _renderer.SetPosition(0, _controller.InitialA);
            _renderer.SetPosition(1, _controller.InitialB);
        }
        else
        {
            Debug.LogWarning("No line renderer");
        }
    }

    private void Start()
    {
        UpdateLinePositions();
    }

    private void Update()
    {
        if(_controller.LineType == Enums.LineType.Shifting)
        {
            UpdateLinePositions();
        }
    }

    private void UpdateLinePositions()
    {
        _renderer.SetPosition(0, _controller.CurrentA);
        _renderer.SetPosition(1, _controller.CurrentB);
    }
}
