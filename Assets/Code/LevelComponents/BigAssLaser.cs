using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAssLaser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _laserStartPoint;
    [SerializeField] private LineRenderer _laserLine;
    [SerializeField] private GameObject _laserEndVisual;

    [Header("Settings")]
    [SerializeField] private float _range;
    [SerializeField] private float _width;

    // Variables to keep track of things
    private Vector3 _maxDistance;
    private Switch _affectedSwitch = null;


    private void Start()
    {
        _laserLine.startWidth = _width;
        _laserLine.endWidth = _width;

        _maxDistance = _laserStartPoint.position + (transform.up * _range);

        _laserEndVisual.transform.position = _maxDistance;
        _laserEndVisual.transform.up = transform.up;

        _laserLine.SetPosition(0, _laserStartPoint.position);
        _laserLine.SetPosition(1, _maxDistance);
    }

    private void FixedUpdate()
    {
        // Adjust laser in case it's moving
        // Debug.Log(transform.up);
        _maxDistance = _laserStartPoint.position + (transform.up * _range);
        _laserLine.SetPosition(0, _laserStartPoint.position);
        // Both starting position and ending position should be modified by width
        RaycastHit2D hit = Physics2D.BoxCast(_laserStartPoint.position, new Vector2(_width, _width), transform.rotation.z, transform.up, _range);

        if (hit)
        {
            // Kill Player
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit player");
                Player.Instance.GetKilled(Enums.KillType.Default);
            }

            // Activate Continous Switch hit
            if(hit.collider.TryGetComponent(out Switch affectedSwitch) && affectedSwitch.SwitchType == Enums.SwitchType.Continuous)
            {
                _affectedSwitch = affectedSwitch;
                _affectedSwitch.Activate();
            }
            // If no switch hit, deactivate switch it was affecting
            else if(_affectedSwitch != null)
            {
                _affectedSwitch.Deactivate();
                _affectedSwitch = null;
            }

            // Draw laser to hit point
            Vector3 projection = Vector3.Project(new Vector3(hit.point.x - _laserStartPoint.position.x, hit.point.y - _laserStartPoint.position.y), _laserStartPoint.position + transform.up - _laserStartPoint.position);
            Vector3 newEnd = _laserStartPoint.position + projection;

            _laserLine.SetPosition(1, newEnd);
            _laserEndVisual.transform.position = newEnd;

            _laserEndVisual.SetActive(true);
        }
        else
        {
            // Draw laser to hit point
            _laserLine.SetPosition(1, _maxDistance);
            _laserEndVisual.transform.position = _maxDistance;

            _laserEndVisual.SetActive(false);

            // Deactivate any switches
            if (_affectedSwitch != null)
            {
                _affectedSwitch.Deactivate();
                _affectedSwitch = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 lineEnd = _laserStartPoint.position + (transform.up * _range);

        Gizmos.DrawLine(_laserStartPoint.position, lineEnd);

    }
}
