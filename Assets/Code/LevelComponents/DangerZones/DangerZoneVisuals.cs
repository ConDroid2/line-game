using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneVisuals : MonoBehaviour
{
    public BoxCollider2D Trigger;

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
}
