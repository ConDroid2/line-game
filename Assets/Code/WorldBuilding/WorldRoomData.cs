using System.Collections;
using System.Collections.Generic;

public class WorldRoomData
{
    public List<RoomConnection> RoomConnections;
    public SerializeableVector3 PreviewRoomPosition;
    public string RoomName;
    public bool StartRoom;

    [Newtonsoft.Json.JsonConstructor]
    public WorldRoomData(List<RoomConnection> roomConnections, SerializeableVector3 previewRoomPosition, string roomName, bool startRoom)
    {
        RoomConnections = roomConnections;
        PreviewRoomPosition = previewRoomPosition;
        RoomName = roomName;
        StartRoom = startRoom;
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
