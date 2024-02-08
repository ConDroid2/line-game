using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OnLineController))]
public class OnLineControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        OnLineController objectOnLine = (OnLineController)target;

        base.OnInspectorGUI();

        float newDistance = EditorGUILayout.Slider("Distance On Line", objectOnLine.DistanceOnLine, 0f, 1f);

        if (newDistance != objectOnLine.DistanceOnLine)
        {
            objectOnLine.SetDistanceOnLineInEditor(newDistance);

            EditorUtility.SetDirty(objectOnLine);
        }

        
    }
}
