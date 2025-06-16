using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointOfInterestVisualsController))]
public class POIVisualsControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Re-Position"))
        {
            (target as PointOfInterestVisualsController).CalculateAndSetPositions();
        }
    }
}
