using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WorldSelectorManager : MonoBehaviour
{
    [SerializeField] private WorldSelectorButton _worldButtonPrefab;
    [SerializeField] private Transform _buttonParent;
    private Dictionary<string, WorldData> _worldNameToData = new Dictionary<string, WorldData>();

    private void Awake()
    {
        JsonUtilities utils = new JsonUtilities("");

        // RUN THIS CODE WHEN IN EDITOR
        if (Application.isEditor)
        {
            foreach (string worldFilePath in System.IO.Directory.EnumerateFiles(Application.dataPath + "/Resources/Worlds", "*.txt"))
            {
                string fixedPath = worldFilePath.Replace("\\", "/");
                Debug.Log(worldFilePath.Replace("\\", "/"));
                WorldData worldData = utils.LoadData<WorldData>(fixedPath);
                _worldNameToData.Add(worldData.Name, worldData);

                WorldSelectorButton newButton = Instantiate<WorldSelectorButton>(_worldButtonPrefab, _buttonParent);
                newButton.WorldName = worldData.Name;
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = worldData.Name;
                newButton.WorldSelected.AddListener(LoadWorld);
            }
        }
        else
        {
            Debug.Log("About to grab worlds");
            foreach (WorldData worldData in utils.LoadAllFromResources<WorldData>("Worlds"))
            {
                _worldNameToData.Add(worldData.Name, worldData);

                WorldSelectorButton newButton = Instantiate<WorldSelectorButton>(_worldButtonPrefab, _buttonParent);
                newButton.WorldName = worldData.Name;
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = worldData.Name;
                newButton.WorldSelected.AddListener(LoadWorld);
            }
        }
    }

    public void LoadWorld(string worldName)
    {
        WorldData worldData = _worldNameToData[worldName];
        string startingRoomName = "";

        foreach(string roomName in worldData.RoomNameToData.Keys)
        {
            if(worldData.RoomNameToData[roomName].StartRoom == true)
            {
                startingRoomName = worldData.RoomNameToData[roomName].RoomName;
            }
        }


        GameManager.Instance?.SetCurrentWorld(worldData);
        SceneManager.LoadScene(startingRoomName);
    }
}
