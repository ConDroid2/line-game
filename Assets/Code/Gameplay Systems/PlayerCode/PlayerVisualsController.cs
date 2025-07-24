using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerVisual _timerVisual;

    // Tracking variables
    private bool _selfDestructHeld = false;
    private BaseControls _baseControls;

    public void HandleSelfDestructStarted(BaseControls baseControls)
    {
        _selfDestructHeld = true;
        _timerVisual.TurnOnDisc();
        _baseControls = baseControls;
    }

    public void HandleSelfDestructCompleted()
    {
        _timerVisual.TurnOffDisc();
    }

    private void Update()
    {
        if (_selfDestructHeld)
        {
            _timerVisual.UpdateEndAngle_FromEmpty(_baseControls.PlayerMap.SelfDestruct.GetTimeoutCompletionPercentage());
        }
    }
}
