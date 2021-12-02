using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetting : MonoBehaviour
{
    public AudioSource _Music;
    public Slider _VolumeSlider;
    public GameObject _ImageSoundOn;
    public GameObject _ImageSoundOff;
    private bool _SoundOn = true;

    public void SoundVolume()
    {
        _Music.volume = _VolumeSlider.value;
    }

    public void SoundOnOff()
    {
        if (_SoundOn)
        {
            _ImageSoundOn.SetActive(false);
            _ImageSoundOff.SetActive(true);
            _SoundOn = false;
            _Music.Stop();
        }
        else
        {
            _ImageSoundOff.SetActive(false);
            _ImageSoundOn.SetActive(true);
            _SoundOn = true;
            _Music.Play();
        }
    }
}
