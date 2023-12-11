using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveLevelWindow : EditorWindow
{
    LevelManager _levelManager;
    string _currentLevelName;
    string _newLevelName;

    // This class assumes there is Level Manager, check in code before opening this window
    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _currentLevelName = _levelManager.transform.parent.name;
        _newLevelName = _currentLevelName;
    }
    private void OnGUI()
    {
        GUILayout.Label("Level Name", EditorStyles.boldLabel);
        _newLevelName = GUILayout.TextField(_newLevelName);

        EditorGUILayout.Space();

        if(GUILayout.Button("Finish Save"))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        if (Application.isPlaying)
        {
            EditorUtility.DisplayDialog("Can't Save", "You can't save a level in Play Mode, please exit Play Mode and try again.", "Ok");
            return;
        }
        
        // The root object we're trying to save
        GameObject root = _levelManager.transform.parent.gameObject;

        // Keep track of if it's ok to save this level
        bool canSave = true;


        bool levelIsNew = _currentLevelName == "Level Prefab";
        bool nameIsDifferent = _currentLevelName != _newLevelName;

        // Level is new, name hasn't been changed
        if (levelIsNew && nameIsDifferent == false)
        {
            EditorUtility.DisplayDialog("Naming Error", "You must input a new name for new levels", "Ok");
            return;
        }

        string saveFolder = EditorUtility.OpenFolderPanel("Select a Level Directory", "Assets/Levels", "");
        // Trim the path up until "Asset"
        int beginningOfAssetPath = saveFolder.IndexOf("Asset");
        string updatedAssetPath = saveFolder.Substring(beginningOfAssetPath);

        

        // Level is edited, name is same
        if (levelIsNew == false && nameIsDifferent == false)
        {
            canSave = EditorUtility.DisplayDialog("Name Check", "These changes will overwrite the saved level", "Ok", "Change Name");
        }

        // Name has been changed
        if (nameIsDifferent) 
        {
            GameObject existingLevel = AssetDatabase.LoadAssetAtPath<GameObject>(updatedAssetPath + "/" + _newLevelName + ".prefab");
            bool levelWithSameNameExists = existingLevel != null;

            if (levelWithSameNameExists)
            {
                EditorUtility.DisplayDialog("Name Error", "A Level with this name already exists, cannot save", "Ok");
                canSave = false;
            }

            // Level is edited, name is different
            if (levelIsNew == false && nameIsDifferent && canSave)
            {
                bool deleteOldAsset = EditorUtility.DisplayDialog("Name Check", "You have changed the name of this level, do you want to delete the old version?", "Yes", "Keep Old Version");

                if (deleteOldAsset)
                {
                    AssetDatabase.DeleteAsset(updatedAssetPath + "/" + _currentLevelName + ".prefab");
                }
            }
        }

        if (canSave)
        {
            root.name = _newLevelName;
            PrefabUtility.SaveAsPrefabAsset(root, saveFolder + "/" + _newLevelName + ".prefab");
            Close();
        }  
    }
}
