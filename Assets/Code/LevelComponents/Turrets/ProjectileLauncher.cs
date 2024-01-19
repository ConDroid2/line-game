using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float projectileStartDistance = 1f;
    [SerializeField] private ProjectileMover _projectilePrefab;

    [SerializeField] private bool _fireOnAwake = false;
    [SerializeField] private float _fireInterval = 1;
    [SerializeField] private ProjectileData _projectileData;

    private float _timeSinceLastFire = 0f;
    private Vector3 _projectileStartPosition => transform.position + (-1 * transform.up * projectileStartDistance);
    private void Awake()
    {
        _projectileData.Direction = -1 * transform.up;
        if (_fireOnAwake)
        {
            Fire();
        }
    }

    private void Update()
    {
        // Needed for if turret has a mover or rotater
        _projectileData.Direction = -1 * transform.up;


        if(_timeSinceLastFire >= _fireInterval)
        {
            Fire();
            _timeSinceLastFire = 0f;
        }

        _timeSinceLastFire += Time.deltaTime;
    }

    private void Fire()
    {
        ProjectileMover newProjectile = Instantiate(_projectilePrefab, _projectileStartPosition, transform.rotation);
        // This is passing by reference
        newProjectile.Fire(_projectileData);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_projectileStartPosition, 0.1f);
    }
}
