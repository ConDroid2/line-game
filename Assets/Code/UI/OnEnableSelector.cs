using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnEnableSelector : MonoBehaviour
{
    [SerializeField] private Selectable _selectOnEnable;

    private void OnEnable()
    {
        _selectOnEnable.Select();
    }
}
