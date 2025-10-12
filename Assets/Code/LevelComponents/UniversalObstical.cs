using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalObstical : MonoBehaviour
{
    [SerializeField] private GameObject _activeVisuals;
    [SerializeField] private GameObject _inactiveVisualsTop;
    [SerializeField] private GameObject _inactiveBottom;
    [SerializeField] private BoxCollider2D _collider;

    public void DeactivateFromTop()
    {
        _activeVisuals.SetActive(false);
        _collider.enabled = false;

        _inactiveVisualsTop.SetActive(true);
    }

    public void DeactivateFromBottom()
    {
        _activeVisuals.SetActive(false);
        _collider.enabled = false;

        _inactiveBottom.SetActive(true);
    }

    public void DeactivateFromTopAndBottom()
    {
        _activeVisuals.SetActive(false);
        _collider.enabled = false;

        _inactiveVisualsTop.SetActive(true);
        _inactiveBottom.SetActive(true);
    }

    public void Activate()
    {
        _activeVisuals.SetActive(true);
        _inactiveVisualsTop.SetActive(false);
        _inactiveBottom.SetActive(false);
        _collider.enabled = true;
    }
}
