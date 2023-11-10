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

        Vector3 newAPos = Handles.FreeMoveHandle(controller.InitialA, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);
        Handles.Label(controller.InitialA + Vector3.down / 3, "A");

        Vector3 newBPos = Handles.FreeMoveHandle(controller.InitialB, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);
        Handles.Label(controller.InitialB + Vector3.down / 3, "B");

        if (EditorGUI.EndChangeCheck())
        {
            if (controller.InitialA != newAPos || controller.InitialB != newBPos)
            {
                if (Event.current.shift)
                {
                    if(controller.InitialA != newAPos)
                    {
                        Vector3 changeVector = newAPos - controller.InitialA;
                        newBPos = controller.InitialB + changeVector;
                    }
                    else if(controller.InitialB != newBPos)
                    {
                        Vector3 changeVector = newBPos - controller.InitialB;
                        newAPos = controller.InitialA + changeVector;
                    }
                }

                Undo.RecordObject(controller, "Change endpoints");
                controller.InitialA = newAPos;
                controller.InitialB = newBPos;
                controller.transform.position = controller.InitialMidpoint;
            }
        }

        //if (controller.LineShifters.Length > 0)
        //{
        //    Vector3 moveTarget = controller.Midpoint + controller.LineShifters[0].MovementVector;

        //    Handles.color = Color.green;

        //    EditorGUI.BeginChangeCheck();

        //    Vector3 newTarget = Handles.FreeMoveHandle(moveTarget, Quaternion.identity, handleSize, Vector3.one, Handles.SphereHandleCap).Round(gridSize);

        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        Vector3 newMoveVector = newTarget - controller.Midpoint;

        //        Undo.RecordObject(controller, "Change shifter");
        //        controller.LineShifters[0].MovementVector = newMoveVector;
                
        //    }
        //}
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmo(LineController line, GizmoType gizmoType)
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.white;
            Vector3 pos = line.InitialMidpoint;
            float handleSize = HandleUtility.GetHandleSize(pos) * 0.1f;
            Gizmos.DrawSphere(line.InitialMidpoint, handleSize);
        }
    }
}
