using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private string _flagName;
    // [SerializeField] private bool _setFlagAs;
    

    public void SetFlag(bool _setFlagAs)
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.SetFlag(_flagName, _setFlagAs);
    }
}
