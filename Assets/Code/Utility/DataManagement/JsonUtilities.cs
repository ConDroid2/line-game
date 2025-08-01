using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class JsonUtilities
{
    private string _pathRoot;
    private JsonSerializerSettings _serializerSettings;
    private Newtonsoft.Json.Serialization.DiagnosticsTraceWriter _traceWriter;

    public JsonUtilities(string root)
    {
        _pathRoot = root;

        _traceWriter = new Newtonsoft.Json.Serialization.DiagnosticsTraceWriter();
        _serializerSettings = new JsonSerializerSettings { TraceWriter = _traceWriter };
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

            File.WriteAllText(path, JsonConvert.SerializeObject(data, _serializerSettings));
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
            Debug.Log("Deserializing data");
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), _serializerSettings);
            Debug.Log(data);
            return data;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public void DeleteDataAt(string relativePath)
    {
        string path = _pathRoot + relativePath;

        if (File.Exists(path) == true)
        {
            try
            {
                File.Delete(path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete save data: {e.Message} {e.StackTrace}");
            }
        }
    }

    public T LoadFromResources<T>(string relativePath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(relativePath);

        try
        {
            T data = JsonConvert.DeserializeObject<T>(textAsset.text);
            return data;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public T[] LoadAllFromResources<T>(string relativePath)
    {
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>(relativePath);

        try
        {
            T[] data = new T[textAssets.Length];

            for(int i = 0; i < textAssets.Length; i++)
            {
                TextAsset textAsset = textAssets[i];

                data[i] = JsonConvert.DeserializeObject<T>(textAsset.text);
            }

            return data;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
