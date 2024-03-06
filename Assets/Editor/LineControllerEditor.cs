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
                bool movePoints = false;

                if (Event.current.shift)
                {
                    if (controller.InitialA != newAPos)
                    {
                        Vector3 changeVector = newAPos - controller.InitialA;
                        newBPos = controller.InitialB + changeVector;
                    }
                    else if (controller.InitialB != newBPos)
                    {
                        Vector3 changeVector = newBPos - controller.InitialB;
                        newAPos = controller.InitialA + changeVector;
                    }

                    movePoints = true;
                }

                //else if (newAPos.x < newBPos.x || (newAPos.x - newBPos.x == 0 && newAPos.y < newBPos.y))
                //{
                //    movePoints = true;
                    
                //}

                // if (movePoints)
                {
                    Undo.RecordObject(controller, "Change endpoints");
                    controller.InitialA = newAPos;
                    controller.InitialB = newBPos;
                    //controller.TestA = new SerializeableVector3(newAPos);
                    //controller.TestB = new SerializeableVector3(newBPos);
                    controller.transform.position = controller.InitialMidpoint;
                    EditorUtility.SetDirty(controller);
                }
            }
        }
    }

    [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
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
