using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cookieGameController : MonoBehaviour
{
    private TempleGameController templeGameController;
    private SummerGameController summerGameController;
    public DialogueManager dialogueManager;
    [Header("偵測物體範圍")]
    public GameObject player;
    public float radius;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("餅乾遊戲變量")]
    private bool startCookieGame;


    [Header("AR生成物控制")]
    private bool cookie1;
    private bool cookie2;
    private bool cookie3;
    private int findCookieCount = 0;



    [Header("按鈕控制")]
    public GameObject DetectObject;
    //public Text DetectBtn;
    public Image btnImage;
    public Sprite searchImage;

    [Header("莉莉絲位置")]
    public VoidEventSo LiliChangeEventSo;

    [Header("遊戲數據")]
    public PuzzleGameDataSo puzzleGameData;
    public CookieGameDataSo cookieGameData;
    public DialogueDataSo liliDialogueData;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;
    private bool isClear;


    void OnEnable()
    {
        ResetDataEventSo.OnEventRaised += ResetCookieGameData;
    }

    void OnDisable()
    {
        ResetDataEventSo.OnEventRaised -= ResetCookieGameData;
    }



    private void Awake()
    {
        templeGameController = GetComponent<TempleGameController>();
        summerGameController = GetComponent<SummerGameController>();

        ReadCookieGameData();

        if (startCookieGame)
        {

            //動畫播放完成後與莉莉絲出現後就出現旁白
            if (!cookieGameData.isPlayAnim && cookieGameData.isFindCookie)
            {
                //莉莉絲出現
                LiliChangeEventSo.RaiseEvent();
                summerGameController.openNarrationSystem(3);
                cookieGameData.isPlayAnim = true;

            }

        }

    }



    void Update()
    {
        //偵測有沒有靠近餅乾區域
        //開啟BTN -> 按下進入ＡＲ場景
        //需繼承點擊的物體給ＡＲ生成
        //要做好數據控管，因為是場景切換
        //以及餅乾的數據控管

        if (cookieGameData.cookieGameOver)
        {
            return;
        }
        if (startCookieGame != true)
        {
            return;
        }

        SaveCookieGameData();

        if (cookieGameData.isFindCookie && !cookieGameData.cookieGameOver)
        {

            // 莉莉絲出現
            // 餅乾遊戲結束，進入下一部分，神殿開啟，找尋樂譜
            // 引導去找莉莉絲對話

            if (liliDialogueData.repeatText)
            {
                cookieGameData.cookieGameOver = true;
                templeGameController.startGame();
            }
            return;
        }


        if (startCookieGame)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

            //如果player的狀態屬於 false 就不執行偵測的動作 
            if (player.activeInHierarchy == false)
            {
                return;
            }

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.tag == "cookie1AR" && !cookie1)
                {
                    DetectObject.SetActive(true);
                    btnImage.sprite = searchImage;

                    ARSystem.cookie1Field(collider.gameObject);
                    return;
                }

                if (collider.gameObject.tag == "cookie2AR" && !cookie2)
                {
                    DetectObject.SetActive(true);
                    btnImage.sprite = searchImage;

                    ARSystem.cookie2Field(collider.gameObject);
                    return;
                }

                if (collider.gameObject.tag == "cookie3AR" && !cookie3)
                {
                    DetectObject.SetActive(true);
                    btnImage.sprite = searchImage;

                    ARSystem.cookie3Field(collider.gameObject);
                    return;
                }

                DetectObject.SetActive(false);
            }

        }

    }



    public void startGame()
    {
        if (puzzleGameData.puzzleGameOver)
        {
            startCookieGame = true;
        }

    }


    public void findCookieGameBtn()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
    }

    public void switchAnimScene()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }

    private void SaveCookieGameData()
    {
        if (!isClear)
        {
            cookieGameData.startCookieGame = startCookieGame;
            cookieGameData.findCookieCount = findCookieCount;
            cookieGameData.cookie1Field = cookie1;
            cookieGameData.cookie2Field = cookie2;
            cookieGameData.cookie3Field = cookie3;
        }
    }
    private void ReadCookieGameData()
    {
        //將資料都帶入變數中
        startCookieGame = cookieGameData.startCookieGame;
        findCookieCount = cookieGameData.findCookieCount;
        cookie1 = cookieGameData.cookie1Field;
        cookie2 = cookieGameData.cookie2Field;
        cookie3 = cookieGameData.cookie3Field;

        if (cookie1 && cookie2 && cookie3)
        {
            cookieGameData.isFindCookie = true;
        }
    }

    private void ResetCookieGameData()
    {
        isClear = true;
        cookieGameData.startCookieGame = false;
        cookieGameData.cookieGameOver = false;
        cookieGameData.isPlayAnim = false;
        cookieGameData.findCookieCount = 0;
        cookieGameData.isFindCookie = false;
        cookieGameData.cookie1Field = false;
        cookieGameData.cookie2Field = false;
        cookieGameData.cookie3Field = false;
    }

}
