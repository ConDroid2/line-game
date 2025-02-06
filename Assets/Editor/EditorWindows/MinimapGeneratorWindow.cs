using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MinimapGeneratorWindow : EditorWindow
{
    [MenuItem("Custom/Minimap Generator")]
    public static void ShowWindow()
    {
        GetWindow<MinimapGeneratorWindow>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Minimap"))
        {
            GenerateMinimap();
        }
    }

    private void GenerateMinimap()
    {
        string path = EditorUtility.OpenFilePanel("Select World to Generate Map For", "Assets/Resources/Worlds", "txt");

        string worldName = System.IO.Path.GetFileName(path).Replace(".txt", "");

        JsonUtilities worldUtils = new JsonUtilities("");
        WorldData worldData = worldUtils.LoadData<WorldData>(path);

        // Grab map room prefab
        MapRoom mapRoomPrefab = AssetDatabase.LoadAssetAtPath<MapRoom>("Assets/Prefabs/UI/Map/Room.prefab");

        // Grab map room connection prefabs
        RectTransform horizontalConnectionPrefab = AssetDatabase.LoadAssetAtPath<RectTransform>("Assets/Prefabs/UI/Map/HorizontalConnection.prefab");
        RectTransform verticalConnectionPrefab = AssetDatabase.LoadAssetAtPath<RectTransform>("Assets/Prefabs/UI/Map/VerticalConnection.prefab");

        // Create GameObjects
        GameObject canvas = new GameObject("Minimap Canvas", typeof(Canvas));
        GameObject mapParent = new GameObject($"{worldName}_MiniMap", typeof(RectTransform), typeof(Minimap));
        Minimap minimapScript = mapParent.GetComponent<Minimap>();
        mapParent.transform.parent = canvas.transform;

        foreach (WorldRoomData room in worldData.RoomNameToData.Values)
        {
            MapRoom mapRoom = Instantiate(mapRoomPrefab, mapParent.transform);
            mapRoom.RectTransform.anchoredPosition = new Vector2(room.PreviewRoomPosition.x / 10f * Consts.RoomInfo.RoomWidth, room.PreviewRoomPosition.y / 5.5f * Consts.RoomInfo.RoomHeight);
            mapRoom.gameObject.name = room.RoomName;

            mapRoom.BackgroundVisualsTransform.sizeDelta = new Vector2(room.RoomWidth / 20 * Consts.RoomInfo.RoomWidth, room.RoomHeight / 11 * Consts.RoomInfo.RoomHeight);

            // _rooms.Add(room.RoomName, mapRoom);

            foreach (float y in room.RightMapConnectionYValues)
            {
                RectTransform newConnection = Instantiate(verticalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(room.MapRoomConnectionXValue, y * 10);
            }

            foreach (float y in room.LeftMapConnectionYValues)
            {
                RectTransform newConnection = Instantiate(verticalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(room.MapRoomConnectionXValue * -1, y * 10);
            }

            foreach (float x in room.TopMapConnectionXValues)
            {
                RectTransform newConnection = Instantiate(horizontalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(x * 10, room.MapRoomConnectionYValue);
            }

            foreach (float x in room.BottomMapConnectionXValues)
            {
                RectTransform newConnection = Instantiate(horizontalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(x * 10, room.MapRoomConnectionYValue * -1);
            }

            // mapRoom.SetConnectionImages(room.HasRightConnection, room.HasLeftConnection, room.HasBottomConnection, room.HasTopConnection);
            minimapScript.MapRooms.Add(mapRoom);
        }

        PrefabUtility.SaveAsPrefabAsset(mapParent, $"Assets/Prefabs/UI/Map/ActualMaps/{mapParent.name}.prefab", out bool success);

        if (success)
        {
            EditorUtility.DisplayDialog("Success", "Minimap generated successfully!", "Ok");
            DestroyImmediate(canvas);
        }  
    }
}
