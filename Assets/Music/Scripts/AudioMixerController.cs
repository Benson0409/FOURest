using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    public void SetBGMVolume(float BGMSliderValue)
    {
        audioMixer.SetFloat("BGMVolume", BGMSliderValue);
    }

    public void SetVFXVolume(float VFXSliderValue)
    {
        audioMixer.SetFloat("VFXVolume", VFXSliderValue);
    }
}
