using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;
    [Header("教學關卡數據")]
    public TeachingGameDataSo teachingGameData;
    [Header("遊戲數據")]
    public MainMenuGameDataSo mainMenuGameData;

    void Awake()
    {
        Application.targetFrameRate = 300;
    }

    //創建新遊戲
    public void StartNewGame()
    {
        SettingGameData();
        SkipTeachingGameData();
        LoadGameScene("TestScene");
    }

    //進入新手訓練關卡
    public void TeachingGame()
    {
        print("開始教學關卡");
        SettingGameData();
        ClearTeachingGameData();
        LoadGameScene("TeachingGame");
    }

    //遊玩先前的遊戲紀錄
    public void StartGame()
    {
        print("開始遊戲");
        LoadGameScene("TestScene");
    }

    //離開遊戲
    public void ExitGame()
    {
        print("退出遊戲");
        Application.Quit();
    }

    private void SettingGameData()
    {
        mainMenuGameData.creatNewGame = true;
        mainMenuGameData.isPlaySave = true;
        PlayerPrefs.DeleteAll();
    }

    private void LoadGameScene(string Scene)
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes(Scene));
    }

    private void ClearTeachingGameData()
    {
        teachingGameData.isFirst = false;
        teachingGameData.isMove = false;
        teachingGameData.isMoveTip = false;
        teachingGameData.isView = false;
        teachingGameData.isViewTip = false;
        teachingGameData.isBtnActive = false;
        teachingGameData.isBtnTip = false;
        teachingGameData.isPick = false;
        teachingGameData.isDetect = false;
        teachingGameData.isARPick = false;
        teachingGameData.isTeachingGameOver = false;
    }

    private void SkipTeachingGameData()
    {
        teachingGameData.isFirst = true;
        teachingGameData.isMove = true;
        teachingGameData.isMoveTip = true;
        teachingGameData.isView = true;
        teachingGameData.isViewTip = true;
        teachingGameData.isBtnActive = true;
        teachingGameData.isBtnTip = true;
        teachingGameData.isPick = true;
        teachingGameData.isDetect = true;
        teachingGameData.isARPick = true;
        teachingGameData.isTeachingGameOver = true;
    }


}
