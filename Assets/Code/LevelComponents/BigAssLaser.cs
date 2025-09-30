using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BigAssLaser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _laserStartPoint;
    [SerializeField] private LineRenderer _laserLine;
    [SerializeField] private ParticleSystem _hitParticles;

    [Header("Settings")]
    [SerializeField] private float _range;
    [SerializeField] private float _width;
    [SerializeField] private LayerMask _laserPhysicsMask;

    // Variables to keep track of things
    private Vector3 _maxDistance;
    private Switch _affectedSwitch = null;

    [SerializeField] private bool _active = true;

    private bool _hittingSomething = false;

    // Events
    public UnityEvent IsColliding;
    public UnityEvent NotColliding;

    public UnityEvent stopLaserSfx;
    public UnityEvent reactivateLaserSfx;

    private void Start()
    {
        _laserLine.startWidth = _width;
        _laserLine.endWidth = _width;

        _maxDistance = _laserStartPoint.position + (transform.up * _range);

        _laserLine.SetPosition(0, _laserStartPoint.position);
        _laserLine.SetPosition(1, _maxDistance);

        Activate(_active);

        NotColliding?.Invoke();
    }

    private void FixedUpdate()
    {

        if (_active == false) return;


        // Adjust laser in case it's moving
        // Debug.Log(transform.up);
        _maxDistance = _laserStartPoint.position + (transform.up * _range);
        _laserLine.SetPosition(0, _laserStartPoint.position);
        // Both starting position and ending position should be modified by width
        RaycastHit2D hit = Physics2D.BoxCast(_laserStartPoint.position, new Vector2(_width, _width), transform.rotation.z, transform.up, _range, layerMask: _laserPhysicsMask);

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

            _hitParticles.transform.position = newEnd;

            

           //  _laserEndVisual.SetActive(true);

            if(_hittingSomething == false)
            {
                _hittingSomething = true;
                IsColliding?.Invoke();
                _hitParticles.Play();
            }
        }
        else
        {
            // Draw laser to hit point
            _laserLine.SetPosition(1, _maxDistance);

            // _laserEndVisual.SetActive(false);
            _hitParticles.Stop();

            // Deactivate any switches
            if (_affectedSwitch != null)
            {
                _affectedSwitch.Deactivate();
                _affectedSwitch = null;
            }

            if(_hittingSomething == true)
            {
                _hittingSomething = false;
                NotColliding?.Invoke();
            }
        }
    }

    public void Activate(bool activate)
    {
        bool previousActiveState = _active;
        _active = activate;

        _laserLine.enabled = _active;

        if (previousActiveState && !_active) // if was active, and is being set to inactive, turn of sfx
        {
            _hitParticles.Stop(); // This has to be called before the command set _hittingSomething to false. It works, but I don't know why JPP 2025/09/29
            this.stopLaserSfx?.Invoke();
            this.NotColliding?.Invoke();
            this._hittingSomething = false;
            // Looking at this code, I think that calling activate will end up bypassing the calls to turn off any switches activated by the laser.
            // I think this is fine, since we didn't end up ever activating switches with a laser ever. JPP 2025/09/29
        }
        else if(!previousActiveState && _active) // if was not currently active, but is becoming active, turn it on
        {
            this.reactivateLaserSfx?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 lineEnd = _laserStartPoint.position + (transform.up * _range);

        Gizmos.DrawLine(_laserStartPoint.position, lineEnd);

    }
}
