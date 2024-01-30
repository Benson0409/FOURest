using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerGameController : MonoBehaviour
{
    //開啟旁白
    //控制遊戲進行
    [Header("旁白系統")]
    public narrationSystem narration;
    public NarrationDataSo narrationData;
    public GameObject narrationPanel;
    public GameObject narrationSystem;
    [Header("遊戲資訊")]
    public MainMenuGameDataSo mainMenuGameData;
    public PuzzleGameDataSo puzzleGameData;
    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("玩家初始位置")]
    public GameObject player;
    public Transform initialPlayerLocate;
    [Header("水仙子動畫設定")]
    public Animator waterAnim;
    public DialogueDataSo waterDialogue;
    [Header("結束遊戲設定")]
    public GameObject gameOverCanva;
    void Awake()
    {
        if (mainMenuGameData.creatNewGame)
        {
            CreatNewGame();
        }
    }
    private void Start()
    {
        //遊戲一開始時啟動旁白
        if (!narrationData.isPlayAwake && !puzzleGameData.isFindPuzzle && !mainMenuGameData.creatNewGame)
        {
            narrationData.isPlayAwake = true;
            //開啟對話
            openNarrationSystem(0);
        }
    }
    void Update()
    {
        //水仙子動畫播放
        if (waterDialogue.currentIndex != 0)
        {
            waterAnim.SetTrigger("Idle");
        }
        //開啟結束結算畫面
        if (DialogueManager.openGameOverCanva)
        {
            SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
            switchScenes.StartCoroutine(switchScenes.FadeOutInScenes());
            gameOverCanva.SetActive(true);
        }
    }
    //開啟旁白
    /// <summary>
    /// 用來顯示旁白系統
    /// </summary>
    /// <param name="playNarrationIndex">根據傳進來的值，來判斷要顯示哪一段旁白</param>
    public void openNarrationSystem(int playNarrationIndex)
    {
        narration.narrationTextAsset = narrationData.narrationTextAsset[playNarrationIndex];
        narration.openNarration = true;
        narration.startDialogue = true;
        narrationSystem.SetActive(true);
        narrationPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("MainMenu"));
    }

    //將資料清除，並將人物移動到最初始的位置
    public void clearGameInformation()
    {
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();

        narrationData.isPlayAwake = false;

        player.transform.position = initialPlayerLocate.position;
        player.transform.rotation = initialPlayerLocate.rotation;


        PlayerPrefs.DeleteAll();

        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
        //print("資料重開");
    }
    public void CreatNewGame()
    {
        mainMenuGameData.creatNewGame = false;
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();

        narrationData.isPlayAwake = false;

        player.transform.position = initialPlayerLocate.position;
        player.transform.rotation = initialPlayerLocate.rotation;

    }
}
