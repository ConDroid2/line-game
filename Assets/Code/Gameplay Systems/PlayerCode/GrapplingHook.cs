using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LineRenderer _previewLine;
    [SerializeField] private OnLineController _playerPreview;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private LayerMask _collisionMask;

    [SerializeField] private float _grappleDistance;
    [SerializeField] private float _grappleShootTime;
    [SerializeField] private AnimationCurve _grappleShootCurve;
    [SerializeField] private float _grapplePullTime;
    [SerializeField] private AnimationCurve _grapplePullCurve;

    private bool _performGrapple = false;

    // Data while performing a grapple
    private IntersectionData _moveTo = null;
    private Vector3 _startPosition;
    private bool _drawingLine = false;
    private float _timeDrawingLine = 0f;
    private bool _movingPlayer = false;
    private float _timeMovingPlayer = 0f;

    // Events
    public System.Action OnGrappleFinished;

    public void AttemptGrapple()
    {
        if (_performGrapple == false && _moveTo != null && _moveTo.Line != null)
        {

            _previewLine.SetPosition(0, transform.position);
            _previewLine.SetPosition(1, transform.position);

            _startPosition = Player.Instance.transform.position;
            _performGrapple = true;
            //_drawingLine = true;
            _movingPlayer = true;

            _playerPreview.SetLine(null);
            _playerPreview.gameObject.SetActive(false);

            if (LevelManager.Instance != null)
            {
                //LevelManager.Instance.ObjectMovementTimeScale = 0f;
            }
        }
        else if(_performGrapple == false && (_moveTo == null || _moveTo.Line == null))
        {
            FinishGrapple();
        }
    }

    private void Update()
    {
        if (_performGrapple == false) return;

        


        if (_movingPlayer)
        {
            

            // In the strange case that you move to a new level mid grapple (from conveyor lines usually), need to end grapple
            if (_moveTo.Line == null)
            {
                FinishGrapple();
            }

            _lineRenderer.SetPosition(1, _moveTo.IntersectionWorldSpace);

            _timeMovingPlayer += Time.deltaTime;

            float journeyRatio = _timeMovingPlayer / _grapplePullTime;

            Player.Instance.transform.position = Vector3.Lerp(_startPosition, _moveTo.IntersectionWorldSpace, _grapplePullCurve.Evaluate(journeyRatio));
            _lineRenderer.SetPosition(0, _startPosition);

            if(_timeMovingPlayer >= _grapplePullTime)
            {

                Player.Instance.SetNewLine(_moveTo.Line, _moveTo.DistanceAlongLine);

                FinishGrapple();
            }
        }
        // move player
    }

    public void CheckAndHandleCollisions()
    {
        // Shoot out a box cast to the _moveTo endpoint
        // If we hit something, _moveTo should be a new 
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, _playerCollider.bounds.size, transform.parent.eulerAngles.z, transform.up, (_moveTo.IntersectionWorldSpace - transform.position).magnitude, _collisionMask);
        if (hit.collider != null)
        {
            _moveTo = new IntersectionData(null, 0f, hit.point, false);
        }
        
    }

    public void FinishGrapple()
    {
        _moveTo = null;
        _timeMovingPlayer = 0f;
        _movingPlayer = false;
        _performGrapple = false;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);
        

        if (LevelManager.Instance != null)
        {
            // LevelManager.Instance.ObjectMovementTimeScale = 1f;
        }

        OnGrappleFinished?.Invoke();
    }

    public void SetPreview()
    {
        if (_performGrapple == true) return;

        Vector3 lineStart = transform.position;
        Vector3 lineEnd = transform.position + (transform.up.normalized * _grappleDistance);

        _moveTo = Player.Instance.MovementController.LevelManager.FindClosestIntersectionFromLine(lineStart, lineEnd);

        if(_moveTo != null)
        {
            CheckAndHandleCollisions();
            lineEnd = _moveTo.IntersectionWorldSpace;

            if (_moveTo.Line != null)
            {
                _playerPreview.gameObject.SetActive(true);
                _playerPreview.SetLine(_moveTo.Line, _moveTo.DistanceAlongLine);
            }
            else
            {
                _playerPreview.SetLine(null);
                _playerPreview.gameObject.SetActive(false);
            }
        }
        else
        {
            _playerPreview.SetLine(null);
            _playerPreview.gameObject.SetActive(false);
        }

        

        _previewLine.SetPosition(0, lineStart);
        _previewLine.SetPosition(1, lineEnd);
    }

    private IEnumerator WaitForAMoment()
    {
        yield return new WaitForSeconds(0.5f);

        FinishGrapple();
    }
}
