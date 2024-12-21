using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ProjectileExplosion _hitExplosionEffectPrefab;
    [Header("Pushable Force Data")]
    [SerializeField] private float _pushableForce;
    [SerializeField] private float _drag;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy destructables
        if (collision.collider.gameObject.TryGetComponent<PlayerDestructible>(out PlayerDestructible destructible))
        {
            destructible.Destruct();
            PlayHitEffect(collision);
            Destroy(gameObject);
            
        }

        // Push pushables
        else if (collision.collider.gameObject.CompareTag("Pushable"))
        {
            
            Vector2 forceDirection = collision.contacts[0].normal * -1;

            LineMovementController movementController = collision.gameObject.GetComponent<LineMovementController>();

            movementController.AddForce(new Force(_pushableForce, forceDirection, _drag, gameObject));

            PlayHitEffect(collision);
            Destroy(gameObject);

        }

        // Activate Switches that aren't Continuous
        else if(collision.collider.TryGetComponent(out Switch switchHit))
        {
            if (switchHit.SwitchType != Enums.SwitchType.Continuous)
            {
                switchHit.Activate();
                Destroy(gameObject);
            }
        }

        else if (collision.collider.TryGetComponent(out RedBlueSwitch redBlueSwitchHit))
        {
            redBlueSwitchHit.Toggle();
            Destroy(gameObject);
        }
    }

    public void PlayHitEffect(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];

        ProjectileExplosion _hitExplosionEffect = Instantiate(_hitExplosionEffectPrefab);

        _hitExplosionEffect.PlayEffect(contactPoint.point, contactPoint.normal);
    }
}
