using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectOnLine))]
public class ObjectOnLineInspeector : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectOnLine objectOnLine = (ObjectOnLine)target;

        base.OnInspectorGUI();

        float newDistance = EditorGUILayout.Slider("Distance On Line", objectOnLine.DistanceOnLine, 0f, 1f);

        if (newDistance != objectOnLine.DistanceOnLine)
        {
            objectOnLine.DistanceOnLine = newDistance;
        }
    }
}
