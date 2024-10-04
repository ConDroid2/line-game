using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDisintegrator : MonoBehaviour
{
    [SerializeField] private Timer _disintegrationTimer;
    [SerializeField] private Timer _comeBackTimer;

    private void Start()
    {
        // Debug.Log("Grabbing stuff and setting events");
        LineController lineController = GetComponentInParent<LineController>();
        LineDrawer lineDrawer = GetComponentInParent<LineDrawer>();

        _disintegrationTimer.OnTimerEnd.AddListener(delegate { lineController.SetActive(false); });
        _disintegrationTimer.OnPercentDoneChange.AddListener(lineDrawer.SetAlpha);

        _comeBackTimer.OnTimerEnd.AddListener(delegate { lineController.SetActive(true); });
        _comeBackTimer.OnTimerEnd.AddListener(delegate { lineDrawer.SetAlpha(0f); });

        lineController.OnObjectAddedToLine.AddListener(HandleObjectAddedToLine);
    }

    public void HandleObjectAddedToLine(GameObject newObject)
    {
        if (newObject.CompareTag("Player"))
        {
            _disintegrationTimer.StartTimer();
        }
    }
}
