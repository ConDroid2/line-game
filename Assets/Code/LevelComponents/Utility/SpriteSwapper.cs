using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private int _indexToUseOnAwake = 0;
    [SerializeField] private List<Sprite> _sprites;

    private void Awake()
    {
        if(_sprites.Count == 0)
        {
            Debug.LogError("No sprites in Sprite Swapper");
        }
        else if(_indexToUseOnAwake >= _sprites.Count)
        {
            Debug.LogError("Sprite Swapper indexToUseOnAwake is out of bounds");
        }
        else
        {
            _renderer.sprite = _sprites[_indexToUseOnAwake];
        }
    }

    public void SwapSprite(int newSpriteIndex)
    {
        if(newSpriteIndex >= _sprites.Count)
        {
            Debug.LogError("Trying to swap sprite, new index out of bounds");
        }
        else
        {
            _renderer.sprite = _sprites[newSpriteIndex];
        }
    }
}
