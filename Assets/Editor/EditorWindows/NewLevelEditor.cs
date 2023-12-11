using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class NewLevelEditor : EditorWindow
{

    private LevelManager _levelManager;
    private LineController _selectedLine;
    
    // EditorWindow CLASS FUNCTIONS
    [MenuItem("Custom/New Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<NewLevelEditor>();
    }

    private void OnGUI()
    {

        GUILayout.Label("Level Building", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Line"))
        {
            SpawnLine();
        }
        if (GUILayout.Button("Add Danger Zone"))
        {
            SpawnDangerZone();
        }

        // Menu for if we have a selected line
        if (_selectedLine != null)
        {
            GUILayout.Label("Selected Line", EditorStyles.boldLabel);
            if (GUILayout.Button("Set as Start"))
            {
                _levelManager.StartingLine = _selectedLine;
            }
            if (GUILayout.Button("Add Object to Line"))
            {
                AddObjectToLine();
            }
        }

        GUILayout.Label("Utilities", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Up New Room"))
        {
            if (CheckForLevelPrefab(false) == false)
            {
                SetUpNewRoom();
            }
        }
        if(GUILayout.Button("Save Room"))
        {
            SaveRoom();
        }

    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
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

    // BUILD LEVEL UTILITIES
    private void SetUpNewRoom()
    {
        Camera oldCamera = FindObjectOfType<Camera>();
        if(oldCamera != null)
        {
            DestroyImmediate(oldCamera.gameObject);
        }


        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BaseLevelObjects/Level Prefab.prefab");
        GameObject cameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/BaseLevelObjects/Main Camera.prefab");

        GameObject levelRoot = (GameObject)PrefabUtility.InstantiatePrefab(levelPrefab);
        _levelManager = levelRoot.GetComponentInChildren<LevelManager>();
        GameObject camera = (GameObject)PrefabUtility.InstantiatePrefab(cameraPrefab);

        camera.name = camera.name.Replace("(Clone)", "");
        _levelManager.transform.parent.name = _levelManager.transform.parent.name.Replace("(Clone)", "");
    }

    private void SpawnLine()
    {
        if (CheckForLevelPrefab())
        {
            LineController linePrefab = AssetDatabase.LoadAssetAtPath<LineController>("Assets/Prefabs/LevelComponents/Line.prefab");

            PrefabUtility.InstantiatePrefab(linePrefab, _levelManager.LineParent.transform);
        }
    }

    private void SpawnDangerZone()
    {
        if (CheckForLevelPrefab())
        {
            DangerZone dangerZonePrefab = AssetDatabase.LoadAssetAtPath<DangerZone>("Assets/Prefabs/LevelComponents/DangerZone.prefab");

            PrefabUtility.InstantiatePrefab(dangerZonePrefab, _levelManager.DangerZoneParent.transform);
        }
    }

    private void AddObjectToLine()
    {
        string fullAssetPath = EditorUtility.OpenFilePanel("Select type of object", "Assets/Prefabs/LevelComponents/OnLineComponents", "prefab");

        // Trim the path up untill "Asset"
        int beginningOfAssetPath = fullAssetPath.IndexOf("Asset");
        string updatedAssetPath = fullAssetPath.Substring(beginningOfAssetPath);

        OnLineController prefab = AssetDatabase.LoadAssetAtPath<OnLineController>(updatedAssetPath);

        OnLineController newObject = (OnLineController)PrefabUtility.InstantiatePrefab(prefab, _levelManager.MiscLevelComponentsParent.transform);
        newObject.SetLine(_selectedLine, 0f);
    }




    //** UTILITIES **//

    private void SaveRoom()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    private bool CheckForLevelPrefab(bool displayMessage = true)
    {
        if (_levelManager == null)
        {
            _levelManager = FindObjectOfType<LevelManager>();

            if (_levelManager == null)
            {
                if (displayMessage)
                {
                    EditorUtility.DisplayDialog("Looking For Level", "No Level found.\nThe action you're trying perform needs a Level", "Ok", "Cancel");
                }
                return false;
            }

            return true;
        }
        else
        {
            return true;
        }
    }
}
