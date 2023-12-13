using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class TestPlay : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public PlayableDirector playableDirector;
    public TimelineAsset timelineAsset1;
    public TimelineAsset timelineAsset;




    //public time timeline;
    public void Play1()
    {
        // 設定要播放的 Timeline
        playableDirector.playableAsset = timelineAsset1;

        // 啟用第一個相機
        cam1.SetActive(true);

        // 播放 Timeline
        playableDirector.Play();
        playableDirector.stopped += OnTimelineStopped;
    }

    // 在 Timeline 播放停止後觸發的事件
    void OnTimelineStopped(PlayableDirector director)
    {
        // 確認是正確的 PlayableDirector
        if (director == playableDirector)
        {
            // 禁用第一個相機
            cam1.SetActive(false);

            // 移除事件監聽，避免重複觸發
            playableDirector.stopped -= OnTimelineStopped;
        }
    }
    public void Play2()
    {
        playableDirector.playableAsset = timelineAsset;
        playableDirector.Play();
    }
}
