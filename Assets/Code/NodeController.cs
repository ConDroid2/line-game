using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] private NodeController _connectedNode;

    public NodeController ConnectedNode => _connectedNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetSlope()
    {
        float rise = _connectedNode.transform.position.y - transform.position.y;
        float run = _connectedNode.transform.position.x - transform.position.x;

        return new Vector2(run, rise).normalized;
    }

    public float GetDistanceBetweenNodes()
    {
        return Vector3.Distance(transform.position, _connectedNode.transform.position);
    }
}
