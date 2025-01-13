using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeBiggerWhenSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    //private float _scaleFactor = 1;
    public float ScaleFactor = 1;

    private Vector3 originalScaleFactors;

    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log($"De-selected, original x scale is {originalScaleFactors.x}");
        transform.localScale = originalScaleFactors;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log($"Selected, scale factor is {ScaleFactor}");
        transform.localScale *= ScaleFactor;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalScaleFactors = transform.localScale;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
