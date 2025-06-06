using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUIController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;


    private void OnEnable()
    {
        if(AudioManager.Instance != null)
        {
            _volumeSlider.value = AudioManager.Instance.Volume;
            _musicSlider.value = AudioManager.Instance.MusicVolume;
            _sfxSlider.value = AudioManager.Instance.SfxVolume;
        }
    }
}
