using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargeEnemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _totalChargeTime;
    [SerializeField] private AnimationCurve _chargeCurve;

    [SerializeField] private bool _activated = false;
    private bool _charging = false;

    private float _timeCharging = 0;

    private Vector3 _chargeStartingPosition;
    private Vector3 _chargeEndingPosition;

    [Header("Events")]
    public UnityEvent DoneCharging;

    private void Update()
    {
        if (_charging)
        {
            _timeCharging += Time.deltaTime;

            float percentageDone = _chargeCurve.Evaluate(_timeCharging / _totalChargeTime);
            // Debug.Log(percentageDone);

            transform.position = Vector3.LerpUnclamped(_chargeStartingPosition, _chargeEndingPosition, percentageDone);

            if(_timeCharging >= _totalChargeTime)
            {
                _charging = false;
                DoneCharging.Invoke();
            }
        }
    }


    public void Charge()
    {
        if(_activated == true && _charging == false)
        {
            _chargeStartingPosition = transform.position;
            _chargeEndingPosition = Player.Instance.transform.position;
            _timeCharging = 0f;
            _charging = true;
        }
    }

    public void HandlePlayerInRange(Collider2D collider)
    {
        Debug.Log($"Checking player {collider.tag}");
        if (collider.CompareTag("Player"))
        {
            if (_activated == false)
            {
                _activated = true;
                Charge();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_charging)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_chargeStartingPosition, _chargeEndingPosition);
        }
    }
}
