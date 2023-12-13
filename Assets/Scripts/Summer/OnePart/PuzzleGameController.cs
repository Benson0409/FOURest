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
    public bool findPuzzle = false;
    public bool collectPuzzleClueMission;

    private int finalCount = 6;
    public int puzzleCount = 0;

    [Header("拼圖")]
    public GameObject[] puzzleClue;
    private GameObject puzzleObject;
    private bool[] puzzleIsFind = new bool[6];


    [Header("拼圖遊戲開啟")]
    public bool openPuzzleGame;
    public GameObject puzzleGame;
    public GameObject MainCamera;
    public GameObject Player;
    public GameObject TouchCanves;
    public GameObject puzzleStarPice;

    [Header("遊戲完成判斷")]
    public GridManager gridManager;
    private cookieGameController cookieGame;
    public static bool puzzleGameOver;
    private bool stopPuzzle;

    //每次開啟遊戲都先判斷當前遊戲的狀態來決定要怎麼進行遊戲
    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();
        cookieGame = GetComponent<cookieGameController>();

        if (PlayerPrefs.GetInt("puzzleGameOver") == 1)
        {
            puzzleStarPice.SetActive(true);
            puzzleGameOver = true;

            //動畫觀看完成在進行對話
            if (PlayerPrefs.GetInt("playAnim") == 1 && !stopPuzzle)
            {
                //引導去調查草叢
                summerGameController.openNarrationSystem();
                PlayerPrefs.SetInt("playAnim", 0);
            }

            stopPuzzle = true;
        }
        else
        {
            puzzleGameOver = false;
            stopPuzzle = false;
        }

        //開啟找拼圖遊戲
        if (PlayerPrefs.GetInt("findPuzzle") == 1)
        {
            findPuzzle = true;
            collectPuzzleClueMission = true;
            puzzleCount = PlayerPrefs.GetInt("puzzleCount");

            //拼圖遊戲尋找顯示判斷
            //看哪一個沒有被拿到，把他顯示出來
            if (PlayerPrefs.GetInt("puzzleIsFind[0]") != 1)
            {
                puzzleClue[0].SetActive(true);
            }

            if (PlayerPrefs.GetInt("puzzleIsFind[1]") != 1)
            {
                puzzleClue[1].SetActive(true);
            }

            if (PlayerPrefs.GetInt("puzzleIsFind[2]") != 1)
            {
                puzzleClue[2].SetActive(true);
            }

            if (PlayerPrefs.GetInt("puzzleIsFind[3]") != 1)
            {
                puzzleClue[3].SetActive(true);
            }

            if (PlayerPrefs.GetInt("puzzleIsFind[4]") != 1)
            {
                puzzleClue[4].SetActive(true);
            }

            if (PlayerPrefs.GetInt("puzzleIsFind[5]") != 1)
            {
                puzzleClue[5].SetActive(true);
            }
        }

    }

    public void Update()
    {
        //拼圖遊戲完成
        //顯示完成告示牌內容（待決議要如何處理）-> 可以用旁白模式或用圖片顯示
        //這邊完成之後拼圖就算完成
        //只執行一次

        if (stopPuzzle)
        {
            return;
        }

        if (puzzleGameOver)
        {
            puzzleGame.SetActive(false);
            takeRange.transform.gameObject.SetActive(false);
            //puzzleStarPice.SetActive(true);
            MainCamera.SetActive(true);
            player.SetActive(true);
            TouchCanves.SetActive(true);

            //引導去調查草叢
            //summerGameController.openNarrationSystem();

            //儲存進度
            //stopPuzzle = true;
            cookieGame.startGame();

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

        foreach (Collider collider in colliders)
        {
            //開啟撿拼圖
            if (collider.gameObject.tag == "puzzlePices")
            {

                if (collectPuzzleClueMission)
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
                                                PlayerPrefs.SetInt("puzzleIsFind[0]", 1);
                                            }

                                            if (collider.gameObject.name == "star.2")
                                            {

                                                puzzleIsFind[1] = true;
                                                PlayerPrefs.SetInt("puzzleIsFind[1]", 1);
                                            }

                                            if (collider.gameObject.name == "star.3")
                                            {

                                                puzzleIsFind[2] = true;
                                                PlayerPrefs.SetInt("puzzleIsFind[2]", 1);
                                            }

                                            if (collider.gameObject.name == "star.4")
                                            {

                                                puzzleIsFind[3] = true;
                                                PlayerPrefs.SetInt("puzzleIsFind[3]", 1);
                                            }

                                            if (collider.gameObject.name == "star.5")
                                            {

                                                puzzleIsFind[4] = true;
                                                PlayerPrefs.SetInt("puzzleIsFind[4]", 1);
                                            }

                                            if (collider.gameObject.name == "star.6")
                                            {

                                                puzzleIsFind[5] = true;
                                                PlayerPrefs.SetInt("puzzleIsFind[5]", 1);
                                            }

                                            puzzleObject = collider.gameObject;
                                            puzzleObject.SetActive(false);
                                            puzzleCount++;
                                            PlayerPrefs.SetInt("puzzleCount", puzzleCount);
                                            return;
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }

                //if (collectPuzzleClueMission)
                //{

                //    detectObject.SetActive(true);
                //    puzzleObject = collider.gameObject;
                //    detectBtn.text = "撿起";
                //    return;
                //}
            }

            //告示牌偵測
            if (collider.gameObject.tag == "BillBoard")
            {
                //修改告示牌
                if (findPuzzle && puzzleCount == finalCount)
                {
                    //收集完成後靠近告示牌
                    collectPuzzleClueMission = false;
                    detectObject.SetActive(true);
                    btnImage.sprite = fixImage;
                    //detectBtn.text = "修補告示牌";
                    return;
                }

                //初始偵測
                //看是否要可以要重複查看告示牌的任務
                if (!findPuzzle)
                {

                    //這邊代表第一次查看要跳出訊息説尋找餅乾
                    detectObject.SetActive(true);
                    btnImage.sprite = searchImage;
                    //detectBtn.text = "查看";
                    return;
                }

            }
        }
        //如果身邊沒有掃到section的部分，就將調查扭關閉
        detectObject.SetActive(false);
    }


    public void PuzzleGame()
    {

        //拼圖碎片顯示
        //紀錄狀態避免旁白系統會一直往後進行
        if (!findPuzzle)
        {
            summerGameController.openNarrationSystem();
            //變數控制
            findPuzzle = true;
            collectPuzzleClueMission = true;
            PlayerPrefs.SetInt("findPuzzle", 1);

            //狀態反應 
            for (int i = 0; i < puzzleClue.Length; i++)
            {
                puzzleClue[i].SetActive(true);
            }

            detectObject.SetActive(false);
            return;
        }

        ////撿拼圖功能
        //if (collectPuzzleClueMission)
        //{

        //    if (puzzleObject != null)
        //    {
        //        puzzleObject.SetActive(false);
        //    }
        //    puzzleCount++;
        //    PlayerPrefs.SetInt("puzzleCount", puzzleCount);
        //    detectObject.SetActive(false);
        //    return;
        //}

        //拼圖遊戲
        if (findPuzzle && puzzleCount == finalCount)
        {
            openPuzzleGame = true;

            puzzleGame.SetActive(true);
            MainCamera.SetActive(false);
            player.SetActive(false);
            TouchCanves.SetActive(false);

            //開啟拼圖遊戲
            detectObject.SetActive(false);
            return;
        }

    }
}
