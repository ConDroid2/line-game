using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlagMasterSetEditor : EditorWindow
{
    List<string> flagMasterSetNames = new List<string>();
    FlagsClass flags = null;

    string newFlagName;
    string masterSetName;

    [MenuItem("Custom/Flag Editor")]
    public static void ShowWindow()
    {
        GetWindow<FlagMasterSetEditor>();
    }

    private void OnGUI()
    {
        if (flags == null)
        {
            foreach (string masterSetName in flagMasterSetNames)
            {
                if (GUILayout.Button(masterSetName))
                {
                    LoadMasterSet(masterSetName);
                }
            }
            if (GUILayout.Button("New Master Set"))
            {
                LoadMasterSet("");
            }
        }
        else
        {
            masterSetName = GUILayout.TextField(masterSetName, GUILayout.MinWidth(50), GUILayout.Width(150));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Flag Set", GUILayout.MinWidth(50), GUILayout.Width(150)))
            {
                SaveFlagSet();
            }
            if (GUILayout.Button("Cancel", GUILayout.MinWidth(50), GUILayout.Width(150)))
            {
                BackToSetList();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(15f);
            // Need to if check again here because if we cancel, flags is null but it finishes running all this code
            if (flags != null)
            {
                string flagToDelete = "";
                foreach (string flag in flags.FlagNames)
                {
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Del", GUILayout.Width(30)))
                    {
                        flagToDelete = flag;
                    }
                    GUILayout.Label(flag);
                    
                    GUILayout.EndHorizontal();
                }

                if(flagToDelete != "")
                {
                    flags.FlagNames.Remove(flagToDelete);
                }

                GUILayout.BeginHorizontal();

                newFlagName = GUILayout.TextField(newFlagName, GUILayout.MinWidth(50), GUILayout.Width(150));
                if (GUILayout.Button("Add New Flag", GUILayout.MinWidth(50), GUILayout.Width(150)))
                {
                    AddNewFlag(newFlagName);
                }

                GUILayout.EndHorizontal();
            }
        }
    }

    private void Awake()
    {
        GetAllFlagMasterSets();
    }

    private void GetAllFlagMasterSets()
    {
        flagMasterSetNames = new List<string>();
        foreach(string flagMasterSetFilePath in System.IO.Directory.EnumerateFiles(Application.dataPath + "/FlagMasterSets", "*.txt"))
        {
            string setName = System.IO.Path.GetFileName(flagMasterSetFilePath).Replace(".txt", "");

            flagMasterSetNames.Add(setName);
        }
    }

    private void LoadMasterSet(string masterSetName)
    {
        if(masterSetName == "")
        {
            flags = new FlagsClass();
            this.masterSetName = "";
        }
        else
        {
            JsonUtilities utils = new JsonUtilities(Application.dataPath + "/FlagMasterSets");

            this.masterSetName = masterSetName;
            flags = utils.LoadData<FlagsClass>("/" + masterSetName + ".txt");
        }
    }

    private void AddNewFlag(string newFlag)
    {
        if (flags.FlagNames.Add(newFlag) == false)
        {
            EditorUtility.DisplayDialog("New Flag Error", "Flag Already Exists", "OK");
        }
        else
        {
            newFlagName = "";
        }
    }

    private void SaveFlagSet()
    {
        if(masterSetName == "")
        {
            EditorUtility.DisplayDialog("No Name", "Please name the flag set", "OK");
            return;
        }
        else
        {
            JsonUtilities utils = new JsonUtilities(Application.dataPath + "/FlagMasterSets");

            utils.SaveData("/" + masterSetName + ".txt", flags);

            BackToSetList();
        }
    }

    private void BackToSetList()
    {
        flags = null;
        masterSetName = "";
        GetAllFlagMasterSets();
    }
}
