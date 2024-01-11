using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cookieGameController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    private TempleGameController templeGameController;
    private SummerGameController summerGameController;

    [Header("餅乾遊戲變量")]
    public static bool cookieGameOver;
    public bool startCookieGame;

    [Header("偵測物體範圍")]
    public GameObject player;
    public float radius;

    [Header("AR生成物控制")]
    //public bool openAR;
    private bool cookie1;
    private bool cookie2;
    private bool cookie3;
    private int findCookieCount = 0;



    [Header("按鈕控制")]
    public GameObject DetectObject;
    //public Text DetectBtn;
    public Image btnImage;
    public Sprite searchImage;

    [Header("角色生成")]
    public VoidEventSo LiliChangeEventSo;

    [Header("遊戲數據")]
    public PuzzleGameDataSo puzzleGameData;
    public CookieGameDataSo cookieGameData;
    public DialogueDataSo liliDialogueData;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;


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

        //findCookieGameDetect();

        //將資料都帶入變數中
        startCookieGame = cookieGameData.startCookieGame;
        cookieGameOver = cookieGameData.cookieGameOver;
        findCookieCount = cookieGameData.findCookieCount;
        cookie1 = cookieGameData.cookie1Field;
        cookie2 = cookieGameData.cookie2Field;
        cookie3 = cookieGameData.cookie3Field;

        if (startCookieGame)
        {

            //動畫播放完成後與莉莉絲出現後就出現旁白
            if (PlayerPrefs.GetInt("playAnim") == 1 && cookie1 && cookie2 && cookie3 && !cookieGameOver)
            {
                //莉莉絲出現
                LiliChangeEventSo.RaiseEvent();


                summerGameController.openNarrationSystem();
                PlayerPrefs.SetInt("playAnim", 0);
                PlayerPrefs.Save();

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

        if (cookieGameOver)
        {
            return;
        }
        if (startCookieGame != true)
        {
            return;
        }

        cookieGameData.startCookieGame = startCookieGame;
        cookieGameData.cookieGameOver = cookieGameOver;
        cookieGameData.findCookieCount = findCookieCount;
        cookieGameData.cookie1Field = cookie1;
        cookieGameData.cookie2Field = cookie2;
        cookieGameData.cookie3Field = cookie3;

        if (cookie1 && cookie2 && cookie3 && !cookieGameOver)
        {

            // 莉莉絲出現
            // 餅乾遊戲結束，進入下一部分，神殿開啟，找尋樂譜
            // 引導去找莉莉絲對話

            if (liliDialogueData.repeatText)
            {
                cookieGameData.cookieGameOver = true;
                templeGameController.startGame();
            }
        }


        if (startCookieGame && !cookieGameOver)
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
        if (!puzzleGameData.puzzleGameOver)
        {
            //代表上一關還沒結束
            return;
        }
        else
        {
            //顯示找餅乾區域
            startCookieGame = true;
        }
    }

    public void findCookieGameDetect()
    {
        if (PlayerPrefs.GetInt("findCookie1") == 1)
        {
            cookie1 = true;
            findCookieCount++;
        }
        else
        {
            cookie1 = false;
        }

        if (PlayerPrefs.GetInt("findCookie2") == 1)
        {
            cookie2 = true;
            findCookieCount++;
        }
        else
        {
            cookie2 = false;
        }

        if (PlayerPrefs.GetInt("findCookie3") == 1)
        {
            cookie3 = true;
            findCookieCount++;
        }
        else
        {
            cookie3 = false;
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

    private void ResetCookieGameData()
    {
        cookieGameData.startCookieGame = false;
        cookieGameData.cookieGameOver = false;
        cookieGameData.findCookieCount = 0;
        cookieGameData.cookie1Field = false;
        cookieGameData.cookie2Field = false;
        cookieGameData.cookie3Field = false;
    }

}
