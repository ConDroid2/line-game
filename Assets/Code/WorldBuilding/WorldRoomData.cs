using System.Collections;
using System.Collections.Generic;

public class WorldRoomData
{
    public List<RoomConnection> RoomConnections;
    public SerializeableVector3 PreviewRoomPosition;
    public string RoomName;
    public bool StartRoom;
    public int RoomWidth;
    public int RoomHeight;

    public bool HasRightConnection;
    public bool HasLeftConnection;
    public bool HasBottomConnection;
    public bool HasTopConnection;

    public float MapRoomConnectionYValue;
    public float MapRoomConnectionXValue;
    public List<float> RightMapConnectionYValues;
    public List<float> LeftMapConnectionYValues;
    public List<float> TopMapConnectionXValues;
    public List<float> BottomMapConnectionXValues;

    [Newtonsoft.Json.JsonConstructor]
    public WorldRoomData(List<RoomConnection> roomConnections, SerializeableVector3 previewRoomPosition, string roomName, bool startRoom, 
        int roomWidth, int roomHeight, 
        float mapRoomConnectionXValue, float mapRoomConnectionYValue,
        List<float> rightMapConnectionYValues, List<float> leftMapConnectionYValues,
        List<float> topMapConnectionXValues, List<float> bottomMapConnectionXValues)
    {
        RoomConnections = roomConnections;
        PreviewRoomPosition = previewRoomPosition;
        RoomName = roomName;
        StartRoom = startRoom;
        RoomWidth = roomWidth;
        RoomHeight = roomHeight;
        MapRoomConnectionXValue = mapRoomConnectionXValue;
        MapRoomConnectionYValue = mapRoomConnectionYValue;
        RightMapConnectionYValues = rightMapConnectionYValues;
        LeftMapConnectionYValues = leftMapConnectionYValues;
        TopMapConnectionXValues = topMapConnectionXValues;
        BottomMapConnectionXValues = bottomMapConnectionXValues;
    }

    public WorldRoomData(SerializeableVector3 previewRoomPosition, string roomName)
    {
        PreviewRoomPosition = previewRoomPosition;
        RoomName = roomName;
        RoomConnections = new List<RoomConnection>();
    }

    public void AddRoomConnection(RoomConnection connection)
    {
        RoomConnections.Add(connection);
    }
}
