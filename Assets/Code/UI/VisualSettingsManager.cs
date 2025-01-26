using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSettingsManager : MonoBehaviour
{
    public void SetBrightness(float newBrightness)
    {
        Screen.brightness = newBrightness;
    }
}
