using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{

    private Stack<KeyObject> _keys = new Stack<KeyObject>();

    private void Start()
    {
        Player.Instance.OnPlayerDeath.AddListener(HandlePlayerDeath);
    }

    private void OnDisable()
    {
        Player.Instance.OnPlayerDeath.RemoveListener(HandlePlayerDeath);
    }

    public void AddKey(KeyObject newKey)
    {
        if(_keys.Contains(newKey) == false)
        {
            newKey.SpringJoint.enabled = true;

            if (_keys.Count == 0)
            {
                newKey.SpringJoint.connectedBody = Player.Instance.FollowPoint;
            }
            else
            {
                newKey.SpringJoint.connectedBody = _keys.Peek().FollowRigidbody;
            }

            _keys.Push(newKey);
        }
    }

    public KeyObject GetKey()
    {
        if(_keys.Count > 0)
            return _keys.Pop();
        else
            return null;
    }

    public void HandlePlayerDeath()
    {
        _keys.Clear();
    }
}
