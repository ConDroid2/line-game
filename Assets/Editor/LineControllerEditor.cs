using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LineController))]
public class LineControllerEditor : Editor
{

    private void OnSceneGUI()
    {
        LineController controller = (LineController)target;
        float handleSize = HandleUtility.GetHandleSize(controller.transform.position) * 0.2f;
        float gridSize = 0.5f;

        EditorGUI.BeginChangeCheck();

        Vector3 newAPos = Handles.FreeMoveHandle(controller.A, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);
        Handles.Label(controller.A + Vector3.down / 3, "A");

        Vector3 newBPos = Handles.FreeMoveHandle(controller.B, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);
        Handles.Label(controller.B + Vector3.down / 3, "B");

        if (EditorGUI.EndChangeCheck())
        {
            if (controller.A != newAPos || controller.B != newBPos)
            {
                if (Event.current.shift)
                {
                    if(controller.A != newAPos)
                    {
                        Vector3 changeVector = newAPos - controller.A;
                        newBPos = controller.B + changeVector;
                    }
                    else if(controller.B != newBPos)
                    {
                        Vector3 changeVector = newBPos - controller.B;
                        newAPos = controller.A + changeVector;
                    }
                }

                Undo.RecordObject(controller, "Change endpoints");
                controller.A = newAPos;
                controller.B = newBPos;
            }
        }

        if (controller.LineShifters.Length > 0)
        {
            Vector3 moveTarget = controller.Midpoint + controller.LineShifters[0].MovementVector;

            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();

            Vector3 newTarget = Handles.FreeMoveHandle(moveTarget, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 newMoveVector = newTarget - controller.Midpoint;

                Undo.RecordObject(controller, "Change shifter");
                controller.LineShifters[0].MovementVector = newMoveVector;
                
            }
        }
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmo(LineController line, GizmoType gizmoType)
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.white;
            Vector3 pos = line.CalculateMidpoint();
            float handleSize = HandleUtility.GetHandleSize(pos) * 0.1f;
            Gizmos.DrawSphere(line.CalculateMidpoint(), handleSize);
        }
    }
}
