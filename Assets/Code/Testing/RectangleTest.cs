using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTest : MonoBehaviour
{
    Rectangle area;

    Vector2 mousePos;
    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        area = new Rectangle(new Vector2(1, 1), new Vector2(5, 1), new Vector2(5, 3), new Vector2(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnDrawGizmos()
    {
        if(area != null)
        {
            if (area.PointInRectangle(gameObject.transform.position))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawLine(area.PointA, area.PointB);
            Gizmos.DrawLine(area.PointB, area.PointC);
            Gizmos.DrawLine(area.PointC, area.PointD);
            Gizmos.DrawLine(area.PointD, area.PointA);
        }
    }


}
