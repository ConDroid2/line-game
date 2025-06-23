using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        if(sceneName == "noChange")
        {
            return; // Do nothing, left in for debug purposes
        }

        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception e)
        {
            Debug.Log($"Scene {sceneName} not found in project");
        }

    }
}
