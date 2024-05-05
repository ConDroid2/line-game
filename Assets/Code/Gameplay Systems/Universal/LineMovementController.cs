using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovementController : MonoBehaviour
{
    // External References
    public OnLineController OnLineController;
    public LevelManager LevelManager { get; private set; }

    // Settings
    public float CheckForIntersectionsDistance; // The distance we check for intersections
    [SerializeField] private bool _canPush;
    [SerializeField] private float _lineSwapAngleTolerance = 45f;
    [SerializeField] private float _edgeOfLineTolerance = 0.000005f;

    // Force Variabls
    private List<Force> _activeForces = new List<Force>();
    private bool _ignoreForces = false;


    // Collision Variabls
    private Collider2D _collider;
    private LineMovementController _objectBeingPushed = null;

    // Stuff to keep track of
    private LineSwapData _previousSwapData = null;

    // Utilities
    private DebugLogger _logger;

    // Events
    public System.Action<Vector3> OnReachedEdgeOfLine;
    public System.Action<int> OnTryToMoveInDirection;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _logger = GetComponent<DebugLogger>();
    }

    private void FixedUpdate()
    {
        if(_activeForces.Count != 0 && _ignoreForces == false)
        {
            MoveFromActiveForces();
        }
    }



    /** GENERAL MOVE STUFF **/

    // Actually LERP along a line based on passed in distance to try to move. Return whether we actually moved
    private bool Move(float distanceToMove)
    {
        // Keep track of if we are actually going to move
        bool willMove = true;

        // Calculate the new distance on line
        float newDistanceOnLine = Mathf.Clamp01(OnLineController.DistanceOnLine + distanceToMove);
        Vector3 attemptedNewPosition = OnLineController.CheckNewPosition(newDistanceOnLine);

        // Check if our new position is actually new
        willMove &= transform.position != attemptedNewPosition;

        // The vector we're actually moving along
        Vector3 movementVector = (attemptedNewPosition - transform.position).normalized;

        // Do an OverlapBox to check if we'll hit anything at our new position
        Collider2D[] results = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D(); // Can put this in a variable eventually

        int numberOfHits = Physics2D.OverlapBox(attemptedNewPosition, _collider.bounds.size, transform.eulerAngles.z, filter, results);

        // Check if any collisions will stop us
        willMove &= HandleCollisions(results);

        // If we can push, deal with any potential pushables
        if (_canPush)
        {
            willMove &= HandlePushables(results, movementVector, distanceToMove);
        }

        // If we meet all criteria, set our new position
        if (willMove)
        {
            OnLineController.DistanceOnLine = newDistanceOnLine;
            // Debug.Log($"Setting distance to: {newDistanceOnLine}. Actual: {OnLineController.DistanceOnLine}");

            float tolerance = 0.000005f;

            if (OnLineController.DistanceOnLine - _edgeOfLineTolerance <= 0 || OnLineController.DistanceOnLine + _edgeOfLineTolerance >= 1)
            {
                OnReachedEdgeOfLine?.Invoke(transform.position);
            }
        }

       //  _logger.DebugLog($"{gameObject.name} returning {willMove}");
        return willMove;
    }


    // Attempt to move based on direct input
    public bool MoveDirectly(Vector2 direction, float moveAmount)
    {
        
        LineController currentLine = OnLineController.CurrentLine;

        HandleSwappingLines(direction, currentLine);

        // Figure out which direction along the line we're trying to move based on angles
        float angleBetweenDirectionAndSlope = Vector3.Angle(direction, currentLine.Slope);
        float distanceToMove = 0f;

        if (angleBetweenDirectionAndSlope < 90f)
        {
            distanceToMove = moveAmount;
            OnTryToMoveInDirection?.Invoke(1);
        }
        else if (angleBetweenDirectionAndSlope > 90f)
        {
            distanceToMove = moveAmount * -1;
            OnTryToMoveInDirection?.Invoke(-1);
        }

        return Move(distanceToMove);
    }


    //Check if we should be moving to a new line based on input, then move to it
    private void HandleSwappingLines(Vector2 inputVector, LineController currentLine)
    {
        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(currentLine, transform.position, CheckForIntersectionsDistance);

        // if (intersections.Count == 0) return;


        //float inputAngleAgainstLine = Vector3.Angle(inputVector, currentLine.Slope);
        //bool goingPositive = inputAngleAgainstLine < 90f;
        //bool goingNegative = inputAngleAgainstLine > 90f;


        float lowestAngle = Vector3.Angle(inputVector.normalized, currentLine.Slope);
        if(lowestAngle > Vector3.Angle(inputVector.normalized, currentLine.Slope * -1))
        {
            lowestAngle = Vector3.Angle(inputVector.normalized, currentLine.Slope * -1);
        }

        // If we're at the edge of a line, we probably don't want to stay on it
        if (OnLineController.DistanceOnLine - _edgeOfLineTolerance <= 0 || OnLineController.DistanceOnLine + _edgeOfLineTolerance >= 1)
        {
            Debug.Log("We're at the edge, setting lowest angle to 90");
            lowestAngle = 90f;
        }
        float newDistanceAlongLine = 0f;
        LineController lineToMoveTo = currentLine;

        Debug.Log($"Lowest Angle {lowestAngle}");

        foreach (IntersectionData intersection in intersections)
        {
            // Check if the input would allow movment along the intersecting line
            float inputAngleAgainstLine = Vector3.Angle(inputVector, intersection.Line.Slope);
            bool goingPositive = inputAngleAgainstLine < 90f;
            bool goingNegative = inputAngleAgainstLine > 90f;

            // Skip intersection if the point is at the end of the line in the direction we are moving in (if we are moving right, don't put us on the right endpoint of a new line)
            if (goingPositive && intersection.DistanceAlongLine == 1 || goingNegative && intersection.DistanceAlongLine == 0)
            {
                Debug.Log($"Skipping {intersection.Line.name}, {inputAngleAgainstLine} + {goingPositive}/{goingNegative}");
                continue;
            }

            // Skip intersection if we have previous frame data and our input is the same and the intersection we're checking was part of our previous frame check
            if(_previousSwapData != null && intersection.IsParallel == false)
            {
                if(_previousSwapData.InputVector == inputVector && _previousSwapData.IntersectionPoints.Contains(intersection.IntersectionWorldSpace))
                {
                    Debug.Log($"Skipping {intersection.Line.name} due to previous frame data");
                    continue;
                }
            }

            float angleAgainstPositiveSlope = Vector3.Angle(inputVector.normalized, intersection.Line.Slope);
            float angleAgainstNegativeSlope = Vector3.Angle(inputVector.normalized, intersection.Line.Slope * -1);

            float angle = angleAgainstPositiveSlope < angleAgainstNegativeSlope ? angleAgainstPositiveSlope : angleAgainstNegativeSlope; 


            bool canMoveToNewLine = angleAgainstPositiveSlope <= _lineSwapAngleTolerance || angleAgainstNegativeSlope <= _lineSwapAngleTolerance;


            if (intersection.IsParallel)
            {
                canMoveToNewLine &= OnLineController.DistanceOnLine - _edgeOfLineTolerance <= 0 || OnLineController.DistanceOnLine + _edgeOfLineTolerance >= 1;
            }

            // Set new line using Intersection Data
            if (canMoveToNewLine && angle < lowestAngle)
            {
                Debug.Log($"LowestAngle is: {lowestAngle}. Angle for {intersection.Line.name} is: {angle}");
                lowestAngle = angle;
                lineToMoveTo = intersection.Line;
                newDistanceAlongLine = intersection.DistanceAlongLine;
            }

            else if (canMoveToNewLine && lineToMoveTo.Slope == currentLine.Slope && intersection.Line.Slope != currentLine.Slope && angle == lowestAngle)
            {
                lineToMoveTo = intersection.Line;
                newDistanceAlongLine = intersection.DistanceAlongLine;
            }
        }

        if(lineToMoveTo != currentLine)
        {
            Debug.Log($"Moving to new line: {lineToMoveTo.name}");
            SetNewLine(lineToMoveTo, newDistanceAlongLine);

            _previousSwapData = new LineSwapData(inputVector, intersections);
        }

        
    }



    /** COLLISION PARSERS/HANLDERS **/

    // Determin whether or not something we're hitting should be stopping us (and return if it is)
    private bool HandleCollisions(Collider2D[] colliders)
    {
        bool canMove = true;

        foreach(Collider2D collider in colliders)
        {
            // If there is no collider in this element, or it's ourself: skip this iteration
            if (collider == null || collider.gameObject == gameObject) continue;

            // _logger.DebugLog($"{gameObject.name} hit {collider.gameObject.name}");
            collider.gameObject.TryGetComponent(out LineMovementController pushable);
            collider.gameObject.TryGetComponent(out OnLineController onLineController);

            // Not sure if this check is required
            if (onLineController == null)
            {
                continue;
            }

            // If we can't push, but hit a pushable
            if (pushable != null && _canPush == false)
            {
                canMove = false;
            }

            // If we hit a universal obstacle
            if (onLineController.ObjectType == Enums.ObjectType.UniversalObstacle)
            {
                canMove = false;
            }
            // If we hit a BlockObstacle, and we're not the Player
            else if (onLineController.ObjectType == Enums.ObjectType.BlockObstacle && OnLineController.ObjectType != Enums.ObjectType.Player)
            {
                canMove = false; 
            }
        }

        // Clear out all forces if we hit something
        if(canMove == false)
        {
            _activeForces.Clear();
        }

        // _logger.DebugLog($"Collisions = {canMove}");
        return canMove;
    }


    // Push anything we should be pushing
    private bool HandlePushables(Collider2D[] colliders, Vector2 direction, float distanceToMove)
    {
        // _logger.DebugLog($"Pushing in {direction}");
        distanceToMove = Mathf.Abs(distanceToMove);
        // Go through each collision until we find one pushable
        foreach (Collider2D collider in colliders)
        {
            if (collider == null ||collider.gameObject == gameObject) continue;

            if (collider.CompareTag("Pushable"))
            {
                // Keep track that we're pushing this object and return whether or not we moved it
                collider.gameObject.TryGetComponent(out LineMovementController otherController);

                // If we're pushing a new thing for some reason, let the thing we were pushing know
                if(_objectBeingPushed != null && _objectBeingPushed != otherController)
                {
                    _objectBeingPushed.SetIgnoreForces(false);
                }

                _objectBeingPushed = otherController;

                otherController.SetIgnoreForces(true);

                // Try to move pushable
                _logger.DebugLog($"Would be pushing {collider.gameObject.name}");
                return otherController.MoveDirectly(direction, distanceToMove);
            }
        }

        // If we don't find any pushables, reset if needed, then we can move
        if(_objectBeingPushed != null)
        {
            _logger.DebugLog($"Stopped pushing {_objectBeingPushed.gameObject.name}");
            _objectBeingPushed.SetIgnoreForces(false);
            _objectBeingPushed = null;
        }

        return true;
    }

   

    /** FORCES STUFF **/

    // Add a new force to our active forces list
    public bool AddForce(Force newForce, bool validateForce = false)
    {
        // If the source of this force isn't already acting on this object
        if (_activeForces.Find(force => force.Source == newForce.Source) == null)
        {
            if(validateForce == true)
            {
                _logger.DebugLog($"Is new force valid? {ValidateForce(newForce)}");
                if (ValidateForce(newForce) == false) return false;
            }

            _logger.DebugLog($"Adding new force");
            _activeForces.Add(newForce);
            return true;
        }

        return false;
    }


    // Move this object based on the forces acting upon it
    public void MoveFromActiveForces()
    {
        LineController currentLine = OnLineController.CurrentLine;

        // Figure out what direction all forces combined are pushing
        Vector2 combinedDirection = Vector2.zero;

        foreach (Force force in _activeForces)
        {
            combinedDirection += (force.Direction * force.Magnitude);
        }

        combinedDirection.Normalize();

        _logger.DebugLog($"Combined direcetion: {combinedDirection}");

        if (combinedDirection == Vector2.zero)
        {
            OnLineController.DistanceOnLine = OnLineController.DistanceOnLine;
            return;
        }

        // Swap lines based on the combined direction
        HandleSwappingLines(combinedDirection, currentLine);

        float distanceToMove = 0f;

        foreach (Force force in _activeForces)
        {
            // Calculate how much this would move the player
            // Apply to newDistanceOnLine
            float angleBetweenForceAndSlope = Vector3.Angle(force.Direction, currentLine.Slope * currentLine.DirectionModifier);


            // Speed modified based on current line length
            float speed = force.Magnitude / OnLineController.CurrentLine.Length;
            float distanceDelta = speed * Time.deltaTime;

            if (angleBetweenForceAndSlope < 90f)
            {
                distanceToMove += distanceDelta;
                // These invokes should be after we know which way we're going
                OnTryToMoveInDirection?.Invoke(1);
            }
            else if (angleBetweenForceAndSlope > 90f)
            {
                distanceToMove -= distanceDelta;
                OnTryToMoveInDirection?.Invoke(-1);
            }

            combinedDirection += force.Direction;
        }

        bool moved = Move(distanceToMove);

        if (!moved)
        {
            // If we didn't move, get rid of all forces
            _activeForces.Clear();
        }

        CleanupForces();
    }


    // Apply drag to all forces, then get rid of any that meet conditions
    private void CleanupForces()
    {
        _logger.DebugLog("Removing Forces");
        _activeForces.ForEach(force => force.ApplyDrag());
        // Loop through each force, remove if condition is met
        _activeForces.RemoveAll(force => force.Magnitude < 0.01f);

        // Need to also remove forces that are trying to push the object in a way it can't move
        _activeForces.RemoveAll(force => ValidateForce(force) == false);

        _logger.DebugLog($"Number of forces after cleanup = {_activeForces.Count}");
    }


    // Return whether or not a force is valid (mainly check if force would try to push us past an endpoint)
    private bool ValidateForce(Force force)
    {
        float angleBetweenForceAndSlope = Vector2.Angle(force.Direction, OnLineController.CurrentLine.Slope);
        bool isValid = true;

        if (angleBetweenForceAndSlope < 90f)
        {
            isValid &= OnLineController.DistanceOnLine + _edgeOfLineTolerance < 1;
        }
        else if (angleBetweenForceAndSlope > 90f)
        {
            isValid &= OnLineController.DistanceOnLine - _edgeOfLineTolerance > 0;
        }

        return isValid;
    }

    

    /** SETTERS **/

    //Set the level manager
    public void SetLevelManager(LevelManager newLevelManager)
    {
        LevelManager = newLevelManager;
    }

    
    // Set whether or not we should ignore forces
    public void SetIgnoreForces(bool ignore)
    {
        if (ignore == true)
        {
            _activeForces.Clear();
        }

        _ignoreForces = ignore;
    }


    // Set new Line
    public void SetNewLine(LineController newLine, float distanceOnLine = 0)
    {
        OnLineController.SetLine(newLine, distanceOnLine);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, CheckForIntersectionsDistance);

    }

    // Utils

    public class LineSwapData
    {
        public Vector2 InputVector;
        public HashSet<Vector3> IntersectionPoints;

        public LineSwapData(Vector2 inputVector, List<IntersectionData> intersections)
        {
            InputVector = inputVector;
            IntersectionPoints = new HashSet<Vector3>();

            foreach(IntersectionData intersection in intersections)
            {
                IntersectionPoints.Add(intersection.IntersectionWorldSpace);
            }
        }
    }
}
