using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicSyncingTest : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public int CurrentBeat = 0;

    public void OnBeatChange(AkEventCallbackMsg message)
    {
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)message.info;

        Debug.Log($"Got user cue: {message.type}");
        Debug.Log($"Beat segment info: {info}");
        Debug.Log($"Name: {info.userCueName}");

        CurrentBeat++;

        Text.text = CurrentBeat.ToString();
    }

    public void OnUserCue(object in_cookie, AkCallbackType type, object in_info)
    {
        AkMarkerCallbackInfo info = (AkMarkerCallbackInfo)in_info;
        Debug.Log($"Got user cue: {type.ToString()}");
        Debug.Log($"Cue name: {info.strLabel}");
    }
}
