using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private string _flagsetName;

    public Dictionary<string, bool> Flags;

    public static GameManager Instance;
    private WorldData _currentWorld;
    private WorldRoomData _currentRoom;
    private RoomPort _toPort;
    private Player _player;

    public System.Action<string, bool> OnSetFlag;

    private void Awake()
    {
        // If no instance, this is instance, else destroy self
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            JsonUtilities utils = new JsonUtilities(Application.dataPath + "/FlagMasterSets");
            FlagsClass flagNames = utils.LoadData<FlagsClass>("/" + _flagsetName + ".txt");

            Flags = new Dictionary<string, bool>();
            foreach(string name in flagNames.FlagNames)
            {
                Flags.Add(name, false);
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
            _player.MovementController.OnReachedEdgeOfLine += HandlePlayerReachedEdgeOfLine;
        }

        if (_currentWorld == null) return;

        _currentRoom = _currentWorld.RoomNameToData[scene.name];

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
}
