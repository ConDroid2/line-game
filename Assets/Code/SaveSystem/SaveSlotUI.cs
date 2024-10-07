using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotUI : MonoBehaviour
{
    public string SlotName = "";
    [SerializeField] private TextAsset _worldAsset;
    [SerializeField] private TextMeshProUGUI _buttonText;

    private SaveSlot _saveData = null;
    private WorldData _worldData = null;

    private void Awake()
    {
        if(SlotName != "")
        {
            _worldData = JsonConvert.DeserializeObject<WorldData>(_worldAsset.text);

            try
            {
                JsonUtilities utils = new JsonUtilities(Application.persistentDataPath + "/");

                _saveData = utils.LoadData<SaveSlot>(SlotName + ".txt");

                _buttonText.text = "I have data";
                Debug.Log("Got data");
            }
            catch
            {
                _buttonText.text = "No Data";
                _saveData = new SaveSlot(SlotName, new Dictionary<string, bool>(), new HashSet<string>(), "", null);
            }
        }
    }

    public void HandleSlotSelected()
    {
        string startingRoomName = "";

        if (_saveData != null)
        {
            GameManager.Instance?.HandleLoadedData(_saveData);
        }

        if(_saveData.CurrentRoomName != "")
        {
            startingRoomName = _saveData.CurrentRoomName;
        }
        else
        {
            foreach (string roomName in _worldData.RoomNameToData.Keys)
            {
                if (_worldData.RoomNameToData[roomName].StartRoom == true)
                {
                    startingRoomName = _worldData.RoomNameToData[roomName].RoomName;
                }
            }
        }

        Debug.Log($"Starting game with {startingRoomName}");
        GameManager.Instance?.SetCurrentWorld(_worldData);
        SceneManager.LoadScene(startingRoomName);
    }

    
}
