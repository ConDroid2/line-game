using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DevHacks_Teleport : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown _roomDropdown;
    [SerializeField] private TMP_Dropdown _roomPortDropdown;

    // Private Variables
    private List<TMP_Dropdown.OptionData> _allRoomOptions = new List<TMP_Dropdown.OptionData>();
    private WorldRoomData _roomDataForSelectedRoom;

    private void Awake()
    {
        if (GameManager.Instance == null) return;

        foreach(string roomName in GameManager.Instance.GetCurrentWorld().RoomNameToData.Keys)
        {
            _allRoomOptions.Add(new TMP_Dropdown.OptionData(roomName));
        }

        _roomDropdown.AddOptions(_allRoomOptions);

        HandleRoomDropdownChange(0);
    }

    // Event Handlers
    public void HandleRoomDropdownChange(int newValue)
    {
        // Figure out what connections we should be showing
        _roomDataForSelectedRoom = GameManager.Instance.GetCurrentWorld().RoomNameToData[_roomDropdown.options[newValue].text];

        var newPortOptions = new List<TMP_Dropdown.OptionData>();
        foreach (RoomConnection connection in _roomDataForSelectedRoom.RoomConnections)
        {
            newPortOptions.Add(new TMP_Dropdown.OptionData(connection.FromPort.SiblingIndex.ToString() + "_" + connection.FromPort.SideOfRoom.ToString()));
        }

        _roomPortDropdown.options = newPortOptions;
    }

    //public void HandlePortDropdownChange(int newValue)
    //{
    //    _selectedPort = _roomPortDropdown.options[newValue];
    //}

    public void HandleSearchChange(string newValue)
    {
        var newRoomOptions = new List<TMP_Dropdown.OptionData>();

        foreach(var roomOption in _allRoomOptions)
        {
            if (roomOption.text.ToLower().Contains(newValue.ToLower()))
            {
                newRoomOptions.Add(roomOption);
            }
        }

        _roomDropdown.options = newRoomOptions;
        HandleRoomDropdownChange(0);
    }

    public void HandleTelport()
    {

        foreach(var connection in _roomDataForSelectedRoom.RoomConnections)
        {
            string combinedPortName = connection.FromPort.SiblingIndex.ToString() + "_" + connection.FromPort.SideOfRoom.ToString();

            if(combinedPortName == _roomPortDropdown.options[_roomPortDropdown.value].text)
            {
                GameManager.Instance.MoveToRoomBasedOnPort(_roomDropdown.options[_roomDropdown.value].text, connection.FromPort);
                return;
            }
        }
    }


}
