using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementSetter : MonoBehaviour
{
    public enum Achievements
    {
        Shoot,
        Grapple,
        Rotate,
        BigKey,
        Firewall,
        Win
    }

    private Dictionary<Achievements, string> ACHIEVEMENTS_DICTIONARY = new()
    {
        { Achievements.Shoot, "ATL_Shoot" },
        { Achievements.Grapple, "ATL_Grapple" },
        { Achievements.Rotate, "ATL_Rotate" },
        { Achievements.BigKey, "ATL_BigKey" },
        { Achievements.Firewall, "ATL_Firewall" },
        { Achievements.Win, "ATL_Win" }
    };

    // The Achievement we'll be unlocking
    public Achievements achievement;

    public void UnlockThisAchievement()
    {
        if (SteamManager.Initialized)
        {
            string name = ACHIEVEMENTS_DICTIONARY[achievement];
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
    }

    public void ClearThisAchievement()
    {
        if (SteamManager.Initialized)
        {
            string name = ACHIEVEMENTS_DICTIONARY[achievement];

            SteamUserStats.ClearAchievement(name);
            SteamUserStats.StoreStats();
        }
    }
}
