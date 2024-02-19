using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    public PlayAudioEventSo playAudioEventSo;
    public AudioClip audioClip;
    public bool playOnable;

    //可以用運用要觸發音效的地方
    void OnEnable()
    {
        if (playOnable)
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip()
    {
        playAudioEventSo.RaiseEvent(audioClip);
    }
}
