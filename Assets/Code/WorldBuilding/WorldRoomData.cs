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

    [Newtonsoft.Json.JsonConstructor]
    public WorldRoomData(List<RoomConnection> roomConnections, SerializeableVector3 previewRoomPosition, string roomName, bool startRoom, int roomWidth, int roomHeight)
    {
        RoomConnections = roomConnections;
        PreviewRoomPosition = previewRoomPosition;
        RoomName = roomName;
        StartRoom = startRoom;
        RoomWidth = roomWidth;
        RoomHeight = roomHeight;
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
