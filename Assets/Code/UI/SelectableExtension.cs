using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectableExtension : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    public void OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnDeselected?.Invoke();
    }
}
