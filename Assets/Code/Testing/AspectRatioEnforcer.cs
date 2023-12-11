using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    public float width = 16;
    public float height = 9;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        // Calculate target aspect ratio
        float targetAspectRatio = width / height;
        // Calculate screen's aspect ratio
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;

        // Get scalar for the viewport
        float targetViewportScale = screenAspectRatio / targetAspectRatio;

        // If scalar is < 1, we need bars on top and bottom
        if(targetViewportScale < 1.0f)
        {
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = targetViewportScale;
            rect.x = 0;
            rect.y = (1.0f - targetViewportScale) / 2.0f;

            _camera.rect = rect;
        }
        // If scalar is > 1, we need bars on the sides
        else
        {
            float scalewidth = 1.0f / targetViewportScale;

            Rect rect = _camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }
    }
}
