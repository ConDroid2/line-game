using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagReader : MonoBehaviour
{
    [SerializeField] private string _flagName;
    [SerializeField] bool _onlyCheckOnStart = false;
    [SerializeField] bool _readFlagOnEnable = false;

    public UnityEvent OnIfTrue;
    public UnityEvent OnIfFalse;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null) return;

        HandleFlagChange(_flagName, GameManager.Instance.Flags[_flagName]);
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        if (_readFlagOnEnable)
        {
            HandleFlagChange(_flagName, GameManager.Instance.Flags[_flagName]);
        }

        if (_onlyCheckOnStart == false)
        {
            GameManager.Instance.OnSetFlag += HandleFlagChange;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;

        if (_onlyCheckOnStart == false)
        {
            GameManager.Instance.OnSetFlag -= HandleFlagChange;
        }
    }
    public void HandleFlagChange(string changedFlagName, bool newFlagValue)
    {
        if (changedFlagName == _flagName)
        {
            if (newFlagValue == true) OnIfTrue.Invoke();
            else OnIfFalse.Invoke();
        }
    }

    public string GetFlagName()
    {
        return _flagName;
    }
}
