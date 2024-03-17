using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveWorld : EditorWindow
{
    RoomPreview[] _roomPreviews;
    string _oldWorldName;
    string _newWorldName;

    private void Awake()
    {  
        WorldBuildingManager manager = FindObjectOfType<WorldBuildingManager>();
        _oldWorldName = manager.CurrentWorldData.Name == null ? "" : manager.CurrentWorldData.Name;
        _newWorldName = _oldWorldName;
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Name", EditorStyles.boldLabel);
        _newWorldName = GUILayout.TextField(_newWorldName);

        if (GUILayout.Button("Finish Save"))
        {
            SaveWorldData();
            Close();
        }
    }

    public void SaveWorldData()
    {
        _roomPreviews = FindObjectsOfType<RoomPreview>();

        bool overlapsExist = CheckWorldForOverlappingRooms();
        RoomPreview startingRoom = CheckForStartingRoom();
        if (startingRoom == null || overlapsExist == true || _newWorldName == "") return;

        WorldData worldData = new WorldData(_newWorldName);

        foreach(RoomPreview room in _roomPreviews)
        {
            // Debug.Log($"Generating data for {room.name}");
            bool isStartRoom = room == startingRoom;
            List<RoomConnection> connectionsForRoom = FindConnectionsForRoom(room);
            SerializeableVector3 roomPreviewPosition = new SerializeableVector3(room.transform.position);
            string roomName = room.name;

            WorldRoomData worldRoomData = new WorldRoomData(connectionsForRoom, roomPreviewPosition, roomName, isStartRoom);
            worldData.AddWorldRoomData(roomName, worldRoomData);
        }

        Debug.Log("About to save data");
        JsonUtilities utils = new JsonUtilities(Application.dataPath + "/Resources/Worlds");
        utils.SaveData<WorldData>("/" + worldData.Name + ".txt", worldData);
    }


    private RoomPreview CheckForStartingRoom()
    {
        RoomPreview startingRoom = null;

        foreach (RoomPreview room in _roomPreviews)
        {
            if (room.StartingRoom == true && startingRoom == null)
            {
                startingRoom = room;
            }
            else if (room.StartingRoom == true && startingRoom != null)
            {
                EditorUtility.DisplayDialog("World Save Error", $"There is more than one starting room({startingRoom.name}, {room.name}). Make sure there is only one.", "Ok");
                return null;
            }
        }

        if (startingRoom == null)
        {
            EditorUtility.DisplayDialog("World Save Error", "There is no starting room. Please check the \"Starting Room\" box on one room", "Ok");
            return null;
        }

        return startingRoom;
    }

    private bool CheckWorldForOverlappingRooms()
    {
        bool overlapsExist = false;
        HashSet<RoomPreview> roomsWithOverlaps = new HashSet<RoomPreview>();

        foreach (RoomPreview room in _roomPreviews)
        {
            foreach (RoomPreview innerRoom in _roomPreviews)
            {
                if (room == innerRoom) continue;
                if (roomsWithOverlaps.Contains(room) && roomsWithOverlaps.Contains(innerRoom)) continue;

                if (CheckIfRoomsOverlap(room, innerRoom))
                {
                    roomsWithOverlaps.Add(room);
                    roomsWithOverlaps.Add(innerRoom);
                    overlapsExist = true;
                }

            }

            if (roomsWithOverlaps.Contains(room))
            {
                room.SetDefaultColor(false);
            }
            else
            {
                room.SetDefaultColor(true);
            }
        }

        return overlapsExist;
    }

    private bool CheckIfRoomsOverlap(RoomPreview roomA, RoomPreview roomB)
    {
        if (roomA.WorldSpaceLeft >= roomB.WorldSpaceRight || roomB.WorldSpaceLeft >= roomA.WorldSpaceRight)
        {
            return false;
        }
        else if (roomA.WorldSpaceBottom >= roomB.WorldSpaceTop || roomB.WorldSpaceBottom >= roomA.WorldSpaceTop)
        {
            return false;
        }

        return true;
    }

    private List<RoomConnection> FindConnectionsForRoom(RoomPreview room)
    {
        List<RoomConnection> connections = new List<RoomConnection>();

        foreach (RoomPreview innerRoom in _roomPreviews)
        {
            if (room == innerRoom) continue;

            foreach (Vector3 roomPort in room.RelativePorts)
            {
                foreach (Vector3 innerRoomPort in innerRoom.RelativePorts)
                {
                    Vector3 worldSpaceRoomPort = room.transform.position + roomPort;
                    Vector3 worldSpaceInnerRoomPort = innerRoom.transform.position + innerRoomPort;

                    if (worldSpaceRoomPort == worldSpaceInnerRoomPort)
                    {
                        // Debug.Log(room.RoomData);
                        // Debug.Log(room.RoomData.Ports[room.RelativePorts.IndexOf(roomPort)]);
                        RoomPort fromPort = room.RoomData.Ports[room.RelativePorts.IndexOf(roomPort)];
                        RoomPort toPort = innerRoom.RoomData.Ports[innerRoom.RelativePorts.IndexOf(innerRoomPort)];

                        RoomConnection newConnection = new RoomConnection(fromPort, innerRoom.name, toPort);
                        connections.Add(newConnection);
                    }
                }
            }
        }

        return connections;
    }
}
