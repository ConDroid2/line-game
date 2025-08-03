using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DangerZoneController))]
public class DangerZoneEditor : Editor
{

    private void OnSceneGUI()
    {
        DangerZoneController zone = (DangerZoneController)target;
        float handleSize = HandleUtility.GetHandleSize(zone.transform.position) * 0.2f;

        Vector3 handlePos = new Vector3(zone.BoxTrigger.bounds.center.x + zone.BoxTrigger.bounds.extents.x, zone.BoxTrigger.bounds.center.y + zone.BoxTrigger.bounds.extents.y, 0f);


        EditorGUI.BeginChangeCheck();

        Vector3 newExtentsPoint = Handles.FreeMoveHandle(handlePos, Quaternion.identity, handleSize, Vector3.zero, Handles.SphereHandleCap).Round(0.5f);

        float newXExtents = newExtentsPoint.x - zone.BoxTrigger.bounds.center.x;
        float newYExtents = newExtentsPoint.y - zone.BoxTrigger.bounds.center.y;

        float newXSize = Mathf.Round(newXExtents * 2);
        float newYSize = Mathf.Round(newYExtents * 2);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(zone.BoxTrigger, "Change Size");

            zone.BoxTrigger.size = new Vector3(
                newXSize > 0 ? newXSize : zone.BoxTrigger.size.x,
                newYSize > 0 ? newYSize : zone.BoxTrigger.size.y,
                0f);
        }
    }


    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmo(DangerZoneController zone, GizmoType gizmoType)
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.red;
            float handleSize = HandleUtility.GetHandleSize(zone.transform.position) * 0.1f;
            Gizmos.DrawSphere(zone.transform.position, handleSize);

            Vector3 bounds = zone.BoxTrigger.bounds.extents;
            Vector3 center = zone.BoxTrigger.bounds.center;

            Vector3 topLeft = new Vector3(center.x -bounds.x, center.y + bounds.y, 0f);
            Vector3 topRight = new Vector3(center.x + bounds.x, center.y + bounds.y, 0f);
            Vector3 bottomRight = new Vector3(center.x + bounds.x, center.y - bounds.y, 0f);
            Vector3 bottomLeft = new Vector3(center.x - bounds.x, center.y - bounds.y, 0f);

            Vector3[] vertices = new Vector3[]
            {
                topLeft,
                topRight,
                bottomRight,
                bottomLeft
            };

            Handles.DrawSolidRectangleWithOutline(vertices, Color.red, Color.white);
        }
    }
}
