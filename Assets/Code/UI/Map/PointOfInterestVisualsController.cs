using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestVisualsController : MonoBehaviour
{
    [SerializeField] private RectTransform _pointOfInterestParent;

    [SerializeField] private List<RectTransform> _pointsOfInterest;

    private const float HEIGHT = 50;

    public void AddPOI(RectTransform poi)
    {
        _pointsOfInterest.Add(poi);
    }

    public void CalculateAndSetPositions()
    {
        int numOfRows = _pointsOfInterest.Count >= 4 ? 2 : 1;
        int numInEachRow = _pointsOfInterest.Count / numOfRows;

        float width = SumPOIWidth() / numOfRows;

        float poiXPos = -width / 2;
        float poiYPos = HEIGHT + (-1 * (HEIGHT / numOfRows));

        for(int i = 0; i < _pointsOfInterest.Count; i++)
        {
            RectTransform poiRect = _pointsOfInterest[i];
            poiRect.localPosition = new Vector3(poiXPos, poiYPos, 0f);

            poiXPos += poiRect.sizeDelta.x;

            if(i + 1 == numInEachRow)
            {
                poiXPos = -width / 2;
                poiYPos -= HEIGHT;
            }
        }
    }

    private float SumPOIWidth()
    {
        float sum = 0;

        foreach(RectTransform rect in _pointsOfInterest)
        {
            Debug.Log(rect.sizeDelta.x);

            sum += rect.sizeDelta.x;
        }

        return sum;
    }
}
