using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneVisuals : MonoBehaviour
{
    [Header("References")]
    public BoxCollider2D Trigger;
    [SerializeField] private SpriteRenderer _boarderRenderer;
    [SerializeField] private SpriteRenderer _gridRenderer;
    [SerializeField] private GameObject _gridMask;
    [Header("Sprites")]
    [SerializeField] private Sprite _cornersBoarder;
    [SerializeField] private Sprite _noCornersBoarder;

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
        if(Trigger.size.x <= 1 && Trigger.size.y <= 1)
        {
            _boarderRenderer.sprite = _noCornersBoarder;
            _gridMask.SetActive(true);
        }
        _boarderRenderer.enabled = true;
        _boarderRenderer.size = Trigger.size;

        _gridRenderer.enabled = true;
        _gridRenderer.size = Trigger.size;
    }
}
