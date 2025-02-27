using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public LineData[] Lines;
    public RoomPort[] Ports;
    public float Left;
    public float Top;
    public float Right;
    public float Bottom;
    public PointOfInterestData[] PointsOfInterest;

    [Newtonsoft.Json.JsonConstructor]
    public RoomData(LineData[] Lines, RoomPort[] Ports, float Left, float Top, float Right, float Bottom, PointOfInterestData[] PointsOfInterest)
    {
        Debug.Log("Calling RoomData Constructor");
        this.Lines = Lines;
        this.Ports = Ports;

        this.Left = Left;
        this.Top = Top;
        this.Right = Right;
        this.Bottom = Bottom;

        this.PointsOfInterest = PointsOfInterest;
    }

    public RoomData(LineController[] lineControllers, LevelManager levelManager)
    {
        Lines = new LineData[lineControllers.Length];

        for(int i = 0; i < Lines.Length; i++)
        {
            LineController line = lineControllers[i];
            LineData newLine = new LineData(line.InitialA, line.InitialB, line.transform.GetSiblingIndex());

            Lines[i] = newLine;
        }

        Left = levelManager.RoomLeftSide;
        Top = levelManager.RoomTopSide;
        Right = levelManager.RoomRightSide;
        Bottom = levelManager.RoomBottomSide;

        Ports = CalculatePorts(levelManager);
    }

    
    public RoomData(RoomData roomData)
    {
        Lines = roomData.Lines;
        Ports = roomData.Ports;

        Left = roomData.Left;
        Top = roomData.Top;
        Right = roomData.Right;
        Bottom = roomData.Bottom;

        PointsOfInterest = roomData.PointsOfInterest;
    }

    private RoomPort[] CalculatePorts(LevelManager levelManager)
    {
        List<RoomPort> portsList = new List<RoomPort>();

        // Check each line to see if it has a port (it is possible one line has multiple ports)
        foreach(LineData line in Lines)
        {
            // Note: We don't need to check each line point for each room side since A will always be to the Left and Bottom of B
            if(line.A.x <= Left)
            {
                RoomPort port = new RoomPort(line.A, line.SiblingIndex, Enums.LinePoints.A, Enums.RoomSides.Left);
                portsList.Add(port);
            }
            if(line.B.x >= Right)
            {
                RoomPort port = new RoomPort(line.B, line.SiblingIndex, Enums.LinePoints.B, Enums.RoomSides.Right);
                portsList.Add(port);
            }
            if(line.A.y <= Bottom)
            {
                RoomPort port = new RoomPort(line.A, line.SiblingIndex, Enums.LinePoints.A, Enums.RoomSides.Bottom);
                portsList.Add(port);
            }
            if(line.B.y >= Top)
            {
                RoomPort port = new RoomPort(line.B, line.SiblingIndex, Enums.LinePoints.B, Enums.RoomSides.Top);
                portsList.Add(port);
            }
        }

        return portsList.ToArray();
    }

    public override string ToString()
    {
        string lineData = "";

        for(int i = 0; i < Lines.Length; i++)
        {
            LineData line = Lines[i];
            lineData += $"Line {i + 1}: A-{line.A} to B-{line.B}";
        }

        return lineData;
    }
}
