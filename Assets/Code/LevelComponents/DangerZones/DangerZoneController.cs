using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneController : MonoBehaviour
{
    [Header("References")]
    public BoxCollider2D BoxTrigger;
    public PolygonCollider2D PolygonCollider;
    [SerializeField] private SpriteRenderer _boarderRenderer;
    [SerializeField] private SpriteRenderer _gridRenderer;
    [SerializeField] private GameObject _gridMask;
    [Header("Sprites")]
    [SerializeField] private Sprite _cornersBoarder;
    [SerializeField] private Sprite _noCornersBoarder;

    private void Awake()
    {
        if(BoxTrigger == null)
        {
            BoxTrigger = GetComponent<BoxCollider2D>();
        }

        SetUpSprite();
    }

    private void SetUpSprite()
    {
        if(BoxTrigger.size.x <= 1 && BoxTrigger.size.y <= 1)
        {
            _boarderRenderer.sprite = _noCornersBoarder;
            _gridMask.SetActive(true);
            BoxTrigger.enabled = false;
            PolygonCollider.enabled = true;
        }
        _boarderRenderer.enabled = true;
        _boarderRenderer.size = BoxTrigger.size;

        _gridRenderer.enabled = true;
        _gridRenderer.size = BoxTrigger.size;
    }
}
