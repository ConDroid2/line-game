using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectOnLine))]
public class ObjectOnLineInspeector : Editor
{
    float newDistance = 0f;
    public override void OnInspectorGUI()
    {
        ObjectOnLine objectOnLine = (ObjectOnLine)target;

        base.OnInspectorGUI();

        if (objectOnLine.MovementController)
        {
            newDistance = EditorGUILayout.Slider("Distance Along Line", objectOnLine.MovementController.DistanceAlongLine, 0, 1);

            if (newDistance != objectOnLine.MovementController.DistanceAlongLine)
            {
                objectOnLine.MovementController.SetDistanceAlongLine(newDistance);
            }
        }
    }
}
