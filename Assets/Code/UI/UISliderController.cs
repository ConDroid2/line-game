using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISliderController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Slider _slider;
    public Image _codeBody;

    // Tracking data
    private float _previousValue = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        _codeBody.gameObject.SetActive(false);

        _previousValue = _slider.value;
    }

    public void HandleValueChange(float newValue)
    {
        if(newValue < _previousValue)
        {
            _codeBody.rectTransform.anchoredPosition = new Vector2(60f, 0f);
            _codeBody.rectTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _codeBody.rectTransform.anchoredPosition = new Vector2(-60f, 0f);
            _codeBody.rectTransform.localScale = new Vector3(-1, 1, 1);
        }

        _previousValue = newValue;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _codeBody.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _codeBody.gameObject.SetActive(true);
    }

    
}
