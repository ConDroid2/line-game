using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicSyncingTest : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public int CurrentBeat = 0;

    public void OnBeatChange()
    {
        CurrentBeat++;

        Text.text = CurrentBeat.ToString();
    }
}
