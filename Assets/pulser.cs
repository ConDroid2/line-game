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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(pulseStart)
        {
            transform.localScale += new Vector3(scaleChangeAmount, scaleChangeAmount, scaleChangeAmount);
            remainingTime = pulseTime;
            pulseHappening = true;
            pulseStart = false;
        }

        if(pulseHappening)
        {
            remainingTime -= Time.deltaTime;
            // Debug.Log(remainingTime);
            if(remainingTime <= 0)
            {
                transform.localScale -= new Vector3(scaleChangeAmount, scaleChangeAmount, scaleChangeAmount);
                pulseHappening = false;
            }
        }

    }

    public void Pulse()
    {
        pulseStart = true;
    }

}
