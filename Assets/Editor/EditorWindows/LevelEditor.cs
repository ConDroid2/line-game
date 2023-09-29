using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    LevelManager _levelManager;
    LineController _selectedLine;


    /** WINDOW FUNCTIONS **/

    [MenuItem("Custom/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }

    private void OnGUI()
    {
        // If there is no level manager
        if (_levelManager == null)
        {
            if(GUILayout.Button("Find Current Level"))
            {
                _levelManager = FindObjectOfType<LevelManager>();
            }
            if (GUILayout.Button("Create New Level"))
            {
                SpawnLevelPrefab();
            }
        }
        else
        {
            GUILayout.Label(_levelManager.transform.parent.name, EditorStyles.boldLabel);

            if(GUILayout.Button("Add Line"))
            {
                SpawnLine();
            }
            if(GUILayout.Button("Add Danger Zone"))
            {
                SpawnDangerZone();
            }

            if(_selectedLine != null)
            {
                GUILayout.Label("Selected Line", EditorStyles.boldLabel);
                if(GUILayout.Button("Set as Start"))
                {
                    _levelManager.StartingLine = _selectedLine;
                }
                if(GUILayout.Button("Delete Line"))
                {
                    _levelManager.Lines.Remove(_selectedLine);
                    DestroyImmediate(_selectedLine.gameObject);
                }
            }
        }
    }

    private void Awake()
    {
        Debug.Log("Editor window awakes");
    }

    private void OnSelectionChange()
    {   
        if(Selection.activeGameObject != null)
        {
            _selectedLine = Selection.activeGameObject.GetComponent<LineController>();
            Repaint();
        }
        else
        {
            _selectedLine = null;
            Repaint();
        }
    }

    /** UTILITY METHODS **/

    private void SpawnLevelPrefab()
    {
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level Prefab.prefab");

        _levelManager = Instantiate(levelPrefab).GetComponentInChildren<LevelManager>();
    }

    private void SpawnLine()
    {
        LineController linePrefab = AssetDatabase.LoadAssetAtPath<LineController>("Assets/Prefabs/LevelComponents/Line.prefab");

        LineController newLine = Instantiate(linePrefab, _levelManager.LineParent.transform);
        _levelManager.AddNewLine(newLine);
    }

    private void SpawnDangerZone()
    {
        DangerZone dangerZonePrefab = AssetDatabase.LoadAssetAtPath<DangerZone>("Assets/Prefabs/LevelComponents/DangerZone.prefab");

        DangerZone newZone = Instantiate(dangerZonePrefab, _levelManager.DangerZoneParent.transform);
    }
}
