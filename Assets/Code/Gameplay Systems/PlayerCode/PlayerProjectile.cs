using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerDestructable"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
