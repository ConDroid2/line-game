using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;


    private void OnEnable()
    {
        if(AudioManager.Instance != null)
        {
            _volumeSlider.value = AudioManager.Instance.Volume;
        }
    }
}
