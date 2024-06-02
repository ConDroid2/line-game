using System.Collections;
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
    private RoomPort _toPort;
    private Player _player;
    private bool _gamePaused = false;

    private HashSet<string> _visitedRooms = new HashSet<string>();

    public System.Action<string, bool> OnSetFlag;

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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_player != null) _player.MovementController.OnReachedEdgeOfLine -= HandlePlayerReachedEdgeOfLine;
    }

    public void SetCurrentWorld(WorldData worldData)
    {
        _currentWorld = worldData;     
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

    // Trigger Handlers
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene name: {scene.name}");
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

        _visitedRooms.Add(_currentRoom.RoomName);

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
}
