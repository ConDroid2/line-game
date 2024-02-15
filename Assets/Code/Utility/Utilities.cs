using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static IntersectionPoint FindIntersectionPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // NOTE: IF ANY INTERSECTION PROBLEMS, CHECK HERE FIRST, IT MIGHT BE BECAUSE OF ROUNDING TO 2 DECIMAL PLACES
        decimal ax = System.Math.Round((decimal)a.x, 2);
        decimal ay = System.Math.Round((decimal)a.y, 2);

        decimal bx = System.Math.Round((decimal)b.x, 2);
        decimal by = System.Math.Round((decimal)b.y, 2);

        decimal cx = System.Math.Round((decimal)c.x, 2);
        decimal cy = System.Math.Round((decimal)c.y, 2);

        decimal dx = System.Math.Round((decimal)d.x, 2);
        decimal dy = System.Math.Round((decimal)d.y, 2);

        decimal a1 = by - ay;
        decimal b1 = ax - bx;
        decimal c1 = (a1 * ax) + (b1 * ay);

        decimal a2 = dy - cy;
        decimal b2 = cx - dx;
        decimal c2 = (a2 * cx) + (b2 * cy);

        decimal determinant = a1 * b2 - a2 * b1;

        // If determinant is 0, lines are parallel, need to check if also touching
        if (determinant == 0)
        {
            // If any point on line ab is the same as a point on cd, then that is an intersection, otherwise they are not touching
            float tolerance = 0.005f;
            Vector3 point = Vector3.zero;

            float lineLength = Vector3.Distance(c, d);
            bool isAOnLine = Vector3.Distance(a, c) + Vector3.Distance(a, d) == lineLength;
            bool isBOnLine = Vector3.Distance(b, c) + Vector3.Distance(b, d) == lineLength;

            //Debug.Log($"Is A on line: {isAOnLine}, is B on line: {isBOnLine}");
            //Debug.Log($"{lineLength} vs {Vector3.Distance(a, c) + Vector3.Distance(a, d)}");
            //Debug.Log($"{lineLength} vs {Vector3.Distance(b, c) + Vector3.Distance(b, d)}");


            // Taking advantage of the fact that all lines go A -> B, meaning we only want this if D and A are the same or C and B are the same
            if (Vector3.Distance(a, d) < tolerance && Vector3.Distance(b, c) > tolerance) point = a;
            else if (Vector3.Distance(b, c) < tolerance && Vector3.Distance(a, d) > tolerance) point = b;
            else if (isAOnLine || isBOnLine) { 
                point = Vector3.negativeInfinity; 
            }
            else point = Vector3.positiveInfinity;

            //Debug.Log(point.x);

            return new IntersectionPoint(point, true);
        }
        

        decimal x = (b2 * c1 - b1 * c2) / determinant;
        decimal y = (a1 * c2 - a2 * c1) / determinant;

        // Line 1 mins/maxes
        decimal line1MaxX = System.Math.Max(ax, bx);
        decimal line1MinX = System.Math.Min(ax, bx);
        decimal line1MaxY = System.Math.Max(ay, by);
        decimal line1MinY = System.Math.Min(ay, by);

        // Line 2 mins/maxes
        decimal line2MaxX = System.Math.Max(cx, dx);
        decimal line2MinX = System.Math.Min(cx, dx);
        decimal line2MaxY = System.Math.Max(cy, dy);
        decimal line2MinY = System.Math.Min(cy, dy);

        // Check if point is on both lines
        bool pointIsOnLine1 = x <= line1MaxX && x >= line1MinX && y <= line1MaxY && y >= line1MinY;
        bool pointIsOnLine2 = x <= line2MaxX && x >= line2MinX && y <= line2MaxY && y >= line2MinY;


        if (pointIsOnLine1 && pointIsOnLine2)
        {
            Vector3 point = new Vector3((float)x, (float)y);
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

        public override string ToString()
        {
            return $"Point: {Point}, IsParallel: {IsParallel}";
        }
    }
}
