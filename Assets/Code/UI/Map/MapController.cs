using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MapRoom _roomPrefab;
    [SerializeField] private RectTransform _verticalConnectionPrefab;
    [SerializeField] private RectTransform _horizontalConnectionPrefab;
    [SerializeField] private RectTransform _mapParent;
    [SerializeField] private Minimap _miniMapPrefab;

    [Header("Settings")]
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private Color _currentRoomColor;
    [SerializeField] private Color _regularRoomColor;
    [SerializeField] private MapRoom _currentRoom;

    // Private references
    private BaseControls _controls;

    private Dictionary<string, MapRoom> _rooms = new Dictionary<string, MapRoom>();

    private void Awake()
    {
        JsonUtilities utils = new JsonUtilities("");

        WorldData _worldData = utils.LoadFromResources<WorldData>("Worlds/" + "WorldNegativeOne");

        Debug.Log(_worldData.Name);

        // Keep track of what rooms I already loaded (HashSet)
        // Loop through each Room

        if(_rooms.Keys.Count == 0)
        {
            Minimap newMap = Instantiate(_miniMapPrefab, _mapParent);

            foreach(MapRoom room in newMap.MapRooms)
            {
                _rooms.Add(room.name, room);
                room.gameObject.SetActive(false);
            }
        }

       

    }

    private void Start()
    {
        if(InputManager.Instance != null)
        {
            _controls = InputManager.Instance.Controls;
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

    private void Update()
    {
        if (_controls != null && _controls.MapMap.enabled)
        {
            // The direction we want to move the map (we want to move it opposite of the input to make it seem like we're moving in the direction of the input
            Vector2 inputVector = _controls.MapMap.ScrollMap.ReadValue<Vector2>() * -1;

            Vector2 moveAmount = inputVector * _scrollSpeed * Time.unscaledDeltaTime;

            _mapParent.anchoredPosition += moveAmount;
        }
    }

    public void SetCurrentRoom(string roomName)
    {
        if(_currentRoom != null)
        {
            _currentRoom.SetColor(_regularRoomColor);
        }

        if (_rooms.ContainsKey(roomName))
        {
            _currentRoom = _rooms[roomName];
            _currentRoom.SetColor(_currentRoomColor);

            Vector2 shiftMapBy = _rooms[roomName].RectTransform.anchoredPosition * -1;
            Debug.Log($"Shift Map By: {shiftMapBy}");
            _mapParent.anchoredPosition = shiftMapBy;
        }

        
    }
}
