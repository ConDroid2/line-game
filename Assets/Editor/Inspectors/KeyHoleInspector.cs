using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyHole))]
public class KeyHoleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Unlock"))
        {
            if (Application.isPlaying)
            {
                (target as KeyHole).UnlockKeyHole();
            }
        }
    }
}
