using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationIndicatorLine : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public Color NotActivatedColor;
    public Color ActivatedColor;

    [SerializeField] Shapes.Polyline _innerLineShape;
    [SerializeField] Shapes.Polyline _outerLineShape;

    [SerializeField] private bool _startActivated = false;
    private bool _activated = false;

    private void Awake()
    {
        SetActivated(_startActivated);

        List<Shapes.PolylinePoint> points = new List<Shapes.PolylinePoint>();

        for(int i = 0; i < LineRenderer.positionCount; i++)
        {
            Shapes.PolylinePoint polylinePoint;

            polylinePoint = new Shapes.PolylinePoint(transform.InverseTransformPoint(LineRenderer.GetPosition(i)));
            points.Add(polylinePoint);
        }

        _innerLineShape.points = points;
        _innerLineShape.meshOutOfDate = true;
        _outerLineShape.points = points;
        _outerLineShape.meshOutOfDate = true;

        LineRenderer.enabled = false;
    }

    public void SetActivated(bool activated)
    {
        if (activated == _activated) return;

        _activated = activated;

        if(_activated == true)
        {
            LineRenderer.startColor = ActivatedColor;
            LineRenderer.endColor = ActivatedColor;

            _innerLineShape.Color = ActivatedColor;
        }
        else
        {
            LineRenderer.startColor = NotActivatedColor;
            LineRenderer.endColor = NotActivatedColor;

            _innerLineShape.Color = NotActivatedColor;
        }
    }
}
