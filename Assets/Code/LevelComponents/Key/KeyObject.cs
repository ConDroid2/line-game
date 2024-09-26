using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyObject : MonoBehaviour
{

    public bool InUse = false;
    [SerializeField] private FlagSetter _flagSetter;
    public SpringJoint2D SpringJoint;
    [SerializeField] private GameObject _explodeEffect;
    [SerializeField] private GameObject _visuals;
    public Rigidbody2D FollowRigidbody;


    // Update is called once per frame

    public void HandlePlayerInRange(Collider2D collider)
    {
        if (SpringJoint.enabled == false && collider.CompareTag("Player"))
        {
            _flagSetter.SetFlag(true);
            InUse = true;

            collider.GetComponent<KeyHolder>().AddKey(this);

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
