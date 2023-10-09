using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private void OnSceneGUI()
    {
        //Player controller = (Player)target;

        //Vector3 pos = controller.transform.position;
        //float range = controller.CheckForIntersectionsDistance;

        //Handles.color = Color.yellow;

        //EditorGUI.BeginChangeCheck();

        //float newRange = Handles.ScaleValueHandle(range, pos + Vector3.right * range, controller.transform.rotation, 5, Handles.ArrowHandleCap, 5);
        //Handles.DrawWireDisc(controller.transform.position, Vector3.forward, controller.CheckForIntersectionsDistance);

        //if (EditorGUI.EndChangeCheck())
        //{
        //    Undo.RecordObject(controller, "Change Range Value");
        //    controller.CheckForIntersectionsDistance = newRange;
        //}
        
    }
}
