using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ButtonCycle : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    private int _currentPhase = 0;
    [SerializeField] List<ButtonCyclePhase> _phases = new List<ButtonCyclePhase>();

    private void Start()
    {
        if (_phases.Count > _currentPhase)
        {
            _buttonText.text = _phases[_currentPhase].PhaseName;
        }
    }

    public void CyclePhase()
    {
        _currentPhase = (_currentPhase + 1) % _phases.Count;

        _buttonText.text = _phases[_currentPhase].PhaseName;
        _phases[_currentPhase].OnPhaseEntered?.Invoke();
    }

    public void SetPhase(int phaseNumber)
    {
        if(phaseNumber < _phases.Count)
        {
            _currentPhase = phaseNumber;

            _buttonText.text = _phases[_currentPhase].PhaseName;
            _phases[_currentPhase].OnPhaseEntered?.Invoke();
        }
    }

    [System.Serializable]
    public class ButtonCyclePhase
    {
        public string PhaseName;
        public UnityEvent OnPhaseEntered;
    }
}
