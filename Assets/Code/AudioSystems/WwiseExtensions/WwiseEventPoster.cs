using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventPoster : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event _wwiseEvent;


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PostEvent();
        //}
    }

    public void PostEvent()
    {
        if(AudioManager.Instance != null)
        {
            _wwiseEvent.Post(gameObject);
        }
    }
}
