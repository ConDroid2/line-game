using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldBuilder : EditorWindow
{
    [MenuItem("Custom/World Builder")]
    public static void ShowWindow()
    {
        GetWindow<WorldBuilder>();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Add Room"))
        {
            AddRoom();
        }

        if(GUILayout.Button("Save World"))
        {
            GetWindow<SaveWorld>();
        }
        if(GUILayout.Button("Load World"))
        {
            LoadWorld();
        }
        if(GUILayout.Button("Close World"))
        {
            CloseWorld();
        }
    }

    private void AddRoom()
    {
        string path = EditorUtility.OpenFilePanel("Select Room", "Assets/Resources/LevelMetadata", "txt");

        string roomName = System.IO.Path.GetFileName(path).Replace("-metadata.txt", "");

        JsonUtilities utils = new JsonUtilities("");
        //RoomData roomData = utils.LoadFromResources<RoomData>("LevelMetadata/" + roomName + "-metadata");
        RoomData roomData = utils.LoadData<RoomData>(path);

        RoomPreview roomPreviewPrefab = AssetDatabase.LoadAssetAtPath<RoomPreview>("Assets/Prefabs/WorldBuilding/RoomPreview.prefab");

        RoomPreview roomPreview = (RoomPreview)PrefabUtility.InstantiatePrefab(roomPreviewPrefab);
        roomPreview.name = roomName;

        roomPreview.SetRoomData(roomData);
        PrefabUtility.RecordPrefabInstancePropertyModifications(roomPreview);
        EditorUtility.SetDirty(roomPreview);
        Debug.Log($"Is {roomPreview.name} dirty? {EditorUtility.IsDirty(roomPreview)}");
    }

    private void LoadWorld()
    {
        if (CloseWorld() == false) return;
        string path = EditorUtility.OpenFilePanel("Select World", "Assets/Resources/Worlds", "txt");

        string worldName = System.IO.Path.GetFileName(path).Replace(".txt", "");

        Debug.Log(worldName);

        JsonUtilities worldUtils = new JsonUtilities("");
        WorldData worldData = worldUtils.LoadData<WorldData>(path);
        // WorldData worldData = worldUtils.LoadFromResources<WorldData>("Worlds/" + worldName);

        Debug.Log(worldData.ToString());

        

        JsonUtilities roomUtils = new JsonUtilities(Application.dataPath + "/Resources/LevelMetadata");
        RoomPreview roomPreviewPrefab = AssetDatabase.LoadAssetAtPath<RoomPreview>("Assets/Prefabs/WorldBuilding/RoomPreview.prefab");

        foreach (string roomName in worldData.RoomNameToData.Keys)
        {
            WorldRoomData worldRoomData = worldData.RoomNameToData[roomName];

            string roomPath = "/" + roomName + "-metadata.txt";
            RoomData roomData = roomUtils.LoadData<RoomData>(roomPath);

            //string roomPath = roomName + "-metadata";
            //RoomData roomData = roomUtils.LoadFromResources<RoomData>("LevelMetadata/" + roomPath);

            RoomPreview roomPreview = (RoomPreview)PrefabUtility.InstantiatePrefab(roomPreviewPrefab);
            roomPreview.name = roomName;

            roomPreview.SetRoomData(roomData);
            roomPreview.transform.position = worldRoomData.PreviewRoomPosition.ConvertToVector3();
            roomPreview.StartingRoom = worldRoomData.StartRoom;
            
        }

        WorldBuildingManager manager = FindObjectOfType<WorldBuildingManager>();
        manager.SetCurrentWorldData(worldData);
    }

    private bool CloseWorld()
    {
        bool closeWorld = EditorUtility.DisplayDialog("Close World", "This will close the current world. Do you wish to proceed?", "Yes", "No");

        if (closeWorld)
        {
            RoomPreview[] rooms = FindObjectsOfType<RoomPreview>();

            for(int i = 0; i < rooms.Length; i++)
            {
                DestroyImmediate(rooms[i].gameObject);
            }

            FindObjectOfType<WorldBuildingManager>().SetCurrentWorldData(null);
        }

        return closeWorld;
    }
}
