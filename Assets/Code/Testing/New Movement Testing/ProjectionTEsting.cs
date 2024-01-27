using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionTEsting : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LineController line;
    public Transform projectionPoint;

    public BaseControls baseControls;

    private void Awake()
    {
        baseControls = new BaseControls();
        baseControls.TestMap.Enable();
    }

    private void Update()
    {
        Vector2 input = baseControls.TestMap.Vector2.ReadValue<Vector2>();
        Vector3 endpoint = transform.position + new Vector3(input.x, input.y, 0f);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endpoint);

        Vector3 lineNormal = line.transform.forward;

        Vector3 projection = Vector3.Project(endpoint, lineNormal);

        projectionPoint.position = projection;

        Debug.Log(projection);
    }
}
