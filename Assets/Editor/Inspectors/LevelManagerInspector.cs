using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("AddLine"))
        {
            LineController newLine = GameObject.Instantiate<LineController>(levelManager.LinePrefab);

            levelManager.AddNewLine(newLine);
        }
    }
}
