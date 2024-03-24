using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFlagChanger : MonoBehaviour
{
    public string FlagName;
    public Toggle CheckBox;
    public TextMeshProUGUI NameText;

    private void OnEnable()
    {
        GetFlagData();
    }

    public void SetName(string newName)
    {
        FlagName = newName;
        NameText.text = FlagName;
        GetFlagData();
    }

    public void GetFlagData()
    {
        if (FlagName != "")
        {
            CheckBox.isOn = GameManager.Instance.Flags[FlagName];
        }
    }

    public void SetFlag(bool newValue)
    {
        GameManager.Instance?.SetFlag(FlagName, newValue);
    }
    

}
