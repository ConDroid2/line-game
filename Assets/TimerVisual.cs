using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine;
using UnityEngine.Events;
using Shapes;

public class TimerVisual : MonoBehaviour
{

    // Objects that this script needs

    public Timer timer;
    [SerializeField]
    Disc disc;

    //public UnityEvent onTimerStart; //

    //Start is called before the first frame update
    void Start()
    {
        this.disc.gameObject.SetActive(false);
        this.timer.OnPercentDoneChange.AddListener((percentDone) => this.UpdateEndAngle(1-percentDone));
        this.timer.OnTimerEnd.AddListener(()=>this.TurnOffDisc());
    }

    //// Update is called once per frame
    //void Update()
    //{





    //}


    // override the start method to make the disc pop up.

    public void StartTimerWithVisual()
    {
        timer.StartTimer();
        //this.disc.aNM = 2.0f * 3.14159f;
        this.disc.gameObject.SetActive(true);
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
        disc.AngRadiansEnd = 3.14159f/2f - radiansFilled;
    }

    public void TurnOffDisc()
    {
        this.disc.gameObject.SetActive(false);
    }

    // What should this do

    // Wait for trigger / have method get triggered by timer event (on start???)

    // When timer is triggered, start the animation

    // Animation time is the same as the time of the timer

    // Decrease the disc's filled amount while the timer is continuing (or have it use the timer decrease on time event, with scaling)

}
