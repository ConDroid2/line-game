using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFootstepsAudioController : MonoBehaviour
{
    public LineMaterial CurrentMaterial;
    public UnityEvent<LineMaterial> OnLineMaterialChanged;
    
    public void HandleMovedToNewLine(LineController newLine)
    {
        if(newLine.MaterialType != CurrentMaterial)
        {
            TurnOffLineMaterialSound();

            CurrentMaterial = newLine.MaterialType;

            TurnOnLineMaterialSound();

            OnLineMaterialChanged?.Invoke(CurrentMaterial);
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
