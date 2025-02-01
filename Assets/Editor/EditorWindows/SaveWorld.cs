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
            // if (room.name.Contains("000") == false) continue;
            // Debug.Log($"Generating data for {room.name}");
            bool isStartRoom = room == startingRoom;
            List<RoomConnection> connectionsForRoom = FindConnectionsForRoom(room);
            SerializeableVector3 roomPreviewPosition = new SerializeableVector3(room.transform.position);
            string roomName = room.name;

            int roomWidth = (int)(room.RoomData.Right - room.RoomData.Left);
            int roomHeight = (int)(room.RoomData.Top - room.RoomData.Bottom);

            bool hasRightConnection = false;
            bool hasLeftConnection = false;
            bool hasBottomConnection = false;
            bool hasTopConnection = false;

            int roomUnitHeight = roomHeight / Consts.MapRoomInfo.PreviewRoomHeight;
            int roomUnitWidth = roomWidth / Consts.MapRoomInfo.PreviewRoomWidth;

            List<float> rightMapConnectionYValues = new List<float>();
            List<float> leftMapConnectionYValues = new List<float>();
            List<float> bottomMapConnectionXValues = new List<float>();
            List<float> topMapConnectionXValues = new List<float>();

            float[] heightSections = new float[roomUnitHeight + 1];
            float[] widthSections = new float[roomUnitWidth + 1];

            float mapRoomConnectionX = Consts.MapRoomInfo.MapRoomConnectionDefaultX * roomUnitWidth;
            float mapRoomConnectionY = Consts.MapRoomInfo.MapRoomConnectionDefaultY * roomUnitHeight;

            for(int i = 0; i < roomUnitHeight + 1; i++)
            {
                heightSections[i] = room.RoomData.Bottom + (Consts.MapRoomInfo.PreviewRoomHeight * i);
            }

            for(int i = 0; i < roomUnitWidth  + 1; i++)
            {
                widthSections[i] = room.RoomData.Left + (Consts.MapRoomInfo.PreviewRoomWidth * i);
            }

            // Debug.Log($"Room bounds are: {room.WorldSpaceRight}, {room.WorldSpaceLeft}, {room.WorldSpaceBottom}, {room.WorldSpaceTop}");
            foreach (RoomConnection roomConnection in connectionsForRoom)
            {
                Vector3 connectionWorldSpace = room.transform.position + (roomConnection.FromPort.RelativePosition.ConvertToVector3() * room.GetScale());

                
                if(room.WorldSpaceRight == connectionWorldSpace.x)
                {
                    for(int i = 0; i < heightSections.Length; i++)
                    {
                        if (i == 0) continue;

                        if(roomConnection.FromPort.RelativePosition.y > heightSections[i - 1] && roomConnection.FromPort.RelativePosition.y < heightSections[i])
                        {
                            rightMapConnectionYValues.Add((heightSections[i - 1] + heightSections[i]) / 2);
                            break;
                        }
                    }
                }
                else if(room.WorldSpaceLeft == connectionWorldSpace.x)
                {
                    for(int i = 0; i < heightSections.Length; i++)
                    {
                        if (i == 0) continue;

                        if (room.name.Contains("W1Tower")) Debug.Log($"Checking if {roomConnection.FromPort.RelativePosition.y} is between {heightSections[i - 1]} and {heightSections[i]}");

                        if (roomConnection.FromPort.RelativePosition.y > heightSections[i - 1] && roomConnection.FromPort.RelativePosition.y < heightSections[i])
                        {
                            if (room.name.Contains("W1Tower")) Debug.Log($"Adding a left connection at {(heightSections[i - 1] + heightSections[i]) / 2}");
                            leftMapConnectionYValues.Add((heightSections[i - 1] + heightSections[i]) / 2);
                            break;
                        }
                    }
                }
                else if(room.WorldSpaceBottom == connectionWorldSpace.y)
                {
                    for(int i = 0; i < widthSections.Length; i++)
                    {
                        if (i == 0) continue;

                        if (roomConnection.FromPort.RelativePosition.x > widthSections[i - 1] && roomConnection.FromPort.RelativePosition.x < widthSections[i])
                        {
                            
                            bottomMapConnectionXValues.Add((widthSections[i - 1] + widthSections[i]) / 2);
                            break;
                        }
                    }
                }
                else if(room.WorldSpaceTop == connectionWorldSpace.y)
                {
                    for(int i = 0; i < widthSections.Length; i++)
                    {
                        if (i == 0) continue;

                        if (roomConnection.FromPort.RelativePosition.x > widthSections[i - 1] && roomConnection.FromPort.RelativePosition.x < widthSections[i])
                        {
                            topMapConnectionXValues.Add((widthSections[i - 1] + widthSections[i]) / 2);
                            break;
                        }
                    }
                }

                hasRightConnection |= room.WorldSpaceRight == connectionWorldSpace.x;
                hasLeftConnection |= room.WorldSpaceLeft == connectionWorldSpace.x;
                hasBottomConnection |= room.WorldSpaceBottom == connectionWorldSpace.y;
                hasTopConnection |= room.WorldSpaceTop == connectionWorldSpace.y;
            }

            WorldRoomData worldRoomData = new WorldRoomData(connectionsForRoom, roomPreviewPosition, roomName, isStartRoom, roomWidth, roomHeight,
                mapRoomConnectionX, mapRoomConnectionY, rightMapConnectionYValues, leftMapConnectionYValues, topMapConnectionXValues, bottomMapConnectionXValues);
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
