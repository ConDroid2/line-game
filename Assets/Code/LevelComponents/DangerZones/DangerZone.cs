using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public BoxCollider2D Trigger;

    private MeshFilter _meshFilter;

    private void Awake()
    {
        if(Trigger == null)
        {
            Trigger = GetComponent<BoxCollider2D>();
        }

        SetUpSprite();
    }

    private void SetUpSprite()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;

        sprite.size = Trigger.size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.GetKilled();
        }
    }
}
