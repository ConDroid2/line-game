using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private string _flagsetName;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _devMenu;
    [SerializeField] private GameObject _map;

    public Dictionary<string, bool> Flags;

    public static GameManager Instance;
    private WorldData _currentWorld;
    private WorldRoomData _currentRoom;
    private string _currentRoomNameForSaveFile;
    private RoomPort _toPort;
    private Player _player;
    private bool _gamePaused = false;

    private HashSet<string> _visitedRooms = new HashSet<string>();

    private bool _firstSceneLoad = true;
    public SaveSlot.WwiseSwitchData PrimaryTrackData;
    public SaveSlot.WwiseSwitchData SecondaryTrackData;
    public string ControlOverridesJson = "";
    // Should this just be public? Lots of other classes want to mess with it
    private SaveSlot _saveSlot;

    public System.Action<string, bool> OnSetFlag;
    public UnityEngine.Events.UnityEvent OnFirstSceneLoad;

    private void Awake()
    {
        // If no instance, this is instance, else destroy self
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            JsonUtilities utils = new JsonUtilities("");
            FlagsClass flagNames;
            if (Application.isEditor)
            {
                flagNames = utils.LoadData<FlagsClass>(Application.dataPath + "/Resources/FlagMasterSets/" + _flagsetName + ".txt");
            }
            else
            {
                flagNames = utils.LoadFromResources<FlagsClass>("FlagMasterSets/" + _flagsetName);
            }

            Flags = new Dictionary<string, bool>();
            foreach(string name in flagNames.FlagNames)
            {
                bool flagStartingValue = false;

                if (name.Contains("_T"))
                {
                    //name = name.Replace("_T", "");
                    flagStartingValue = true;
                }

                Flags.Add(name.Replace("_T", ""), flagStartingValue);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //if(_saveSlot.TrackPrimary != null && AudioManager.Instance != null)
        //{
        //    Debug.Log("Setting primary track from save");
        //    _saveSlot.TrackPrimary.SetValue(AudioManager.Instance.SoundPlayer);
        //}

        //if(_saveSlot.TrackSecondary != null && AudioManager.Instance != null)
        //{
        //    Debug.Log("Setting secondary track from save");
        //    _saveSlot.TrackSecondary.SetValue(AudioManager.Instance.SoundPlayer);
        //}
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_player != null) _player.MovementController.OnReachedEdgeOfLine -= HandlePlayerReachedEdgeOfLine;
    }

    public void SetCurrentWorld(WorldData worldData)
    {
        _currentWorld = worldData;     
    }

    public WorldData GetCurrentWorld()
    {
        return _currentWorld;
    }

    public void SetFlag(string flagName, bool setFlagAs)
    {
        if (Flags.ContainsKey(flagName))
        {
            if(Flags[flagName] != setFlagAs)
            {
                Flags[flagName] = setFlagAs;
                OnSetFlag?.Invoke(flagName, setFlagAs);
            }
        }
    }

    public HashSet<string> GetVisitedRooms()
    {
        return _visitedRooms;
    }

    public WorldRoomData GetCurrentRoom()
    {
        return _currentRoom;
    }

    // Trigger Handlers
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        if(scene.name == "JPMenu_Main" || scene.name == "Scrolling Credits Scene")
        {
            if (_player != null) Destroy(_player.gameObject);

            _firstSceneLoad = true;
            return;
        }
        // Debug.Log($"Scene name: {scene.name}");
        // Get Level Manager if it exists
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if(levelManager != null && _toPort != null)
        {
            Transform lineTransform = levelManager.LineParent.transform.GetChild(_toPort.SiblingIndex);
            LineController newStartLine = lineTransform.GetComponent<LineController>();

            levelManager.StartingLine = newStartLine;

            Vector3 portPosition = _toPort.RelativePosition.ConvertToVector3();

            Vector3 startPosition = portPosition;

            if (Flags.ContainsKey("TopOfRoom"))
            {
                Flags["TopOfRoom"] = _toPort.SideOfRoom == Enums.RoomSides.Top;
            }
            if (Flags.ContainsKey("BottomOfRoom"))
            {
                Flags["BottomOfRoom"] = _toPort.SideOfRoom == Enums.RoomSides.Bottom;
            }
            if (Flags.ContainsKey("LeftOfRoom"))
            {
                Flags["LeftOfRoom"] = _toPort.SideOfRoom == Enums.RoomSides.Left;
            }
            if (Flags.ContainsKey("RightOfRoom"))
            {
                Flags["RightOfRoom"] = _toPort.SideOfRoom == Enums.RoomSides.Right;
            }

            if (_toPort.SideOfRoom == Enums.RoomSides.Top) startPosition = portPosition + new Vector3(0f, -0.25f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Bottom) startPosition = portPosition + new Vector3(0f, 0.25f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Left) startPosition = portPosition + new Vector3(0.25f, 0f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Right) startPosition = portPosition + new Vector3(-0.25f, 0f, 0f); 


            float startingDistance = startPosition.InverseLerp(levelManager.StartingLine.InitialA, levelManager.StartingLine.InitialB);




            levelManager.SetStartingDistance(startingDistance);

            // _toPort = null;

            
        }
        else if (levelManager != null && _toPort == null)
        {
            float startingDistance = levelManager.StartingPoint == Enums.LinePoints.A ? 0 : 1;
            levelManager.SetStartingDistance(startingDistance);
        }

        if(_player == null  && levelManager != null)
        {
            Debug.Log("Creating Player");
            _player = Instantiate(_playerPrefab);
            DontDestroyOnLoad(_player.gameObject);
        }

        if(_player != null)
        {
            _player.MovementController.OnReachedEdgeOfLine -= HandlePlayerReachedEdgeOfLine;
            _player.MovementController.OnReachedEdgeOfLine += HandlePlayerReachedEdgeOfLine;

            // I don't like this, maybe find a better way to do this later
            KeyObject[] keys = FindObjectsOfType<KeyObject>();

            foreach (KeyObject key in keys)
            {
                if (key.InUse)
                {
                    key.transform.position = _toPort.RelativePosition.ConvertToVector3();
                }
            }
        }

        
        if (_currentWorld == null) return;
        _currentRoom = _currentWorld.RoomNameToData[scene.name];

        // Else, leave the _toPort as is and set the _currentRoom to this room
        if (levelManager.DoNotAllowPlayerToLoadIntoThisRoom == false)   
        {
            _currentRoomNameForSaveFile = _currentRoom.RoomName;
        }

        _visitedRooms.Add(_currentRoom.RoomName);

        SaveData(levelManager);

        // Handle loading the correct music
        if (_firstSceneLoad)
        {
            Debug.Log($"Loading in correct track: {_saveSlot.TrackPrimary.SwitchGroup} -- {_saveSlot.TrackSecondary.SwitchGroup}");
            if (_saveSlot?.TrackPrimary != null)
                AkSoundEngine.SetSwitch(_saveSlot.TrackPrimary.SwitchGroup, _saveSlot.TrackPrimary.SwitchState, AudioManager.Instance.SoundPlayer);

            if(_saveSlot?.TrackSecondary != null) 
                AkSoundEngine.SetSwitch(_saveSlot.TrackSecondary.SwitchGroup, _saveSlot.TrackSecondary.SwitchState, AudioManager.Instance.SoundPlayer);
            // OnFirstSceneLoad?.Invoke();
            _firstSceneLoad = false;  
        }
    }

    public void HandlePlayerReachedEdgeOfLine(Vector3 playerPosition)
    {
        if (_currentRoom == null) return;

        foreach(RoomConnection roomConnection in _currentRoom.RoomConnections)
        {
            if(roomConnection.FromPort.RelativePosition.ConvertToVector3() == playerPosition)
            {
                _toPort = roomConnection.ToPort;
                SceneManager.LoadScene(roomConnection.ToLevelName);
            }
        }    
    }

    public void MoveToRoomBasedOnPort(string roomName, RoomPort port)
    {
        _toPort = port;
        SceneManager.LoadScene(roomName);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _gamePaused = true;
    }

    private void Unpause()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _gamePaused = false;
    }

    public void HandlePause()
    {
        Debug.Log("Handling Pause. Game Paused: " + _gamePaused);

        if (_devMenu.activeInHierarchy == true) return;

        if(_gamePaused == false)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    public void HandleDevMenu()
    {
        if (_gamePaused == true) return;

        if(_devMenu.activeInHierarchy == false)
        {
            Time.timeScale = 0;
            _devMenu.SetActive(true);
        }
        else{
            Time.timeScale = 1;
            _devMenu.SetActive(false);
        }
    }

    public void HandleMap()
    {
        if (_devMenu.activeInHierarchy == true || _gamePaused) return;

        if(_map.activeInHierarchy == false)
        {
            Time.timeScale = 0;
            _map.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _map.SetActive(false);
        }
    }

    public void SaveData(LevelManager levelManager)
    {
        if (_saveSlot == null) return;

        JObject controlOverrides = null;
        if(ControlOverridesJson != null && ControlOverridesJson != "")
        {
            controlOverrides = JObject.Parse(ControlOverridesJson);
        }

        RoomPort portToSave = levelManager.DoNotAllowPlayerToLoadIntoThisRoom ? _saveSlot.CurrentPort : _toPort;

        SaveSlot newSlot = new SaveSlot(_saveSlot.Name, Flags, _visitedRooms, _currentRoomNameForSaveFile, portToSave, PrimaryTrackData, SecondaryTrackData, controlOverrides);
        Debug.Log(newSlot.ControlOverridesJson);

        JsonUtilities utils = new JsonUtilities(Application.persistentDataPath + "/");

        utils.SaveData($"{newSlot.Name}.txt", newSlot);
    }

    public void HandleLoadedData(SaveSlot saveData)
    {
        _saveSlot = saveData;
        _toPort = saveData.CurrentPort;
        _visitedRooms = saveData.RoomsVisited;
        PrimaryTrackData = saveData.TrackPrimary;
        SecondaryTrackData = saveData.TrackSecondary;

        if(_saveSlot.ControlOverridesJson != null)
            InputManager.Instance?.LoadControlOverrides(_saveSlot.ControlOverridesJson.ToString());


        // Doing it this way allows us to add keys without breaking the save system
        foreach(string key in saveData.Flags.Keys)
        {
            if (Flags.ContainsKey(key))
            {
                Flags[key] = saveData.Flags[key];
            }
        }
    }
}
