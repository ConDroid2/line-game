using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMover : MonoBehaviour
{
    private Transform _transform;

    private int _moveTime = 1;
    private float _timeMoving = 0f;
    private int direction = 1;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeMoving += Time.deltaTime;

        if(_timeMoving >= _moveTime)
        {
            direction *= -1;
            _timeMoving = 0;
        }

        _transform.position += (new Vector3(0f, Time.deltaTime, 0f) * direction);
    }
}
