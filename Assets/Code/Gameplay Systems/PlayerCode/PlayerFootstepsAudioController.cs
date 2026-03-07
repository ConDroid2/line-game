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

            CurrentMaterial = newLine.MaterialType;

            TurnOnLineMaterialSound();
        }

        
    }

    public void TurnOffLineMaterialSound()
    {
        if(CurrentMaterial?.EndSoundEvent != null)
        {
            CurrentMaterial.EndSoundEvent.Post(gameObject);
        }
    }

    public void TurnOnLineMaterialSound()
    {
        if (CurrentMaterial.StartSoundEvent != null)
        {
            CurrentMaterial.StartSoundEvent.Post(gameObject);
        }
    }

    public void ClearCurrentLineMaterial()
    {
        CurrentMaterial = null;
    }
}
