using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private float _aimingSpeed = 4f;
    private bool _active = false;

    public void SetAim(Vector2 aimVector)
    {
        if(_active == false)
        {
            return;
        }

        // This is sort of the effect we want but it behaves a little strangely, maybe just use a straight up Lerp or something
        // Vector2 newUp = Vector2.MoveTowards(transform.up, aimVector, _aimingSpeed * Time.deltaTime);

        Vector2 newUp = Vector3.Slerp(transform.up, aimVector, _aimingSpeed * Time.deltaTime);

        // Debug.Log($"Current up: {transform.up} \n Current goal: {aimVector} \n Next frame up: {newUp}");

        transform.up = newUp;
    }

    public void Activate()
    {
        _active = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _active = false;
        gameObject.SetActive(false);
    }
}
