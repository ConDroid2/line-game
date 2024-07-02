using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyObject : MonoBehaviour
{

    public bool InUse = false;
    [SerializeField] private FlagSetter _flagSetter;
    [SerializeField] private SpringJoint2D _springJoint;
    [SerializeField] private GameObject _explodeEffect;
    [SerializeField] private GameObject _visuals;


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

            _explodeEffect.SetActive(true);
        }
    }

    public void TurnOffVisuals()
    {
        _flagSetter.SetFlag(false);
        _visuals.SetActive(false);
    }

    public void DestroyKey()
    {
        Destroy(gameObject);
    }

    public void Use()
    {
        Destroy(gameObject);
    }
}
