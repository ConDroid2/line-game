using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MapRoom _roomPrefab;
    [SerializeField] private RectTransform _mapParent;

    [Header("Settings")]
    [SerializeField] private Color _currentRoomColor;
    [SerializeField] private Color _regularRoomColor;
    [SerializeField] private Image _currentRoomImage;

    private Dictionary<string, MapRoom> _rooms = new Dictionary<string, MapRoom>();

    private void Awake()
    {
        JsonUtilities utils = new JsonUtilities("");

        WorldData _worldData = utils.LoadFromResources<WorldData>("Worlds/" + "WorldNegativeOne");

        Debug.Log(_worldData.Name);

        // Keep track of what rooms I already loaded (HashSet)
        // Loop through each Room



        foreach(WorldRoomData room in _worldData.RoomNameToData.Values)
        {
            if (_rooms.ContainsKey(room.RoomName) == false)
            {
                MapRoom mapRoom = Instantiate(_roomPrefab, _mapParent.transform);
                mapRoom.RectTransform.anchoredPosition = new Vector2(room.PreviewRoomPosition.x / 10f * 200, room.PreviewRoomPosition.y / 5.5f * 110);
                mapRoom.gameObject.name = room.RoomName;

                mapRoom.RectTransform.sizeDelta = new Vector2(room.RoomWidth / 20 * 200, room.RoomHeight / 11 * 110);

                _rooms.Add(room.RoomName, mapRoom);

                mapRoom.gameObject.SetActive(false);
            }
        }

    }

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            foreach(string roomName in GameManager.Instance.GetVisitedRooms())
            {
                if (_rooms.ContainsKey(roomName))
                {
                    _rooms[roomName].gameObject.SetActive(true);
                }
            }

            SetCurrentRoom(GameManager.Instance.GetCurrentRoom().RoomName);
        }
    }

    public void SetCurrentRoom(string roomName)
    {
        if(_currentRoomImage != null)
        {
            _currentRoomImage.color = _regularRoomColor;
        }

        if (_rooms.ContainsKey(roomName))
        {
            _currentRoomImage = _rooms[roomName].BasicRoomImage;
            _currentRoomImage.color = _currentRoomColor;

            Vector2 shiftMapBy = _rooms[roomName].RectTransform.anchoredPosition * -1;
            Debug.Log($"Shift Map By: {shiftMapBy}");
            _mapParent.anchoredPosition = shiftMapBy;
        }

        
    }
}
