using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Linq;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    [Header("Public References")]
    public GameObject LineParent;
    public GameObject DangerZoneParent;
    public GameObject MiscLevelComponentsParent;

    [Header("Room Settings")]
    [Tooltip("Height of room in Room Units")] public int RoomHeight = 1;
    [Tooltip("Width of Room in Room Units")] public int RoomWidth = 1;
    

    [Header("Likely to be made private")]
    [SerializeField] Player _playerPrefab;
    private Player _player;
    public LineController[] Lines;

    [Header("Camera Stuff")]
    [SerializeField] CinemachineVirtualCamera _staticVirtualCamera;
    [SerializeField] CinemachineVirtualCamera _dynamicVirtualCamera;
    private CinemachineVirtualCamera _inUseCamera;
    [SerializeField] PolygonCollider2D _cameraBounds;

    

    [Header("Misc")]
    public LineController StartingLine;
    public float _startingDistance;
    public Enums.LinePoints StartingPoint = Enums.LinePoints.A;
    [HideInInspector] public float ObjectMovementTimeScale = 1f; 

    // Shouldn't need this
    public LineController LinePrefab;

    // PUBLIC ACCESSORS // 
    public float RoomLeftSide => -10 * RoomWidth;
    public float RoomRightSide => 10 * RoomWidth;
    public float RoomTopSide => 5.5f * RoomHeight;
    public float RoomBottomSide => -5.5f * RoomHeight;

    // A Dictonary of Line -> Dictionary of Vector3 -> Intersection Data
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> _intersections;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Lines = LineParent.GetComponentsInChildren<LineController>();
        // Eventually everything in this function will be in something like "On Player Enter" or something and will be called by the world manager
        if (Lines.Length == 0)
        {
            Debug.LogError("There are no lines");
            return;
        }

        //foreach (LineController line in Lines)
        //{
        //    line.ConfigureInformation();
        //}

        _intersections = CalculateIntersections();

        if(StartingLine == null)
        {
            Debug.Log("No start line, picking first line");
            StartingLine = Lines[0];
        }


        if (_player == null)
        {
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                SetPlayer(Instantiate(_playerPrefab));
            }
        }
        if (_player != null)
        {
            SetPlayer(_player);
        }

        _cameraBounds.SetPath(0, new Vector2[] {
            new Vector2(RoomLeftSide, RoomTopSide),
            new Vector2(RoomLeftSide, RoomBottomSide),
            new Vector2(RoomRightSide, RoomBottomSide),
            new Vector2(RoomRightSide, RoomTopSide)
        });

        if(RoomHeight != 1 || RoomWidth != 1)
        {
            _inUseCamera = _dynamicVirtualCamera;
        }
        else
        {
            _inUseCamera = _staticVirtualCamera;
        }

        _inUseCamera.gameObject.SetActive(true);
        _inUseCamera.Follow = _player.transform;

        // TODO: Revisit this, not sure if there's a better way to do this
        new List<LineMovementController>(FindObjectsOfType<LineMovementController>()).ForEach(controller => controller.SetLevelManager(this));
    }

    private void Update()
    {
        _intersections = CalculateIntersections();
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        _player.SetLevelManager(this);
        StartCoroutine(_player.MakeInvulnerable(_player.RoomStartInvulnerability));
        ResetPlayer();
        _player.OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void OnDisable()
    {
        Player.Instance.OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    public void ResetPlayer()
    {
        // float startingPos = StartingPoint == Enums.LinePoints.A ? 0 : 1;
        _player.SetNewLine(StartingLine, _startingDistance);
        
    }

    public void HandlePlayerDeath()
    {
        StartCoroutine(PlayerDeathCoroutine());
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        List<ExplodeOnDeath> thingsToExplode = FindObjectsOfType<ExplodeOnDeath>().OrderBy(x => x.Priority).ToList();

        foreach(ExplodeOnDeath exploder in thingsToExplode)
        {
            exploder.TriggerEffect();

            while (exploder.IsPlaying())
            {
                yield return null;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetStartingDistance(float startingDistance)
    {
        _startingDistance = startingDistance;
    }

    public List<IntersectionData> GetIntersectionPointsAroundPos(LineController currentLine, Vector3 position, float range)
    {
        List<IntersectionData> intersectionsData = new List<IntersectionData>();

        Dictionary<Vector3, List<IntersectionData>> pointToData = new Dictionary<Vector3, List<IntersectionData>>();
        if(_intersections.TryGetValue(currentLine, out pointToData))
        {
            foreach(Vector3 intersectionPoint in pointToData.Keys)
            {
                if(Vector3.Distance(position, intersectionPoint) <= range)
                {
                    foreach(IntersectionData data in pointToData[intersectionPoint])
                    {
                        intersectionsData.Add(data);
                    }
                }
            }
        }

        return intersectionsData;
    }

    public IntersectionData FindClosestIntersectionFromLine(Vector3 lineStart, Vector3 lineEnd)
    {
        List<IntersectionData> intersections = new List<IntersectionData>();

        foreach(LineController line in Lines)
        {
            if (line.Active == false) continue;

            Utilities.IntersectionPoint intersectionPoint = Utilities.FindIntersectionPoint(line.CurrentA, line.CurrentB, lineStart, lineEnd);
            Vector3 point = intersectionPoint.Point;

            if(point.x != Vector3.positiveInfinity.x)
            {
                // Use the X values to find how far along the line we are from 0 to 1
                float tValue = Mathf.InverseLerp(line.CurrentA.x, line.CurrentB.x, point.x);

                // If we got 0 using X, try Y
                if (tValue == 0)
                {
                    tValue = Mathf.InverseLerp(line.CurrentA.y, line.CurrentB.y, point.y);
                }

                intersections.Add(new IntersectionData(line, tValue, point, intersectionPoint.IsParallel));
            }
        }

        IntersectionData closestIntersection = null;
        float closestDistance = float.PositiveInfinity;

        foreach(IntersectionData intersection in intersections)
        {
            float distance = Vector3.Distance(lineStart, intersection.IntersectionWorldSpace);
            if(distance < closestDistance)
            {
                closestIntersection = intersection;
                closestDistance = distance;
            }
        }

        return closestIntersection;
    }

    /** PRIVATE METHODS**/
    // Would be smart to split this into one dictionary that has only static lines, and one that deals with only moving line stuff
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> CalculateIntersections()
    {
        Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> intersections = new Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>>();

        // Loop through each line
        foreach (LineController currentLine in Lines)
        {
            // We don't actually need intersection data if nothing is on the line
            if (currentLine.HasObjectsOn == false) continue;

            Dictionary<Vector3, List<IntersectionData>> innerDictionary = new Dictionary<Vector3, List<IntersectionData>>();

            // Loop through each line
            foreach (LineController otherLine in Lines)
            {
                // Make sure we're not comparing a line to itself
                if (currentLine != otherLine && currentLine.Active && otherLine.Active)
                {
                    Utilities.IntersectionPoint intersectionPoint = Utilities.FindIntersectionPoint(currentLine.CurrentA, currentLine.CurrentB, otherLine.CurrentA, otherLine.CurrentB);
                    Vector3 point = intersectionPoint.Point;

                    // Make sure we actually have an intersection point (using the x component is enough since we can't actually compare Vector3.infinity with itself)
                    if (point.x != Vector3.positiveInfinity.x)
                    {
                        // Need to do some tolerance stuff to make things close to 0 act as 0 (This might need to be extended to other numbers)
                        float aX = otherLine.CurrentA.x;
                        float bX = otherLine.CurrentB.x;
                        float aY = otherLine.CurrentA.y;
                        float bY = otherLine.CurrentB.y;
                        float zeroTolerance = 0.000005f;

                        if (aX > -zeroTolerance && aX < zeroTolerance) aX = 0f;
                        if (bX > -zeroTolerance && bX < zeroTolerance) bX = 0f;
                        if (aY > -zeroTolerance && aY < zeroTolerance) aY = 0f;
                        if (bY > -zeroTolerance && bY < zeroTolerance) bY = 0f;
                        // Use the X values to find how far along the line we are from 0 to 1
                        float tValue = Mathf.InverseLerp(aX, bX, point.x);

                        // If we got 0 using X, try Y
                        if (tValue == 0)
                        {
                            tValue = Mathf.InverseLerp(aY, bY, point.y);
                        }

                        // Create intersection data and add it to the dictionary
                        IntersectionData data = new IntersectionData(otherLine, tValue, point, intersectionPoint.IsParallel);

                        if (innerDictionary.ContainsKey(point))
                        {
                            innerDictionary[point].Add(data);
                        }
                        else
                        {
                            innerDictionary.Add(point, new List<IntersectionData>() { data });
                        }

                        //Debug.Log("Added intersection between: " + currentLine.name + " and " + otherLine.name);
                        //Debug.Log("Intersection point is: " + intersectionPoint);
                        //Debug.Log("Distance along the second line is: " + tValue);
                    }
                }
            }

            intersections.Add(currentLine, innerDictionary);
        }

        return intersections;
    }

    public List<IntersectionData> GetAllIntersectionData()
    {
        List<IntersectionData> intersections = new List<IntersectionData>();

        foreach(Dictionary<Vector3, List<IntersectionData>> lineInfo in _intersections.Values)
        {
            foreach(List<IntersectionData> intersectionDatas in lineInfo.Values)
            {
                intersections.AddRange(intersectionDatas);
            }
        }

        return intersections;
    }

    public RoomData CreateRoomData()
    {
        return new RoomData(LineParent.GetComponentsInChildren<LineController>(), this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 topLeft = new Vector3(RoomLeftSide, RoomTopSide);
        Vector3 bottomLeft = new Vector3(RoomLeftSide, RoomBottomSide);
        Vector3 bottomRight = new Vector3(RoomRightSide, RoomBottomSide);
        Vector3 topRight = new Vector3(RoomRightSide, RoomTopSide);

        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;

            foreach (Dictionary<Vector3, List<IntersectionData>> intersectionData in _intersections.Values)
            {
                foreach (Vector3 point in intersectionData.Keys)
                {
                    Gizmos.DrawSphere(point, 0.3f);
                }
            }
        }
    }
}
