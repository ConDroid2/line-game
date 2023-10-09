using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineMovementController))]
public class ObjectOnLine : MonoBehaviour
{
    public LineMovementController MovementController;

    private void Awake()
    {
        MovementController = GetComponent<LineMovementController>();
    }
}
