using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LineMaterial")]
public class LineMaterial : ScriptableObject
{
    public Enums.LineMaterial MaterialType;
    public AK.Wwise.Event StartSoundEvent;
    public AK.Wwise.Event EndSoundEvent;
}
