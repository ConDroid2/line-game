using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FreeObjectShifter))]
public class FreeObjectShifterEditor : Editor
{
    private void OnSceneGUI()
    {
        FreeObjectShifter shifter = (FreeObjectShifter)target;

        float handleSize = HandleUtility.GetHandleSize(shifter.transform.position) * 0.2f;
        float gridSize = 0.5f;

        Handles.color = Color.green;

        EditorGUI.BeginChangeCheck();

        Vector3 newTarget = Handles.FreeMoveHandle(shifter.transform.position + shifter.MovementVector, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);

        if (EditorGUI.EndChangeCheck())
        {
            Vector3 newMoveVector = newTarget - shifter.transform.position;

            Undo.RecordObject(shifter, "Change free object shifter");
            shifter.MovementVector = newMoveVector;

        }

        Handles.DrawLine(shifter.transform.position, shifter.transform.position + shifter.MovementVector);
    }
}
