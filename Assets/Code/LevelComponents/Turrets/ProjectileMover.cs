using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private ProjectileData _projectileData;

    private bool _moving = false;
    private float _timeAlive = 0f;

    private void Update()
    {
        if (_moving)
        {
            _timeAlive += Time.deltaTime;

            if(_timeAlive >= _projectileData.LifeSpan)
            {
                Destroy(gameObject);
            }

            transform.position += _projectileData.Direction * _projectileData.Speed * Time.deltaTime;
        }
    }

    public void Fire(ProjectileData projectileData)
    {
        _projectileData = projectileData;

        _moving = true;
    }
}
