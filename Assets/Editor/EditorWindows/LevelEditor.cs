using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    LevelManager _levelManager;
    Player _player;
    LineController _selectedLine;

    bool _isNewLevel;
    string _originalName;


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
            if(GUILayout.Button("Load Existing Level"))
            {
                LoadExistingLevel();
            }
        }
        else
        {
            GUILayout.BeginHorizontal();
                GUILayout.Label("Level Name", EditorStyles.boldLabel);
                _levelManager.transform.parent.name = GUILayout.TextField(_levelManager.transform.parent.name, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if(GUILayout.Button("Add Line"))
            {
                SpawnLine();
            }
            if(GUILayout.Button("Add Danger Zone"))
            {
                SpawnDangerZone();
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

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= FindAndSetUpLevel;
        EditorApplication.playModeStateChanged += FindAndSetUpLevel;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= FindAndSetUpLevel;
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
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Can't Save", "You can't save a level in Play Mode, please exit Play Mode and try again.", "Ok");
            return;
        }

        GameObject root = _levelManager.transform.parent.gameObject;

        // By default, we are good to save level
        bool save = true;

        // If level is new, make sure they know to add a unique name (add unique name check later)
        if(_isNewLevel == true)
        {
            save = EditorUtility.DisplayDialog("Trying to Save New Level", "Looks like this level is new, make sure you gave it a unique name", "I did, let's save", "Cancel");
        }
        // If level is old and has a new name, make sure they know this will make a new asset
        else if(_isNewLevel == false && _originalName != _levelManager.transform.parent.name)
        {
            save = EditorUtility.DisplayDialog("New Name", "This level's name was " + _originalName + " and is now " + _levelManager.transform.parent.name + ", this will create a new asset.", "That's OK", "Cancel");
        }

        // If we're good to save, save
        if (save)
        {
            string saveFolder = EditorUtility.OpenFolderPanel("Select a Level Directory", "Assets/Levels", "");
            PrefabUtility.SaveAsPrefabAsset(root, saveFolder + "/" + root.name.Replace("(clone)", "") + ".prefab");
            _isNewLevel = false;
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
        _levelManager = (PrefabUtility.InstantiatePrefab(levelPrefab) as GameObject).GetComponentInChildren<LevelManager>();

        _levelManager.transform.parent.name = _levelManager.transform.parent.name.Replace("(Clone)", "");
        _levelManager.SetPlayer(_player);

        // Set Class Variables
        _originalName = _levelManager.transform.parent.name;
        _isNewLevel = false;
    }


    private void SpawnLevelPrefab()
    {
        GameObject levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Level Prefab.prefab");

        _levelManager = Instantiate(levelPrefab).GetComponentInChildren<LevelManager>();
        _levelManager.transform.parent.name = _levelManager.transform.parent.name.Replace("(Clone)", "");
        _levelManager.SetPlayer(_player);
        _isNewLevel = true;
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
        LineController linePrefab = AssetDatabase.LoadAssetAtPath<LineController>("Assets/Prefabs/LevelComponents/Line.prefab");

        LineController newLine = (LineController)PrefabUtility.InstantiatePrefab(linePrefab, _levelManager.LineParent.transform);
        // _levelManager.AddNewLine(newLine);
    }

    private void SpawnDangerZone()
    {
        DangerZone dangerZonePrefab = AssetDatabase.LoadAssetAtPath<DangerZone>("Assets/Prefabs/LevelComponents/DangerZone.prefab");

        PrefabUtility.InstantiatePrefab(dangerZonePrefab, _levelManager.DangerZoneParent.transform);
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
}
