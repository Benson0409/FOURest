using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenVFX : MonoBehaviour
{
    //當螢幕偵測到手指的觸碰時就觸發事件，播放音效
    AudioDefination audioDefination;
    void Awake()
    {
        audioDefination = GetComponent<AudioDefination>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchPhase touchPhase = Input.GetTouch(0).phase;
            if (touchPhase == TouchPhase.Began)
            {
                audioDefination.PlayAudioClip();
            }
        }
    }
}
