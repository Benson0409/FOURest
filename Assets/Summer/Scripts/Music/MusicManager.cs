using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip BGM;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGM;
        audioSource.Play();
    }
}
