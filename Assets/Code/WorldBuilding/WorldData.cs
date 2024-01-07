
using System.Collections.Generic;
[System.Serializable]
public class WorldData
{
    public string Name;
    public Dictionary<string, WorldRoomData> RoomNameToData;

    [Newtonsoft.Json.JsonConstructor]
    public WorldData(string name, Dictionary<string, WorldRoomData> roomNameToConnections)
    {
        Name = name;
        RoomNameToData = roomNameToConnections;
    }

    public WorldData(string name)
    {
        Name = name;
        RoomNameToData = new Dictionary<string, WorldRoomData>();
    }

    public void AddWorldRoomData(string roomName, WorldRoomData worldRoomData)
    {
        if(RoomNameToData.ContainsKey(roomName) == false)
        {
            RoomNameToData.Add(roomName, worldRoomData);
        }
    }
}
