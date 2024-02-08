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
        Debug.Log($"collided with {collision.collider.gameObject.name}");
        if (collision.collider.gameObject.CompareTag("PlayerDestructable"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        else if (collision.collider.gameObject.CompareTag("Pushable"))
        {
            
            Vector2 forceDirection = collision.contacts[0].normal * -1;
            Debug.Log($"Trying to push in direction: {forceDirection}");

            LineMovementController movementController = collision.gameObject.GetComponent<LineMovementController>();

            Debug.Log(movementController.AddForce(new Force(_pushableForce, forceDirection, _drag, gameObject)));
            Destroy(gameObject);

        }
    }
}
