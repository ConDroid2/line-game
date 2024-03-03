using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomPreview))]
public class RoomPreviewEditor : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmo(RoomPreview preview, GizmoType gizmoType)
    {
        if (Application.isEditor)
        {
            Gizmos.color = Color.white;
            Vector3 pos = preview.transform.position;
            float handleSize = HandleUtility.GetHandleSize(pos) * 0.1f;
            Gizmos.DrawSphere(pos, handleSize);
        }
    }
}
