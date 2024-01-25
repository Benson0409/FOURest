using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeachingGameController : MonoBehaviour
{
    //來顯示當前的任務目標是什麼 讓他們沒有看的對話也可以知道
    [Header("任務文字")]
    public Text misiionText;
    [Header("旁白系統")]
    public narrationSystem narration;
    public NarrationDataSo narrationData;
    public GameObject narrationPanel;
    public GameObject narrationSystem;

    [Header("教學關卡數據")]
    public TeachingGameDataSo teachingGameData;

    [Header("場景淡入淡出")]
    public SwitchScenes scenesCanvaPrefabs;


    public void openNarrationSystem(int playNarrationIndex)
    {
        narration.narrationTextAsset = narrationData.narrationTextAsset[playNarrationIndex];
        narration.openNarration = true;
        narration.startDialogue = true;
        narrationSystem.SetActive(true);
        narrationPanel.SetActive(true);
    }

    public void StartSummerGame()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }
}
