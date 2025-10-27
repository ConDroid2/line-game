using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneUtilities : MonoBehaviour
{
    public void StopPlayerMovement()
    {
        if (Player.Instance != null)
        {
            Player.Instance.SetAllowInput(false);
        }
    }

    public void AllowPlayerMovement()
    {
        if (Player.Instance != null)
        {
            Player.Instance.SetAllowInput(true);
        }
    }
}
