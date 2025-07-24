using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Shapes;

public class TimerVisual : MonoBehaviour
{

    // Objects that this script needs

    [SerializeField] private Timer _timer;
    [SerializeField] private Disc _disc;

    [Header("Settings")]
    [SerializeField] private bool _startFromFull;

    //public UnityEvent onTimerStart; //

    //Start is called before the first frame update
    void Start()
    {
        _disc.gameObject.SetActive(false);

        // Have the timer call a certain function based on if we want to start full or empty
        if (_startFromFull)
        {
            _timer.OnPercentDoneChange.AddListener(UpdateEndAngle_FromFull);
        }
        else
        {
            _timer.OnPercentDoneChange.AddListener(UpdateEndAngle_FromEmpty);
        }

        _timer.OnTimerEnd.AddListener(TurnOffDisc);
    }

    public void StartTimerWithVisual()
    {
        _timer.StartTimer();
        //this.disc.aNM = 2.0f * 3.14159f;
        _disc.gameObject.SetActive(true);
    }

    public void UpdateEndAngle(float fractionFilled)
    {
        Debug.Log(fractionFilled*180f/3.14159f);

        float radiansFilled = fractionFilled * 2.0f * 3.14159f; // convert from fraction to radians

        // when fraction filled = 1, AngRadiansEnd = -3 * pi/4
        // fractionFilled = 0 => AngRadiansEnd = pi
        // The start end is at 90 degrees, and the circle is "filled" counterclockwise through negative angles up to -270 degrees.
        // The math is the same for the circle becoming smaller; during the event, fraction filled decreases from 1 to zero, so the end point
        // of the disk will approach the start (90 degrees) and become smaller and smaller.
        _disc.AngRadiansEnd = 3.14159f/2f - radiansFilled;
    }

    public void UpdateEndAngle_FromFull(float fractionFilled)
    {
        fractionFilled = 1 - fractionFilled;

        float radiansFilled = fractionFilled * 2.0f * 3.14159f; // convert from fraction to radians
        _disc.AngRadiansEnd = 3.14159f / 2f - radiansFilled;
    }

    public void UpdateEndAngle_FromEmpty(float fractionFilled)
    {
        float radiansFilled = fractionFilled * 2.0f * 3.14159f; // convert from fraction to radians

        _disc.AngRadiansEnd = 3.14159f / 2f + radiansFilled;
    }

    public void TurnOffDisc()
    {
        _disc.gameObject.SetActive(false);
    }

    public void TurnOnDisc()
    {
        _disc.gameObject.SetActive(true);
    }

    // What should this do

    // Wait for trigger / have method get triggered by timer event (on start???)

    // When timer is triggered, start the animation

    // Animation time is the same as the time of the timer

    // Decrease the disc's filled amount while the timer is continuing (or have it use the timer decrease on time event, with scaling)

}
