using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;

public class WorldBuildingManager : MonoBehaviour
{
    public WorldData CurrentWorldData;

    private void Awake()
    {
        foreach(string worldFile in System.IO.Directory.EnumerateFiles(Application.dataPath + "/Worlds", "*.txt"))
        {
            Debug.Log(worldFile);
        }
    }

    public void SetCurrentWorldData(WorldData worldData)
    {
        CurrentWorldData = worldData;
        // EditorUtility.SetDirty(this);
    }
}
