using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class TimeLineController : MonoBehaviour
{
    //轉場過來播放動畫
    //利用int的數值變化，來讓我們播放不同的動畫
    public PlayableDirector playableDirector;
    public TimelineAsset puzzle;
    public TimelineAsset cookie;
    public TimelineAsset templeDoor;
    public TimelineAsset treasure;
    public TimelineAsset color;
    public TimelineAsset crystalBall;
    public TimelineAsset water;

    [Header("動畫轉場")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("目前播放的動畫片段")]
    public int animationClip = 0;

    [Header("變量控制")]
    public bool isPlay;
    public bool swichScene;
    public bool over = false;

    void Awake()
    {
        isPlay = true;
        if (PlayerPrefs.GetInt("animSave") == 1)
        {
            animationClip = PlayerPrefs.GetInt("animationClip");
        }
        switch (animationClip)
        {
            case 0:
                // 設定要播放的 Timeline
                playableDirector.playableAsset = puzzle;
                break;
            case 1:
                playableDirector.playableAsset = cookie;
                break;
            case 2:
                playableDirector.playableAsset = templeDoor;
                break;
            case 3:
                playableDirector.playableAsset = treasure;
                break;
            case 4:
                playableDirector.playableAsset = color;
                break;
            case 5:
                playableDirector.playableAsset = crystalBall;
                break;
            case 6:
                playableDirector.playableAsset = water;
                break;
        }
    }
    void Update()
    {
        if (over)
        {
            return;
        }
        if (swichScene)
        {
            over = true;
            animationClip++;

            PlayerPrefs.SetInt("playAnim", 1);
            PlayerPrefs.Save();

            //進行轉場
            SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
            switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
        }

        // 在場景中尋找名為 "SceneCanvas" 的物體
        GameObject targetObject = GameObject.Find("SceneCanvas");

        //物體消失
        if (targetObject == null)
        {
            print("播放");
            playableDirector.Play();
        }
        // 播放 Timeline
        playableDirector.stopped += OnTimelineStopped;
    }

    // 在 Timeline 播放停止後觸發的事件
    void OnTimelineStopped(PlayableDirector director)
    {
        // 確認是正確的 PlayableDirector
        if (director == playableDirector)
        {
            swichScene = true;
            PlayerPrefs.SetInt("animationClip", animationClip);
            //紀錄已經執行過，讓下一次執行可以繼承數值
            PlayerPrefs.SetInt("animSave", 1);

            // 移除事件監聽，避免重複觸發
            playableDirector.stopped -= OnTimelineStopped;

        }
    }
}
