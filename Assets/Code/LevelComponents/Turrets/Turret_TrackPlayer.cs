using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_TrackPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Player.Instance != null)
        {
            transform.up = (Player.Instance.transform.position - transform.position) * -1;
        }
    }
}
