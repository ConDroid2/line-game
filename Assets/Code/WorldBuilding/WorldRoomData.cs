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

    [Newtonsoft.Json.JsonConstructor]
    public WorldRoomData(List<RoomConnection> roomConnections, SerializeableVector3 previewRoomPosition, string roomName, bool startRoom, int roomWidth, int roomHeight, bool hasRightConnection, bool hasLeftConnection, bool hasBottomConnection, bool hasTopConnection)
    {
        RoomConnections = roomConnections;
        PreviewRoomPosition = previewRoomPosition;
        RoomName = roomName;
        StartRoom = startRoom;
        RoomWidth = roomWidth;
        RoomHeight = roomHeight;
        HasRightConnection = hasRightConnection;
        HasLeftConnection = hasLeftConnection;
        HasBottomConnection = hasBottomConnection;
        HasTopConnection = hasTopConnection;
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
