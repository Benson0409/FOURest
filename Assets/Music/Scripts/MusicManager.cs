using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    // private AudioSource audioSource;
    // public AudioClip BGM;

    // void Awake()
    // {
    //     audioSource = GetComponent<AudioSource>();
    //     audioSource.clip = BGM;
    //     audioSource.Play();
    // }

    [Header("事件監聽")]
    public PlayAudioEventSo BGMEvent;
    public PlayAudioEventSo FXEvent;

    [Header("AudioSource")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    //監聽兩個音效事件有無被觸發，如果有的話就播放音效
    void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
    }

    void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
    }

    private void OnBGMEvent(AudioClip clip)
    {
        print("123456789");
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
}
