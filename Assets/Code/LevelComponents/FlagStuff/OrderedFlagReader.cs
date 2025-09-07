using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrderedFlagReader : MonoBehaviour
{
    [System.Serializable]
    public class Reader
    {
        public string FlagName;
        public UnityEvent IfFlagTrue;
    }

    public List<Reader> IfTrueReaders = new List<Reader>();

    [SerializeField] private UnityEvent IfAllFalse;

    void Start()
    {
        if (GameManager.Instance == null) return;


        CheckAllFlags();
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        CheckAllFlags();
        GameManager.Instance.OnSetFlag += HandleFlagChange;
    }

    private void CheckAllFlags()
    {
        if (GameManager.Instance == null) return;

        foreach(Reader reader in IfTrueReaders)
        {
            if (GameManager.Instance.Flags.ContainsKey(reader.FlagName))
            {
                if(GameManager.Instance.Flags[reader.FlagName] == true)
                {
                    reader.IfFlagTrue?.Invoke();
                    return;
                }
            }
        }

        IfAllFalse?.Invoke();
    }



    private void HandleFlagChange(string changedFlagName, bool newFlagValue)
    {
        foreach(Reader reader in IfTrueReaders)
        {
            if(changedFlagName == reader.FlagName && newFlagValue == true)
            {
                reader.IfFlagTrue?.Invoke();
                return;
            }
        }

        IfAllFalse?.Invoke();
    }
}
