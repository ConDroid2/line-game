using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActivationIndicatorLine))]
public class ActivationIndicatorLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ActivationIndicatorLine line = (ActivationIndicatorLine)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Add Point"))
        {
            Debug.Log("Button Pressed");
            line.LineRenderer.positionCount += 1;
        }

        if(GUILayout.Button("Remove Last Point"))
        {
            if(line.LineRenderer.positionCount > 0)
            {
                line.LineRenderer.positionCount -= 1;
            }
        }
    }

    private void OnSceneGUI()
    {
        ActivationIndicatorLine line = (ActivationIndicatorLine)target;


        float handleSize = HandleUtility.GetHandleSize(line.transform.position) * 0.2f;
        float gridSize = 0.5f;

        Vector3[] newPointPositions = new Vector3[line.LineRenderer.positionCount];
        line.LineRenderer.GetPositions(newPointPositions);

        EditorGUI.BeginChangeCheck();

        for(int i = 0; i < line.LineRenderer.positionCount; i++)
        {
            newPointPositions[i] = Handles.FreeMoveHandle(newPointPositions[i], Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);
        }

        if (EditorGUI.EndChangeCheck())
        {
            bool pointChanged = false;

            for(int i = 0; i < line.LineRenderer.positionCount; i++)
            {
                if(newPointPositions[i] != line.LineRenderer.GetPosition(i))
                {
                    pointChanged = true;
                }
            }

            if (pointChanged)
            {
                Undo.RecordObject(line, "Points Changed");

                line.LineRenderer.SetPositions(newPointPositions);
                EditorUtility.SetDirty(line);
            }
        }
    }
}
