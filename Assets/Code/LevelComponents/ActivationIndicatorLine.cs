using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationIndicatorLine : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public Color NotActivatedColor;
    public Color ActivatedColor;

    [SerializeField] private bool _startActivated = false;
    private bool _activated = false;

    private void Awake()
    {
        SetActivated(_startActivated);
    }

    public void SetActivated(bool activated)
    {
        if (activated == _activated) return;

        _activated = activated;

        if(_activated == true)
        {
            LineRenderer.startColor = ActivatedColor;
            LineRenderer.endColor = ActivatedColor;
        }
        else
        {
            LineRenderer.startColor = NotActivatedColor;
            LineRenderer.endColor = NotActivatedColor;
        }
    }
}
