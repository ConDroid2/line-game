using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private RectTransform _roomPrefab;

    [SerializeField] private Transform _mapParent;

    private void Awake()
    {
        JsonUtilities utils = new JsonUtilities("");

        WorldData _worldData = utils.LoadFromResources<WorldData>("Worlds/" + "WorldNegativeOne");

        Debug.Log(_worldData.Name);

        // Keep track of what rooms I already loaded (HashSet)
        // Loop through each Room
        Dictionary<string, RectTransform> loadedRoomNames = new Dictionary<string, RectTransform>();



        foreach(WorldRoomData room in _worldData.RoomNameToData.Values)
        {
            if (loadedRoomNames.ContainsKey(room.RoomName) == false)
            {
                RectTransform roomPreview = Instantiate(_roomPrefab, _mapParent);
                roomPreview.anchoredPosition = new Vector2(room.PreviewRoomPosition.x / 10f * 200, room.PreviewRoomPosition.y / 5.5f * 110);
                roomPreview.gameObject.name = room.RoomName;

                Debug.Log($"Room width/height = {room.RoomWidth} / {room.RoomHeight}");
                roomPreview.sizeDelta = new Vector2(room.RoomWidth / 20 * 200, room.RoomHeight / 11 * 110);

                loadedRoomNames.Add(room.RoomName, roomPreview);
            }
        }

    }
}
