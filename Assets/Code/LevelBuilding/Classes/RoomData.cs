using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public LineData[] Lines;
    public float width;
    public float height;

    public RoomData(LineController[] lineControllers)
    {
        Lines = new LineData[lineControllers.Length];

        for(int i = 0; i < Lines.Length; i++)
        {
            LineController line = lineControllers[i];
            LineData newLine = new LineData(line.InitialA, line.InitialB);

            Lines[i] = newLine;
        }

        width = 20;
        height = 11;
    }

    [Newtonsoft.Json.JsonConstructor]
    public RoomData(LineData[] Lines)
    {
        this.Lines = Lines;

        width = 20;
        height = 11;
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
