using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateSetter : MonoBehaviour
{

    public int TargetFrameRate = 60;


    private void Awake()
    {
        // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;
    }
}
