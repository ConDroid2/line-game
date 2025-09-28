using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThreeArrowActivatorScript : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public UnityEvent Event0;
    public UnityEvent Event1;
    public UnityEvent Event2;

    private int internalCount = 0;

    public void ActivateNext()
    {
        //Debug.Log($"internal count = {internalCount}");
        switch (internalCount)
        {
            case 0:
                this.Event0?.Invoke();
                break;
            case 1:
                this.Event1?.Invoke();
                break;
            case 2:
                this.Event2?.Invoke();
                break;
        }

        internalCount = internalCount >= 2 ? 0 : internalCount + 1;

    }
}
