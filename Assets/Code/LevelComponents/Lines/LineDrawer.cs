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

    // When passing in, 0 means opaque, 1 means transparent
    public void SetAlpha(float alphaPercentage)
    {
        float newAlpha = 1f - alphaPercentage;

        _renderer.startColor = new Color(_renderer.startColor.r, _renderer.startColor.g, _renderer.startColor.b, newAlpha);
        _renderer.endColor = new Color(_renderer.endColor.r, _renderer.endColor.g, _renderer.endColor.b, newAlpha);
    }
}
