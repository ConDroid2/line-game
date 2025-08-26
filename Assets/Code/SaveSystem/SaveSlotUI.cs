using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class SaveSlotUI : MonoBehaviour
{
    public string SlotName = "";
    [SerializeField] private TextAsset _worldAsset;
    //[SerializeField] private TextMeshProUGUI _buttonText;
    public UnityEvent OnFileFound;
    public UnityEvent OnFileEmpty;

    private SaveSlot _saveData = null;
    private WorldData _worldData = null;

    public Image ShootIcon;
    public Image GrappleIcon;
    public Image RotateIcon;
    public Image FireshieldIcon;

    //private void Awake()
    //{
    //    if(SlotName != "")
    //    {
    //        _worldData = JsonConvert.DeserializeObject<WorldData>(_worldAsset.text);

    //        try
    //        {
    //            JsonUtilities utils = new JsonUtilities(Application.persistentDataPath + "/");

    //            _saveData = utils.LoadData<SaveSlot>(SlotName + ".txt");

    //            CheckFlagsAndSetIcons(_saveData);

    //            //_buttonText.text = "I have data";
    //            this.OnFileFound?.Invoke();
    //            //Debug.Log("Got data");
    //        }
    //        catch
    //        {
    //                this.OnFileEmpty?.Invoke();
    //            //_buttonText.text = "Empty";
    //            _saveData = new SaveSlot(SlotName, new Dictionary<string, bool>(), new HashSet<string>(), "", null, null, null, null);
    //        }
    //    }
    //}

    private void OnEnable()
    {
        if (SlotName != "")
        {
            _worldData = JsonConvert.DeserializeObject<WorldData>(_worldAsset.text);

            try
            {
                JsonUtilities utils = new JsonUtilities(Application.persistentDataPath + "/");

                _saveData = utils.LoadData<SaveSlot>(SlotName + ".txt");

                CheckFlagsAndSetIcons(_saveData);

                //_buttonText.text = "I have data";
                this.OnFileFound?.Invoke();
                //Debug.Log("Got data");
            }
            catch
            {
                this.OnFileEmpty?.Invoke();
                //_buttonText.text = "Empty";
                _saveData = new SaveSlot(SlotName, new Dictionary<string, bool>(), new HashSet<string>(), "", null, null, null, null);
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

    public void HandleSlotDeleted()
    {
        if(_saveData != null)
        {
            JsonUtilities utils = new JsonUtilities(Application.persistentDataPath + "/");

            utils.DeleteDataAt(SlotName + ".txt");

            _saveData = null;

            CheckFlagsAndSetIcons(_saveData);
        }
    }

    public void CheckFlagsAndSetIcons(SaveSlot saveData)
    {
        bool shootUnlocked = saveData == null ? false : saveData.Flags["ShootUnlocked"];
        bool grappleUnlocked = saveData == null ? false : saveData.Flags["GrappleUnlocked"];
        bool rotateUnlocked = saveData == null ? false : saveData.Flags["RotateUnlocked"];
        bool gotMcguffin = saveData == null ? false : saveData.Flags["GotMcGuffin"];

        this.ShootIcon.gameObject.SetActive(shootUnlocked);
        this.GrappleIcon.gameObject.SetActive(grappleUnlocked);
        this.RotateIcon.gameObject.SetActive(rotateUnlocked);
        this.FireshieldIcon.gameObject.SetActive(gotMcguffin);
    }
}
