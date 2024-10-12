using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private ProjectileMover _projectilePrefab;

    [Header("Settings")]
    [SerializeField] private bool _automatic = true;

    [SerializeField] private bool _fireOnAwake = false;
    [SerializeField] private float _fireInterval = 1;
    [SerializeField] private ProjectileData _projectileData;

    [Header("Events")]
    public UnityEvent OnFire;


    private float _nextFireTime = 0f;

    private void Awake()
    {
        if (_fireOnAwake)
        {
            Fire();
        }
    }

    private void Update()
    {

        if (_automatic)
        {
            Fire();
        }
    }

    public void Fire()
    {
        if (Time.time >= _nextFireTime)
        {
            _projectileData.Direction = transform.up;

            ProjectileMover newProjectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
            newProjectile.Fire(new ProjectileData(_projectileData));

            _nextFireTime = Time.time + _fireInterval;

            OnFire?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.05f);
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 0.1f));
    }
}
