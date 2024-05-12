using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{

    public bool InUse = false;
    [SerializeField] private FlagSetter _flagSetter;
    [SerializeField] private SpringJoint2D _springJoint;


    // Update is called once per frame

    public void HandlePlayerInRange(Collider2D collider)
    {
        if (_springJoint.enabled == false && collider.CompareTag("Player"))
        {
            _flagSetter.SetFlag(true);
            _springJoint.enabled = true;
            InUse = true;
            _springJoint.connectedBody = Player.Instance.FollowPoint;

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
