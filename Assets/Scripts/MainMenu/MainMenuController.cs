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
    //創建新遊戲
    public void StartNewGame()
    {
        mainMenuGameData.creatNewGame = true;
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }
    //直接進入遊戲
    public void StartGame()
    {
        print("開始遊戲");
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }

    //進入新手訓練關卡
    public void TeachingGame()
    {
        print("開始教學關卡");
        mainMenuGameData.creatNewGame = true;
        ClearTeachingGameData();
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TeachingGame"));
    }
    public void ExitGame()
    {
        print("退出遊戲");
        Application.Quit();
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

    //離開遊戲

}
