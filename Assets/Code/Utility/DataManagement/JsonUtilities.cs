using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonUtilities
{
    private string _pathRoot;

    public JsonUtilities(string root)
    {
        _pathRoot = root;
    }
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = _pathRoot + relativePath;
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists at path, deleting old data and creating new file");
                File.Delete(path);  
            }

            using FileStream stream = File.Create(path);
            stream.Close();

            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            Debug.Log("Successfully saved data");

            return true;
        }
        catch(System.Exception e)
        {
            Debug.LogError("Something went wrong while saving data: " + e.Message + e.StackTrace);
            return false;
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = _pathRoot + relativePath;

        if(File.Exists(path) == false)
        {
            Debug.LogError("No data found at path: " + path);
            throw new FileNotFoundException("Path does not exist: " + path);
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
