using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelTransitionController : OnDeathEffect
{
    [Header("Settings")]
    [SerializeField] private float _transitionLength;
    [SerializeField] private AnimationCurve _transitionCurve;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _maskTransform;
    [SerializeField] private Transform _transitionSprite;

    // Member Variables
    private float _timeTransitioning = 0f;
    private bool _isPlaying = false;
    private bool _isGrowing = false;
    private float _startValue;
    private float _endValue;

    private void Awake()
    {
        if(_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_isPlaying)
        {
            _timeTransitioning += Time.deltaTime;

            float percentage = _timeTransitioning / _transitionLength;

            float lerpValue = Mathf.Lerp(_startValue, _endValue, _transitionCurve.Evaluate(percentage));

            _maskTransform.localScale = new Vector3(lerpValue, lerpValue, 1);

            if(_timeTransitioning >= _transitionLength)
            {
                _isPlaying = false;
                if(_isGrowing) _transitionSprite.gameObject.SetActive(false);
            }
        }
    }

    public void InitializeEffect()
    {
        Vector3 playerPosition = Player.Instance.transform.position;
        Vector3 cameraPosition = _cameraTransform.position;

        _transitionSprite.position = new Vector3(cameraPosition.x, cameraPosition.y);
        _maskTransform.position = playerPosition;
    }

    public void InitializeEffectSize(int widthScale, int heightScale)
    {
        _transitionSprite.localScale = new Vector2((widthScale * 20) + 1, (heightScale * 11) +1);
    }

    public override void TriggerEffect()
    {
        TriggerEffectWithOption(false);
    }

    public void TriggerEffectWithOption(bool growing)
    {
        _isGrowing = growing;
        

        Vector3 playerPosition = Player.Instance.transform.position;
        Vector3 cameraPosition = _cameraTransform.position;

        // _transitionSprite.position = new Vector3(cameraPosition.x, cameraPosition.y);
        _maskTransform.position = playerPosition;

        Vector3 topLeftCorner = new Vector3(cameraPosition.x - 10, cameraPosition.y + 5.5f);
        Vector3 topRightCorner = new Vector3(cameraPosition.x + 10, cameraPosition.y + 5.5f);
        Vector3 bottomRightCorner = new Vector3(cameraPosition.x + 10, cameraPosition.y - 5.5f);
        Vector3 bottomLeftCorner = new Vector3(cameraPosition.x - 10, cameraPosition.y - 5.5f);

        float[] cornerDistances =
        {
            Vector3.Distance(playerPosition, topLeftCorner),
            Vector3.Distance(playerPosition, topRightCorner),
            Vector3.Distance(playerPosition, bottomRightCorner),
            Vector3.Distance(playerPosition, bottomLeftCorner)
        };

        float greatestDistance = cornerDistances.Max();

        if (_isGrowing)
        {
            _startValue = 0f;
            _endValue = greatestDistance * 2;
        }
        else
        {
            _startValue = greatestDistance * 2;
            _endValue = 0f;
        }
        
        _timeTransitioning = 0f;
        _transitionSprite.gameObject.SetActive(true);
        _isPlaying = true;
    }

    public override bool IsPlaying()
    {
        return _isPlaying;
    }
}
