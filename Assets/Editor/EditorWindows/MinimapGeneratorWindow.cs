using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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
        JsonUtilities levelMetadataUtils = new JsonUtilities(Application.dataPath + "/Resources/LevelMetadata");
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

                mapRoom.AddRoomConnectionImages(newConnection.GetComponent<Image>());
            }

            foreach (float y in room.LeftMapConnectionYValues)
            {
                RectTransform newConnection = Instantiate(verticalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(room.MapRoomConnectionXValue * -1, y * 10);

                mapRoom.AddRoomConnectionImages(newConnection.GetComponent<Image>());
            }

            foreach (float x in room.TopMapConnectionXValues)
            {
                RectTransform newConnection = Instantiate(horizontalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(x * 10, room.MapRoomConnectionYValue);

                mapRoom.AddRoomConnectionImages(newConnection.GetComponent<Image>());
            }

            foreach (float x in room.BottomMapConnectionXValues)
            {
                RectTransform newConnection = Instantiate(horizontalConnectionPrefab, mapRoom.RectTransform);
                newConnection.anchoredPosition = new Vector2(x * 10, room.MapRoomConnectionYValue * -1);

                mapRoom.AddRoomConnectionImages(newConnection.GetComponent<Image>());
            }

            Debug.Log($"Loading metadata for : {room.RoomName}");
            RoomData roomMetadata = levelMetadataUtils.LoadData<RoomData>($"/{room.RoomName}-metadata.txt");

            PointOfInterestVisualsController visualsController = mapRoom.GetComponent<PointOfInterestVisualsController>();

            LoadPointsOfInterest(mapRoom.PointOfInterestParent, visualsController, roomMetadata);
            visualsController.CalculateAndSetPositions();

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

    private void LoadPointsOfInterest(RectTransform parent, PointOfInterestVisualsController visualsController, RoomData metadata)
    {
        if (metadata.PointsOfInterest == null) return;

        RectTransform keyPOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Key}.prefab");
        RectTransform lockPOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Lock}.prefab");
        RectTransform shootPOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Shoot}.prefab");
        RectTransform grapplePOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Grapple}.prefab");
        RectTransform rotatePOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Rotate}.prefab");
        RectTransform firewallPOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.Firewall}.prefab");
        RectTransform mcGuffinPOI = AssetDatabase.LoadAssetAtPath<RectTransform>($"Assets/Prefabs/UI/Map/PointsOfInterest/{Consts.MapRoomInfo.PointOfInterest.McGuffin}.prefab");

        foreach (PointOfInterestData pointOfInterest in metadata.PointsOfInterest)
        {
            RectTransform newPOI = null;

            switch (pointOfInterest.Type)
            {
                case 0:
                    newPOI = Instantiate(lockPOI, parent);
                    break;
                case 1:
                    newPOI = Instantiate(keyPOI, parent);
                    break;
                case 2:
                    newPOI = Instantiate(shootPOI, parent);
                    break;
                case 3:
                    newPOI = Instantiate(grapplePOI, parent);
                    break;
                case 4:
                    newPOI = Instantiate(rotatePOI, parent);
                    break;
                case 5:
                    newPOI = Instantiate(firewallPOI, parent);
                    break;
                case 6:
                    newPOI = Instantiate(mcGuffinPOI, parent);
                    break;
                default:
                    break;
            }

            // Set flag stuff on newPOI
            newPOI.GetComponent<FlagReader>().SetFlagName(pointOfInterest.FlagName);

            visualsController.AddPOI(newPOI);
        }
    }
}
