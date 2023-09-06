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
            _renderer.SetPosition(0, _controller.A);
            _renderer.SetPosition(1, _controller.B);
        }
        else
        {
            Debug.LogWarning("No line renderer");
        }
    }

    private void Update()
    {
        if (_controller.LineShifters.Length > 0 && _renderer != null)
        {
            _renderer.SetPosition(0, _controller.A);
            _renderer.SetPosition(1, _controller.B);
        }
    }
}
