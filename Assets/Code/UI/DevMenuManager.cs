using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenuManager : MonoBehaviour
{
    [SerializeField] private UIFlagChanger _flagChangerPrefab;
    [SerializeField] private GameObject _roomFlagsParent;

    private List<GameObject> _flagChangers = new List<GameObject>();

    private void OnEnable()
    {
        for(int i = 0; i < _flagChangers.Count; i++)
        {
            Destroy(_flagChangers[i]);
        }

        _flagChangers = new List<GameObject>();

        HashSet<string> flagNames = new HashSet<string>();

        foreach(FlagReader flagReader in FindObjectsOfType<FlagReader>())
        {
            flagNames.Add(flagReader.GetFlagName());
        }

        foreach(string flagName in flagNames)
        {
            if (flagName == "ShootUnlocked" || flagName == "GrappleUnlocked" || flagName == "RotateUnlocked") continue;

            UIFlagChanger newFlagChanger = Instantiate(_flagChangerPrefab, _roomFlagsParent.transform);

            newFlagChanger.SetName(flagName);

            _flagChangers.Add(newFlagChanger.gameObject);
        }
    }

    public void HandleDebugLogsClicked()
    {
        Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
    }
}
