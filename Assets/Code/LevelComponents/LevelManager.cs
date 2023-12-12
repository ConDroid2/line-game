using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Public References")]
    public GameObject LineParent;
    public GameObject DangerZoneParent;
    public GameObject MiscLevelComponentsParent;

    [Header("Likely to be made private")]
    [SerializeField] Player _playerPrefab;
    private Player _player;
    public LineController[] Lines;

    

    [HideInInspector] public LineController StartingLine;
    [SerializeField] private Enums.LinePoints _startingPoint = Enums.LinePoints.A;

    // Shouldn't need this
    public LineController LinePrefab;

    // A Dictonary of Line -> Dictionary of Vector3 -> Intersection Data
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> _intersections;

    private void Start()
    {
        Lines = LineParent.GetComponentsInChildren<LineController>();
        // Eventually everything in this function will be in something like "On Player Enter" or something and will be called by the world manager
        if (Lines.Length == 0)
        {
            Debug.LogError("There are no lines");
            return;
        }

        foreach (LineController line in Lines)
        {
            line.ConfigureInformation();
        }

        _intersections = CalculateIntersections();

        if(StartingLine == null)
        {
            StartingLine = Lines[0];
        }


        if(_player == null)
        {
            _player = FindObjectOfType<Player>();

            if(_player == null)
            {
                SetPlayer(Instantiate(_playerPrefab));
            }
        }
        if(_player != null)
        {
            SetPlayer(_player);
        }
    }

    private void Update()
    {
        _intersections = CalculateIntersections();
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        _player.SetLevelManager(this);
        ResetPlayer();
        _player.OnPlayerDeath += ResetPlayer;
    }

    public void ResetPlayer()
    {
        float startingPos = _startingPoint == Enums.LinePoints.A ? 0 : 1;
        _player.SetNewLine(StartingLine, startingPos);
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

    /** PRIVATE METHODS**/
    // Would be smart to split this into one dictionary that has only static lines, and one that deals with only moving line stuff
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> CalculateIntersections()
    {
        Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> intersections = new Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>>();

        // Loop through each line
        foreach (LineController currentLine in Lines)
        {
            Dictionary<Vector3, List<IntersectionData>> innerDictionary = new Dictionary<Vector3, List<IntersectionData>>();

            // Loop through each line
            foreach (LineController otherLine in Lines)
            {
                // Make sure we're not comparing a line to itself
                if (currentLine != otherLine)
                {
                    Utilities.IntersectionPoint intersectionPoint = Utilities.FindIntersectionPoint(currentLine.CurrentA, currentLine.CurrentB, otherLine.CurrentA, otherLine.CurrentB);
                    Vector3 point = intersectionPoint.Point;

                    // Make sure we actually have an intersection point (using the x component is enough since we can't actually compare Vector3.infinity with itself)
                    if (point.x != Vector3.positiveInfinity.x)
                    {
                        // Use the X values to find how far along the line we are from 0 to 1
                        float tValue = Mathf.InverseLerp(otherLine.CurrentA.x, otherLine.CurrentB.x, point.x);

                        // If we got 0 using X, try Y
                        if (tValue == 0)
                        {
                            tValue = Mathf.InverseLerp(otherLine.CurrentA.y, otherLine.CurrentB.y, point.y);
                        }

                        // Create intersection data and add it to the dictionary
                        IntersectionData data = new IntersectionData(otherLine, tValue, intersectionPoint.IsParallel);

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

    public RoomData CreateRoomData()
    {
        return new RoomData(LineParent.GetComponentsInChildren<LineController>());
    }
}
