using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    [SerializeField] Timer _timer;
    [SerializeField] ParticleSystem _particleSystem;

    public void PlayEffect(Vector3 position, Vector2 direction)
    {
        transform.position = position;
        transform.up = direction;

        _particleSystem.Play();

        _timer.StartTimer();
    }

    public void DestoryEffect()
    {
        Destroy(gameObject);
    }
}
