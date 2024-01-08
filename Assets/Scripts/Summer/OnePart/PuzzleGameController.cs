using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PuzzleGameController : MonoBehaviour
{
    private SummerGameController summerGameController;
    //要多少距離才可以發現線索
    [Header("線索收集")]
    public RectTransform takeRange;
    public GameObject player;
    public float radius;
    public GameObject detectObject;
    public Sprite searchImage;
    public Sprite fixImage;
    public Image btnImage;
    //public Text detectBtn;

    [Header("變量控制")]
    public PuzzleGameDataSo puzzleGameData;
    bool findPuzzle = false;
    //bool collectPuzzleClueMission;

    private int targetPuzzleCount = 6;
    public int puzzleCount = 0;

    [Header("拼圖")]
    public GameObject[] puzzleClue;
    private GameObject puzzleObject;
    private bool[] puzzleIsFind = new bool[6];


    [Header("拼圖遊戲開啟")]
    //看我的背包需不需要關閉
    public bool openPuzzleGame;
    public GameObject puzzleGame;
    public GameObject MainCamera;
    public GameObject Player;
    public GameObject TouchCanves;
    public GameObject puzzleStarPice;

    [Header("遊戲完成判斷")]
    public GridManager gridManager;
    private cookieGameController cookieGame;
    [HideInInspector] public static bool puzzleGameOver;
    private bool overGame;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;

    void OnEnable()
    {
        ResetDataEventSo.OnEventRaised += ResetPuzzleGameData;
    }

    void OnDisable()
    {
        ResetDataEventSo.OnEventRaised -= ResetPuzzleGameData;
    }
    //每次開啟遊戲都先判斷當前遊戲的狀態來決定要怎麼進行遊戲
    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();
        cookieGame = GetComponent<cookieGameController>();

        findPuzzle = puzzleGameData.isFindPuzzle;
        puzzleCount = puzzleGameData.puzzleClipCount;
        puzzleGameOver = puzzleGameData.puzzleGameOver;

        if (puzzleGameOver)
        {
            //星星拼圖顯示
            puzzleStarPice.SetActive(true);

            //動畫觀看完成在進行對話
            if (PlayerPrefs.GetInt("playAnim") == 1 && !overGame)
            {
                //引導去調查草叢
                summerGameController.openNarrationSystem();
                PlayerPrefs.SetInt("playAnim", 0);
            }

            //儲存進度,開始下一關遊戲
            cookieGame.startGame();
            overGame = true;
        }
        else
        {
            overGame = false;
        }


        //開啟找拼圖遊戲
        if (findPuzzle)
        {
            for (int i = 0; i < puzzleGameData.puzzleState.Length; i++)
            {
                if (!puzzleGameData.puzzleState[i])
                {
                    puzzleClue[i].SetActive(true);
                }
            }
        }

    }

    public void Update()
    {
        //拼圖遊戲完成
        //顯示完成告示牌內容（待決議要如何處理）-> 可以用旁白模式或用圖片顯示
        //這邊完成之後拼圖就算完成
        //只執行一次

        if (overGame)
        {
            return;
        }

        puzzleGameData.isFindPuzzle = findPuzzle;
        puzzleGameData.puzzleClipCount = puzzleCount;

        if (puzzleGameOver)
        {
            puzzleGame.SetActive(false);
            takeRange.transform.gameObject.SetActive(false);

            MainCamera.SetActive(true);
            player.SetActive(true);
            TouchCanves.SetActive(true);

            return;
        }

        //------------------

        //如果在碎片還未收集完成時靠近靠示牌，跳出一則訊息説請收集碎片
        //如果收集完成，就呈現BTN 説使用碎片來調查

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

        //如果player的狀態屬於 false 就不執行偵測的動作 
        if (player.activeInHierarchy == false)
        {
            return;
        }

        //要可以找拼圖時在讓她尋找
        foreach (Collider collider in colliders)
        {
            //開啟撿拼圖
            if (collider.gameObject.tag == "puzzlePices")
            {
                if (findPuzzle)
                {

                    foreach (Touch touch in Input.touches)
                    {
                        Vector2 touchPosition = touch.position;

                        if (RectTransformUtility.RectangleContainsScreenPoint(takeRange, touchPosition))
                        {
                            switch (touch.phase)
                            {
                                case TouchPhase.Began:

                                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                                    RaycastHit hit;

                                    if (Physics.Raycast(ray, out hit))
                                    {
                                        if (collider.gameObject.tag == "puzzlePices")
                                        {
                                            //根據點擊的物品來判斷哪一個位置已被撿起
                                            if (collider.gameObject.name == "star.1")
                                            {

                                                puzzleIsFind[0] = true;
                                                puzzleGameData.puzzleState[0] = true;
                                            }

                                            if (collider.gameObject.name == "star.2")
                                            {

                                                puzzleIsFind[1] = true;
                                                puzzleGameData.puzzleState[1] = true;
                                            }

                                            if (collider.gameObject.name == "star.3")
                                            {

                                                puzzleIsFind[2] = true;
                                                puzzleGameData.puzzleState[2] = true;
                                            }

                                            if (collider.gameObject.name == "star.4")
                                            {

                                                puzzleIsFind[3] = true;
                                                puzzleGameData.puzzleState[3] = true;
                                            }

                                            if (collider.gameObject.name == "star.5")
                                            {

                                                puzzleIsFind[4] = true;
                                                puzzleGameData.puzzleState[4] = true;
                                            }

                                            if (collider.gameObject.name == "star.6")
                                            {

                                                puzzleIsFind[5] = true;
                                                puzzleGameData.puzzleState[5] = true;
                                            }

                                            puzzleObject = collider.gameObject;
                                            puzzleObject.SetActive(false);
                                            puzzleCount++;
                                            return;
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }

            }

            //告示牌偵測
            if (collider.gameObject.tag == "BillBoard")
            {
                //修改告示牌
                if (findPuzzle && puzzleCount == targetPuzzleCount)
                {
                    detectObject.SetActive(true);
                    btnImage.sprite = fixImage;
                    return;
                }

                //初始偵測
                //看是否要可以要重複查看告示牌的任務
                if (!findPuzzle)
                {

                    //這邊代表第一次查看要跳出訊息説尋找餅乾
                    detectObject.SetActive(true);
                    btnImage.sprite = searchImage;
                    return;
                }

            }
        }
        //如果身邊沒有掃到section的部分，就將調查扭關閉
        detectObject.SetActive(false);
    }

    //Btn 使用
    public void PuzzleGame()
    {

        //拼圖碎片顯示
        //紀錄狀態避免旁白系統會一直往後進行
        if (!findPuzzle)
        {
            summerGameController.openNarrationSystem();
            //變數控制
            findPuzzle = true;


            //將拼圖碎片顯示
            for (int i = 0; i < puzzleClue.Length; i++)
            {
                puzzleClue[i].SetActive(true);
            }

            detectObject.SetActive(false);
            return;
        }


        //拼圖遊戲
        if (findPuzzle && puzzleCount == targetPuzzleCount)
        {
            openPuzzleGame = true;

            puzzleGame.SetActive(true);

            MainCamera.SetActive(false);
            player.SetActive(false);
            TouchCanves.SetActive(false);
            detectObject.SetActive(false);

            return;
        }

    }
    private void ResetPuzzleGameData()
    {
        puzzleGameData.isFindPuzzle = false;
        puzzleGameData.puzzleClipCount = 0;
        puzzleGameData.puzzleGameOver = false;
        for (int i = 0; i < puzzleGameData.puzzleState.Length; i++)
        {
            puzzleGameData.puzzleState[i] = false;
        }
    }
}
