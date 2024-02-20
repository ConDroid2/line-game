using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlot
{
    private Dictionary<string, bool> _flags;

    public bool GetFlag(string flagName)
    {
        if (_flags.ContainsKey(flagName))
        {
            return _flags[flagName];
        }
        else
        {
            return false;
        }
    }

    public bool SetFlag(string flagName, bool value)
    {
        if (_flags.ContainsKey(flagName))
        {
            _flags[flagName] = value;
            return true;
        }
        else
        {
            return false;
        }
    }
}
