using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField] private Transform _followPoint;
    [SerializeField] private float _followResponsiveness;
    [SerializeField] private FlagSetter _flagSetter;


    // Update is called once per frame
    void Update()
    {
        if(_followPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _followPoint.position, _followResponsiveness * Time.deltaTime);
        }
    }

    public void HandlePlayerInRange(Collider2D collider)
    {
        if (_followPoint == null && collider.CompareTag("Player"))
        {
            _flagSetter.SetFlag(true);
            _followPoint = Player.Instance.FollowPoint;

            DontDestroyOnLoad(gameObject);

            Player.Instance.OnPlayerDeath += HandlePlayerDeath;
        }
    }

    public void HandlePlayerDeath()
    {
        Player.Instance.OnPlayerDeath -= HandlePlayerDeath;

        _flagSetter.SetFlag(false);

        Destroy(gameObject);
    }

    public void Use()
    {
        Player.Instance.OnPlayerDeath -= HandlePlayerDeath;
        Destroy(gameObject);
    }
}
