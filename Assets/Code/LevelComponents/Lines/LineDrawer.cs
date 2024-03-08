using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(LineController))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private LineController _controller;

    private Vector3 _visualA;
    private Vector3 _visualB;


    // Start is called before the first frame update
    void Start()
    {
        if (_renderer != null)
        {
            UpdateLinePositions();
            _renderer.startColor = _controller.LineColor;
            _renderer.endColor = _controller.LineColor;
        }
        else
        {
            Debug.LogWarning("No line renderer");
        }
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
        SetVisualEndpoints();

        _renderer.SetPosition(0, _visualA);
        _renderer.SetPosition(1, _visualB);
    }

    // When passing in, 0 means opaque, 1 means transparent
    public void SetAlpha(float alphaPercentage)
    {
        float newAlpha = 1f - alphaPercentage;

        _renderer.startColor = new Color(_renderer.startColor.r, _renderer.startColor.g, _renderer.startColor.b, newAlpha);
        _renderer.endColor = new Color(_renderer.endColor.r, _renderer.endColor.g, _renderer.endColor.b, newAlpha);
    }

    private void SetVisualEndpoints()
    {
        Vector3 slope = _controller.Slope;
        float halfWidth = _renderer.startWidth / 2;

        _visualA = _controller.CurrentA - (slope.normalized * halfWidth);
        _visualB = _controller.CurrentB + (slope.normalized * halfWidth);
    }

    public void HandleLineActiveChanged(bool active)
    {
        if(active == true)
        {
            SetAlpha(0f);
        }
        else
        {
            SetAlpha(1f);
        }
    }
}
