using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using UnityEngine.Events;

public class ColorAlphaModulator : MonoBehaviour
{
    public Timer timer;
    private UnityAction<float> timerAction;
    private ShapeRenderer _shape;

    public bool firstCallIsIncrease;
    private bool trueIncreaseFalseDecrease;
    private bool modulationInProgress = false;

    private int currentCallNumber = 0; // number to hold the current call
    public int callsToIgnore; // this object will ignore this many calls to events to change the intensity.

    public float maxAlpha; // the max alpha for this object, must be between 0 and 1
    public float minAlpha; // the min alpha for this object, must be between 0 and 1

    // Start is called before the first frame update
    void Start()
    {
        _shape = GetComponent<ShapeRenderer>();

        if (timerAction is null)
        {
            Debug.Log("Timer Action was null before it was set to timer event");
        }

        if (timer is null)
        {
            Debug.Log("Timer is null");
        }

        //timer.OnPercentDoneChange.AddListener(timerAction); // add this dynamic action to the timer
        timer.OnTimerEnd.AddListener(ChangeIncreaseDecreaseAction); // add this action to the timer on completion which changes the action done onPercent Change.
        timer.OnTimerEnd.AddListener(() => Debug.Log("Timer completed\n"));
        timer.OnTimerEnd.AddListener(() => this.modulationInProgress = false); ;

        trueIncreaseFalseDecrease = firstCallIsIncrease;
        if (firstCallIsIncrease)
        {
            //timerAction = IncreaseAlpha;
            timer.OnPercentDoneChange.AddListener(IncreaseAlpha); // add this dynamic action to the timer
            this.SetIntensity(minAlpha);
        }
        else
        {
            //timerAction = ReduceAlpha;
            timer.OnPercentDoneChange.AddListener(ReduceAlpha); // add this dynamic action to the timer
            this.SetIntensity(maxAlpha);
        }

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void SetIntensity(float alphaLevel)
    {
        // Note that level must be between 0 and 1; 0 = transparent, and 1 = opaque.
        float r = _shape.Color.r;
        float b = _shape.Color.b;
        float g = _shape.Color.g;
        _shape.Color = new Color(r, g, b, alphaLevel);
    }

    private void ReduceAlpha(float fractionComplete)
    {
        this.Interpolate(fractionComplete, this.maxAlpha, this.minAlpha);
    }

    private void IncreaseAlpha(float fractionComplete)
    {
        this.Interpolate(fractionComplete, this.minAlpha, this.maxAlpha);
    }

    private void Interpolate(float fractionComplete, float y1, float y2)
    {
        float value = y1 + (y2 - y1) * fractionComplete;
        this.SetIntensity(value);
    }

    public void ModulateAlpha()
    {
        Debug.Log("ModulateAlpha invoked\n");
        Debug.Log($"CurrentCallNumber = {this.currentCallNumber}\n");
        
        if (this.currentCallNumber == 0 && !this.modulationInProgress) // only do the alpha setting and stuff when the call is set to zero.
        {
            // When this is called, do the work in the timer, which will call the action set to the OnPercentDoneChange action
            timer.StartTimer();
            this.modulationInProgress = true;
            Debug.Log($"Timer Started\n");
            // When the timer finishes, it has to swtich the increase/decrease action to use the other function than currently stored

        }

        this.currentCallNumber = this.currentCallNumber + 1 > this.callsToIgnore ? 0 : this.currentCallNumber + 1;
        Debug.Log($"NewCallNumber = {this.currentCallNumber}\n");
    }

    private void ChangeIncreaseDecreaseAction()
    {
        Debug.Log("ChangeIncreaseDecreaseAction called\n");

        if (this.trueIncreaseFalseDecrease) // if true, this means the last operation that was called (and just completed) was an increase in alpha
        {
            Debug.Log("trueIncreaseFalseDecrease is true\n");
            // therefore set the action to reduce, and set trueIncreaseFalseDecrease to false
            //this.timerAction = this.ReduceAlpha;
            timer.OnPercentDoneChange.RemoveListener(IncreaseAlpha);
            timer.OnPercentDoneChange.AddListener(ReduceAlpha);
        }
        else // trueIncreaseFalseDecrease is false, therefore the last operation that occured was a reduction in intensity.
        {
            Debug.Log("trueIncreaseFalseDecrease is false\n");
            // therefore set the action to increase alpha.
            //this.timerAction = this.IncreaseAlpha;
            timer.OnPercentDoneChange.RemoveListener(ReduceAlpha);
            timer.OnPercentDoneChange.AddListener(IncreaseAlpha);
        }

        // reverse trueIncreaseFalseDecrease so that the action switches when this is called again.
        this.trueIncreaseFalseDecrease = !this.trueIncreaseFalseDecrease;
    }
}
