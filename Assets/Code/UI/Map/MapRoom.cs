using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRoom : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform BackgroundVisualsTransform;
    [SerializeField] private Image _basicRoomImage;
    [SerializeField] private Image _rightConnection;
    [SerializeField] private Image _leftConnection;
    [SerializeField] private Image _bottomConnection;
    [SerializeField] private Image _topConnection;


    public void SetColor(Color color)
    {
        _basicRoomImage.color = color;
        _rightConnection.color = color;
        _leftConnection.color = color;
        _bottomConnection.color = color;
        _topConnection.color = color;
    }

    public void SetConnectionImages(bool hasRightConnection, bool hasLeftConnection, bool hasBottomConnection, bool hasTopConnection)
    {
        _rightConnection.gameObject.SetActive(hasRightConnection);
        _leftConnection.gameObject.SetActive(hasLeftConnection);
        _bottomConnection.gameObject.SetActive(hasBottomConnection);
        _topConnection.gameObject.SetActive(hasTopConnection);
    }
}
