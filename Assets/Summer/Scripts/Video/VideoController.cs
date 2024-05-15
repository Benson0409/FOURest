using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
public class VideoController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public CinemachineVirtualCamera cam1;

    [Header("動畫轉場")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("變量控制")]
    public bool isPlay;
    public bool swichScene;
    private bool isPlayOver;

    void Awake()
    {
        isPlay = true;
    }

    void Update()
    {
        //避免重複進行場景切換
        if (isPlayOver)
        {
            return;
        }

        if (swichScene)
        {
            //進行轉場
            GoToSummer();
        }

        // 在場景中尋找名為 "SceneCanvas" 的物體
        GameObject targetObject = GameObject.Find("SceneCanvas");

        //淡入淡出結束後再進行動畫播放
        if (targetObject == null)
        {
            print("播放");
            playableDirector.Play();
        }
        // 播放 Timeline
        playableDirector.stopped += OnTimelineStopped;
    }

    public void GoToSummer()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
        isPlayOver = true;
    }

    // 在 Timeline 播放停止後觸發的事件
    void OnTimelineStopped(PlayableDirector director)
    {
        // 確認是正確的 PlayableDirector
        if (director == playableDirector)
        {
            swichScene = true;
            cam1.Priority = 10;
            // 移除事件監聽，避免重複觸發
            playableDirector.stopped -= OnTimelineStopped;

        }
    }
}
