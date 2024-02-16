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

    //用來判斷要播放哪一個對應的動畫
    [Header("遊戲數據")]
    public PuzzleGameDataSo puzzleGameData;
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    [Header("變量控制")]
    public bool isPlay;
    public bool swichScene;
    private bool isPlayOver;

    void Awake()
    {
        isPlay = true;
        if (!puzzleGameData.isPlayAnim)
        {
            animationClip = 0;
            playableDirector.playableAsset = puzzle;
        }
        else if (!cookieGameData.isPlayAnim)
        {
            animationClip = 1;
            playableDirector.playableAsset = cookie;
        }
        else if (!templeGameData.isPlayDoorAnim)
        {
            animationClip = 2;
            playableDirector.playableAsset = templeDoor;
        }
        else if (!templeGameData.isPlayMusicAnim)
        {
            animationClip = 3;
            playableDirector.playableAsset = treasure;
        }
        else if (!colorGameData.isPlayFiliterAnim)
        {
            animationClip = 4;
            playableDirector.playableAsset = color;
        }
        else if (!colorGameData.isPlayCrystalBallAnim)
        {
            animationClip = 5;
            playableDirector.playableAsset = crystalBall;
        }
        else
        {
            animationClip = 6;
            playableDirector.playableAsset = water;
        }
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
            SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
            switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
            isPlayOver = true;
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
