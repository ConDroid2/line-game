using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    LevelManager _levelManager;
    Player _player;
    LineController _selectedLine;


    /** WINDOW FUNCTIONS **/

    [MenuItem("Custom/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditor>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Utilities", EditorStyles.boldLabel);
        if (GUILayout.Button("Create New Level"))
        {
            SpawnLevelPrefab();
        }

        if(GUILayout.Button("Load Existing Level"))
        {
            LoadExistingLevel();
        }

        GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Level"))
            {
                SaveNewLevel();
            }

            if (GUILayout.Button("Close Level"))
            {
                CloseLevel();
            }
        GUILayout.EndHorizontal();

        GUILayout.Label("Level Building", EditorStyles.boldLabel);
        if(GUILayout.Button("Add Line"))
        {
            SpawnLine();
        }
        if(GUILayout.Button("Add Danger Zone"))
        {
            SpawnDangerZone();
        }

        

        // Menu for if we have a selected line
        if(_selectedLine != null)
        {
            GUILayout.Label("Selected Line", EditorStyles.boldLabel);
            if(GUILayout.Button("Set as Start"))
            {
                _levelManager.StartingLine = _selectedLine;
            }
            if(GUILayout.Button("Add Object to Line"))
            {
                AddObjectToLine();
            }
        }
    }

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
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

    private void FindAndSetUpLevel(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _player = FindObjectOfType<Player>();

            // _levelManager.SetPlayer(_player);
        }
    }

    private void SaveNewLevel()
    {
        if (CheckForLevelPrefab())
        {
            GetWindow<SaveLevelWindow>();

            
        }
    }

    private void LoadExistingLevel()
    {
        string levelPath = EditorUtility.OpenFilePanel("Select a level", "Assets/Levels", "prefab");

        // Trim the path up untill "Asset"
        int beginningOfAssetPath = levelPath.IndexOf("Asset");
        string updatedAssetPath = levelPath.Substring(beginningOfAssetPath);

        // Grab asset, instantiate, remove clone from name
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(updatedAssetPath);
        _levelManager = Instantiate(levelPrefab).GetComponentInChildren<LevelManager>();

        _levelManager.transform.parent.name = _levelManager.transform.parent.name.Replace("(Clone)", "");
    }


    private void SpawnLevelPrefab()
    {
        if (CheckForPlayer())
        {
            GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level Prefab.prefab");

            _levelManager = Instantiate(levelPrefab).GetComponentInChildren<LevelManager>();
            _levelManager.transform.parent.name = _levelManager.transform.parent.name.Replace("(Clone)", "");
            _levelManager.SetPlayer(_player);
        }
    }

    private void CloseLevel()
    {
        if(EditorUtility.DisplayDialog("Save Check", "About to close level, any unsaved changes will be lost.", "Ok", "Cancel"))
        {
            DestroyImmediate(_levelManager.transform.parent.gameObject);
        }
    }

    /** ADD TO LEVEL**/
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

    private bool CheckForLevelPrefab()
    {
        if (_levelManager == null)
        {
            _levelManager = FindObjectOfType<LevelManager>();

            if(_levelManager == null)
            {
                EditorUtility.DisplayDialog("Looking For Level", "No Level found.\nThe action you're trying perform needs a Level", "Ok", "Cancel");
                return false;
            }

            return true;
        }
        else
        {
            return true;
        }
    }

    private bool CheckForPlayer()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                EditorUtility.DisplayDialog("Looking For Player", "No Player found.\nThe action you're trying perform needs a Player", "Ok", "Cancel");
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
