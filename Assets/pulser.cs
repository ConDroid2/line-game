using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulser : MonoBehaviour
{
    private bool pulseStart = false;
    private bool pulseHappening = false;
    private float remainingTime = 0;
    public float pulseTime;
    public float scaleChangeAmount;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Constant(0, 1, 0);

    private float _initialScale;
    private float _targetScale;

    private void Awake()
    {
        _initialScale = transform.localScale.x;
        _targetScale = _initialScale + scaleChangeAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(pulseStart)
        {
            transform.localScale = new Vector3(_targetScale, _targetScale, _targetScale);
            remainingTime = pulseTime;
            pulseHappening = true;
            pulseStart = false;
        }

        if(pulseHappening)
        {
            remainingTime -= Time.deltaTime;

            float currentScale = Mathf.Lerp(_initialScale, _targetScale, _animationCurve.Evaluate(remainingTime / pulseTime));

            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            // Debug.Log(remainingTime);
            if(remainingTime <= 0)
            {
                transform.localScale = new Vector3(_initialScale, _initialScale, _initialScale);
                pulseHappening = false;
            }
        }

    }

    public void Pulse()
    {
        pulseStart = true;
    }

}
