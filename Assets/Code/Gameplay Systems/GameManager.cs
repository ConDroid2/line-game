using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;

    public static GameManager Instance;
    private WorldData _currentWorld;
    private WorldRoomData _currentRoom;
    private RoomPort _toPort;
    private Player _player;

    private void Awake()
    {
        // If no instance, this is instance, else destroy self
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
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

            if (_toPort.SideOfRoom == Enums.RoomSides.Top) startPosition = portPosition + new Vector3(0f, -0.25f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Bottom) startPosition = portPosition + new Vector3(0f, 0.25f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Left) startPosition = portPosition + new Vector3(0.25f, 0f, 0f);
            else if (_toPort.SideOfRoom == Enums.RoomSides.Right) startPosition = portPosition + new Vector3(-0.25f, 0f, 0f);

            float startingDistance = startPosition.InverseLerp(levelManager.StartingLine.InitialA, levelManager.StartingLine.InitialB);




            levelManager.SetStartingDistance(startingDistance);

            _toPort = null;
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
