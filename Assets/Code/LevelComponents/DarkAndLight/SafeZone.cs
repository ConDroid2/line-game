using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafeZone : MonoBehaviour
{
    public CircleCollider2D Collider;
    public bool IsActive { get; private set; }

    [Header("Private Fields")]
    [SerializeField] private bool _startActivated = true;
    [SerializeField] private GameObject _maskObject;
    [SerializeField] private SpriteSwapper _spriteSwapper;

    public static System.Action<SafeZone> OnEnabled;
    public static System.Action<SafeZone> OnDisabled;
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    private void Awake()
    {
        Collider = GetComponent<CircleCollider2D>();

        if (_startActivated)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        // OnEnabled?.Invoke(this);

        _maskObject.SetActive(true);
        IsActive = true;
        OnActivated?.Invoke();
        // _spriteSwapper.SwapSprite(0);
    }

    public void Deactivate()
    {
        // OnDisabled?.Invoke(this);

        _maskObject.SetActive(false);
        IsActive = false;
        OnDeactivated?.Invoke();
        // _spriteSwapper.SwapSprite(1);
    }

    private void OnEnable()
    {
        OnEnabled?.Invoke(this);
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke(this);
    }
}
