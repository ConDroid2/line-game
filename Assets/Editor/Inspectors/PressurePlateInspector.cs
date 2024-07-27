using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PressurePlateScript))]
public class PressurePlateInspector : Editor
{
    public override void OnInspectorGUI()
    {
        PressurePlateScript script = (PressurePlateScript)target;        

        base.OnInspectorGUI(); ;

        if (GUI.changed)
        {
            script.SetCorrectSpriteInEditor();
        }
    }
}
