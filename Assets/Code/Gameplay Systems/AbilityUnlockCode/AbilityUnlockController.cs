using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlockController : MonoBehaviour
{
    private void OnEnable()
    {
        if(Player.Instance != null)
        {
            Player.Instance.SetAllowMove(false);
        }
    }

    public void OnCloseMenu()
    {
        if (Player.Instance != null)
        {
            Player.Instance.SetAllowMove(true);
        }
    }
}
