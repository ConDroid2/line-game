using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Pushable Force Data")]
    [SerializeField] private float _pushableForce;
    [SerializeField] private float _drag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy destructables
        if (collision.collider.gameObject.CompareTag("PlayerDestructable"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        // Push pushables
        else if (collision.collider.gameObject.CompareTag("Pushable"))
        {
            
            Vector2 forceDirection = collision.contacts[0].normal * -1;

            LineMovementController movementController = collision.gameObject.GetComponent<LineMovementController>();

            movementController.AddForce(new Force(_pushableForce, forceDirection, _drag, gameObject));
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
    }
}
