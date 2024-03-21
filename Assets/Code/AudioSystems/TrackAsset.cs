using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Track Asset", menuName = "Audio System/Track Asset")]
public class TrackAsset : ScriptableObject
{
    public AudioClip IntroTrack;
    public AudioClip LoopTrack;
    public int BPM;
    public int TimeSignature;
}
