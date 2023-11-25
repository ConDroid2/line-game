using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static IntersectionPoint FindIntersectionPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        float a1 = b.y - a.y;
        float b1 = a.x - b.x;
        float c1 = (a1 * a.x) + (b1 * a.y);

        float a2 = d.y - c.y;
        float b2 = c.x - d.x;
        float c2 = (a2 * c.x) + (b2 * c.y);

        float determinant = a1 * b2 - a2 * b1;

        // If determinant is 0, lines are parallel, need to check if also touching
        if (determinant == 0)
        {
            // If any point on line ab is the same as a point on cd, then that is an intersection, otherwise they are not touching
            float tolerance = 0.005f;
            Vector3 point = Vector3.zero;

            if (Vector3.Distance(c, a) < tolerance) point = a;
            else if (Vector3.Distance(d, a) < tolerance) point = a;
            else if (Vector3.Distance(c, b) < tolerance) point = b;
            else if (Vector3.Distance(d, b) < tolerance) point = b;
            else point = Vector3.positiveInfinity;

            return new IntersectionPoint(point, true);
        }
        

        float x = (b2 * c1 - b1 * c2) / determinant;
        float y = (a1 * c2 - a2 * c1) / determinant;

        // Line 1 mins/maxes
        float line1MaxX = Mathf.Max(a.x, b.x);
        float line1MinX = Mathf.Min(a.x, b.x);
        float line1MaxY = Mathf.Max(a.y, b.y);
        float line1MinY = Mathf.Min(a.y, b.y);

        // Line 2 mins/maxes
        float line2MaxX = Mathf.Max(c.x, d.x);
        float line2MinX = Mathf.Min(c.x, d.x);
        float line2MaxY = Mathf.Max(c.y, d.y);
        float line2MinY = Mathf.Min(c.y, d.y);

        // Check if point is on both lines
        bool pointIsOnLine1 = x <= line1MaxX && x >= line1MinX && y <= line1MaxY && y >= line1MinY;
        bool pointIsOnLine2 = x <= line2MaxX && x >= line2MinX && y <= line2MaxY && y >= line2MinY;


        if (pointIsOnLine1 && pointIsOnLine2)
        {
            Vector3 point = new Vector3(x, y);
            return new IntersectionPoint(point, false);
        }
        else
        {
            return new IntersectionPoint(Vector3.positiveInfinity, false);
        }
    }

    public static Enums.SlopeType DetermineSlopeType(float x, float y)
    {
        Enums.SlopeType type = Enums.SlopeType.Horizontal;

        if (x == 0 && y != 0)
        {
            type = Enums.SlopeType.Vertical;
        }
        else if ((x > 0 && y > 0) || (x < 0 && y < 0))
        {
            type = Enums.SlopeType.Ascending;
        }
        else if ((x < 0 && y > 0) || (x > 0 && y < 0))
        {
            type = Enums.SlopeType.Descending;
        }
        else if(x == 0 && y == 0)
        {
            type = Enums.SlopeType.None;
        }

        return type;
    }

    public static void SwapVector3(ref Vector3 a, ref Vector3 b)
    {
        Vector3 temp = a;

        a = b;
        b = temp;
    }

    public class IntersectionPoint
    {
        public Vector3 Point;
        public bool IsParallel;

        public IntersectionPoint(Vector3 newPoint, bool isParallel)
        {
            Point = newPoint;
            IsParallel = isParallel;
        }
    }
}
