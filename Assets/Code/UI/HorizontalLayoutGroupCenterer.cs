using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLayoutGroupCenterer : MonoBehaviour
{

    [ContextMenu("Center Objects")]
    public void CenterObjects()
    {
        int childCount = transform.childCount;

        //if(childCount < 2)
        //{
        //    Debug.Log("Not enough children to center");
        //    return;
        //}

        float positionOffset = 0f;

        for(int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<RectTransform>();

            Debug.Log(child.sizeDelta.x);

            positionOffset += child.sizeDelta.x;
        }

        positionOffset /= 2;

        Debug.Log($"Offset is: -{positionOffset}");

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(-1 * positionOffset, rectTransform.anchoredPosition.y);
    }
}
