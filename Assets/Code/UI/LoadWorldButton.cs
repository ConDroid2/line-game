using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWorldButton : MonoBehaviour
{
    public string WorldName = "";

    private WorldData _worldData;

    private void Awake()
    {
        if(WorldName != "")
        {
            JsonUtilities utils = new JsonUtilities("");

            _worldData = utils.LoadFromResources<WorldData>("Worlds/" + WorldName);
        }
        else
        {
            Debug.Log("No world set");
        }
    }

    public void LoadWorld()
    {
        string startingRoomName = "";

        foreach (string roomName in _worldData.RoomNameToData.Keys)
        {
            if (_worldData.RoomNameToData[roomName].StartRoom == true)
            {
                startingRoomName = _worldData.RoomNameToData[roomName].RoomName;
            }
        }


        GameManager.Instance?.SetCurrentWorld(_worldData);
        SceneManager.LoadScene(startingRoomName);
    }
}
