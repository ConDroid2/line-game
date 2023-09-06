using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<LineController> _lines = new List<LineController>();

    public LineController LinePrefab;

    // A Dictonary of Line -> Dictionary of Vector3 -> Intersection Data
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> _intersections;

    private void Awake()
    {
        if(_lines.Count == 0)
        {
            Debug.LogError("There are no lines");
            return;
        }

        Debug.Log(_lines.Count);

        foreach (LineController line in _lines)
        {
            line.ConfigureInformation();
        }

        _intersections = CalculateIntersections();

        _player.LevelManager = this;
        _player.SetNewLine(_lines[0], 0f);
    }

    private void Update()
    {
        _intersections = CalculateIntersections();
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

    public void AddNewLine(LineController newLine)
    {
        _lines.Add(newLine);
    }

    /** PRIVATE METHODS**/
    // Would be smart to split this into one dictionary that has only static lines, and one that deals with only moving line stuff
    private Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> CalculateIntersections()
    {
        Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>> intersections = new Dictionary<LineController, Dictionary<Vector3, List<IntersectionData>>>();

        // Loop through each line
        foreach (LineController currentLine in _lines)
        {
            Dictionary<Vector3, List<IntersectionData>> innerDictionary = new Dictionary<Vector3, List<IntersectionData>>();

            // Loop through each line
            foreach (LineController otherLine in _lines)
            {
                // Make sure we're not comparing a line to itself
                if (currentLine != otherLine)
                {
                    Vector3 intersectionPoint = Utilities.FindIntersectionPoint(currentLine.A, currentLine.B, otherLine.A, otherLine.B);

                    // Make sure we actually have an intersection point (using the x component is enough since we can't actually compare Vector3.infinity with itself)
                    if (intersectionPoint.x != Vector3.positiveInfinity.x)
                    {
                        // Use the X values to find how far along the line we are from 0 to 1
                        float tValue = Mathf.InverseLerp(otherLine.A.x, otherLine.B.x, intersectionPoint.x);

                        // If we got 0 using X, try Y
                        if (tValue == 0)
                        {
                            tValue = Mathf.InverseLerp(otherLine.A.y, otherLine.B.y, intersectionPoint.y);
                        }

                        // Create intersection data and add it to the dictionary
                        IntersectionData data = new IntersectionData(otherLine, tValue);

                        if (innerDictionary.ContainsKey(intersectionPoint))
                        {
                            innerDictionary[intersectionPoint].Add(data);
                        }
                        else
                        {
                            innerDictionary.Add(intersectionPoint, new List<IntersectionData>() { data });
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

    /** VISUAL DEBUGGING **/
    private void OnDrawGizmosSelected()
    {
        HashSet<Vector3> intersectionPoints = new HashSet<Vector3>();

        foreach(LineController line in _lines)
        {
            foreach(LineController otherLine in _lines)
            {
                intersectionPoints.Add(Utilities.FindIntersectionPoint(line.A, line.B, otherLine.A, otherLine.B));
            }
        }

        foreach(Vector3 intersection in intersectionPoints)
        {
            Gizmos.DrawSphere(intersection, 0.1f);
        }
    }
}
