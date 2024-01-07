using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldSelectorButton : MonoBehaviour
{
    public UnityEvent<string> WorldSelected;
    public string WorldName = "";

    public void FireWorldSelected()
    {
        WorldSelected.Invoke(WorldName)
;   }
}
