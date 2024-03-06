using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPreview : MonoBehaviour
{
    public bool StartingRoom = false;
    public RoomData RoomData;
    public List<Vector3> RelativePorts;
    
    [Range(0, 1)] [SerializeField] private float _scale;

    [Header("Border Lines")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _overlappingColor;
    private Color _colorToUse;


    private void Awake()
    {
        Debug.Log($"I have room data: {RoomData != null}");
    }

    public void SetRoomData(RoomData newRoomData)
    {
        RoomData = new RoomData(newRoomData);

        RelativePorts = new List<Vector3>();
        foreach(RoomPort port in RoomData.Ports)
        {
            Vector3 relativePort = port.RelativePosition.ConvertToVector3() * _scale;
            RelativePorts.Add(relativePort);
        }

        SetDefaultColor(true);
    }

    public void SetDefaultColor(bool useDefault)
    {
        _colorToUse = useDefault ? _defaultColor : _overlappingColor;
    }

    public float WorldSpaceTop => transform.position.y + (RoomData.Top * _scale);
    public float WorldSpaceBottom => transform.position.y + (RoomData.Bottom * _scale);
    public float WorldSpaceLeft => transform.position.x + (RoomData.Left * _scale);
    public float WorldSpaceRight => transform.position.x + (RoomData.Right * _scale);


    private void OnDrawGizmos()
    {
        Vector3 topLeft = new Vector3(WorldSpaceLeft, WorldSpaceTop);
        Vector3 topRight = new Vector3(WorldSpaceRight, WorldSpaceTop);
        Vector3 bottomRight = new Vector3(WorldSpaceRight, WorldSpaceBottom);
        Vector3 bottomLeft = new Vector3(WorldSpaceLeft, WorldSpaceBottom);

        Gizmos.color = _colorToUse;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        Gizmos.color = Color.white;

        if (RoomData != null && RoomData.Lines != null)
        {
            foreach (LineData line in RoomData.Lines)
            {
                Gizmos.DrawLine(transform.position + (line.A.ConvertToVector3() * _scale), transform.position + (line.B.ConvertToVector3() * _scale));
            }
        }
    }
}
