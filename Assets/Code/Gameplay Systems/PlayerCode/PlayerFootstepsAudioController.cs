using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepsAudioController : MonoBehaviour
{
    public LineMaterial CurrentMaterial;
    
    public void HandleMovedToNewLine(LineController newLine)
    {
        if(newLine.MaterialType != CurrentMaterial)
        {
            TurnOffLineMaterialSound();

            if(newLine.MaterialType.StartSoundEvent != null)
            {
                newLine.MaterialType.StartSoundEvent.Post(gameObject);
            }
        }

        CurrentMaterial = newLine.MaterialType;
    }

    public void TurnOffLineMaterialSound()
    {
        if(CurrentMaterial?.EndSoundEvent != null)
        {
            CurrentMaterial.EndSoundEvent.Post(gameObject);
        }
    }

    public void ClearCurrentLineMaterial()
    {
        CurrentMaterial = null;
    }
}
