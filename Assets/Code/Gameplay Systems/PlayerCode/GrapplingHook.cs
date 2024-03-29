using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
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
        if (_performGrapple == false)
        {
            Vector3 lineStart = transform.position;
            Vector3 lineEnd = transform.position + (transform.up.normalized * _grappleDistance);

            // Make level manager static
            _moveTo = Player.Instance.MovementController.LevelManager.FindClosestIntersectionFromLine(lineStart, lineEnd);

            if(_moveTo == null)
            {
                // Didn't fine a line
                Debug.Log("Did not find a line");
                _moveTo = new IntersectionData(null, 0f, lineEnd, false);
            }

            // Check collisions
            CheckAndHandleCollisions();

            _startPosition = Player.Instance.transform.position;
            _performGrapple = true;
            _drawingLine = true;
        }
    }

    private void Update()
    {
        if (_performGrapple == false) return;

        


        // draw line
        if(_drawingLine)
        {
            _timeDrawingLine += Time.deltaTime;
            float journeyRatio = _timeDrawingLine / _grappleShootTime;

            // draw line based on lerp between transform.position and intersection world position
            Vector3 grapplePosition = Vector3.Lerp(_startPosition, _moveTo.IntersectionWorldSpace, _grappleShootCurve.Evaluate(journeyRatio));
            _lineRenderer.SetPosition(0, _startPosition);
            _lineRenderer.SetPosition(1, grapplePosition);

            if(_timeDrawingLine >= _grappleShootTime)
            {
                _timeDrawingLine = 0f;
                _drawingLine = false;

                if(_moveTo.Line == null)
                {
                    StartCoroutine(WaitForAMoment());
                }
                else
                {
                    _movingPlayer = true;
                }   
            }
        }

        if (_movingPlayer)
        {
            // In the strange case that you move to a new level mid grapple (from conveyor lines usually), need to end grapple
            if (_moveTo.Line == null)
            {
                FinishGrapple();
            }

            _timeMovingPlayer += Time.deltaTime;

            float journeyRatio = _timeMovingPlayer / _grapplePullTime;

            Player.Instance.transform.position = Vector3.Lerp(_startPosition, _moveTo.IntersectionWorldSpace, _grapplePullCurve.Evaluate(journeyRatio));
            _lineRenderer.SetPosition(0, Player.Instance.transform.position);

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
        OnGrappleFinished?.Invoke();
    }

    private IEnumerator WaitForAMoment()
    {
        yield return new WaitForSeconds(0.5f);

        FinishGrapple();
    }
}
