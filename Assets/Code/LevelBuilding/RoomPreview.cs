using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPreview : MonoBehaviour
{
    public bool StartingRoom = false;
    public RoomData RoomData;
    public List<Vector3> RelativePorts;
    
    [Range(0, 1)] [SerializeField] private float _scale;
    [SerializeField] private LineRenderer _lineRendererPrefab;

    [Header("Border Lines")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _overlappingColor;
    [SerializeField] private LineRenderer _top;
    [SerializeField] private LineRenderer _left;
    [SerializeField] private LineRenderer _bottom;
    [SerializeField] private LineRenderer _right;


    private void Awake()
    {
        Debug.Log($"I have room data: {RoomData != null}");
    }

    public void SetRoomData(RoomData newRoomData)
    {
        RoomData = new RoomData(newRoomData);

        // Build the boarders
        ArrangeBorders();

        foreach(LineData line in RoomData.Lines)
        {
            LineRenderer lineRenderer = Instantiate<LineRenderer>(_lineRendererPrefab, transform);

            lineRenderer.SetPosition(0, line.A.ConvertToVector3() * _scale);
            lineRenderer.SetPosition(1, line.B.ConvertToVector3() * _scale);
        }

        RelativePorts = new List<Vector3>();
        foreach(RoomPort port in RoomData.Ports)
        {
            Vector3 relativePort = port.RelativePosition.ConvertToVector3() * _scale;
            RelativePorts.Add(relativePort);
        }

        SetDefaultColor(true);
    }

    private void ArrangeBorders()
    {
        float left = RoomData.Left * _scale;
        float right = RoomData.Right * _scale;
        float top = RoomData.Top * _scale;
        float bottom = RoomData.Bottom * _scale;

        Vector3 pos = transform.position;

        Vector3 topLeft = new Vector3(pos.x + left, pos.y + top, 0f);
        Vector3 bottomLeft = new Vector3(pos.x + left, pos.y + bottom, 0f);
        Vector3 bottomRight = new Vector3(pos.x + right, pos.y + bottom, 0f);
        Vector3 topRight = new Vector3(pos.x + right, pos.y + top, 0f);


        _top.SetPositions(new Vector3[] { topLeft, topRight });
        _left.SetPositions(new Vector3[] { topLeft, bottomLeft });
        _bottom.SetPositions(new Vector3[] { bottomLeft, bottomRight });
        _right.SetPositions(new Vector3[] { bottomRight, topRight });
    }

    public void SetDefaultColor(bool useDefault)
    {
        Color newColor = useDefault ? _defaultColor : _overlappingColor;

        _top.startColor = newColor;
        _top.endColor = newColor;

        _bottom.startColor = newColor;
        _bottom.endColor = newColor;

        _left.startColor = newColor;
        _left.endColor = newColor;

        _right.startColor = newColor;
        _right.endColor = newColor;
    }

    public float WorldSpaceTop => transform.position.y + (RoomData.Top * _scale);
    public float WorldSpaceBottom => transform.position.y + (RoomData.Bottom * _scale);
    public float WorldSpaceLeft => transform.position.x + (RoomData.Left * _scale);
    public float WorldSpaceRight => transform.position.x + (RoomData.Right * _scale);
}
