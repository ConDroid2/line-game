using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRoom : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform BackgroundVisualsTransform;
    public RectTransform PointOfInterestParent;
    [SerializeField] private Image _basicRoomImage;
    [SerializeField] private List<Image> _roomConnectionImages;


    [SerializeField] private Enums.RoomArea _roomArea;
    private Color _defaultColor;


    [Header("Color Options")]
    [SerializeField] private Color _hubColor;
    [SerializeField] private Color _w1Color;
    [SerializeField] private Color _w2Color;
    [SerializeField] private Color _w3Color;
    [SerializeField] private Color _depthsColor;
    [SerializeField] private Color _w4Color;


    private void Awake()
    {
        switch (_roomArea)
        {
            case Enums.RoomArea.Hub:
                _defaultColor = _hubColor;
                break;
            case Enums.RoomArea.W1:
                _defaultColor = _w1Color;
                break;
            case Enums.RoomArea.W2:
                _defaultColor = _w2Color;
                break;
            case Enums.RoomArea.W3:
                _defaultColor = _w3Color;
                break;
            case Enums.RoomArea.Depths:
                _defaultColor = _depthsColor;
                break;
            case Enums.RoomArea.W4:
                _defaultColor = _w4Color;
                break;
            default:
                _defaultColor = _hubColor;
                break;
        }

        Debug.Log(_defaultColor);
    }

    public void SetToDefaultColor()
    {
        SetColor(_defaultColor);
    }

    public void SetColor(Color color)
    {
        _basicRoomImage.color = color;
        foreach(Image image in _roomConnectionImages)
        {
            image.color = color;
        }
    }

    public void AddRoomConnectionImages(Image image)
    {
        _roomConnectionImages.Add(image);
    }
}
