using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempleGameController : MonoBehaviour
{
    //整體的遊戲控制大門＋祭壇
    //AR偵測判斷的部分，以及線索的統整
    //因為這一關每個部分都息息相關，所以寫在一起，後面要換再更換寫法
    //密碼的配置也在這裡判斷

    [Header("對話系統判斷")]
    public DialogueManager dialogueManager;
    public GameObject dialogueCanva;

    private SummerGameController summerGameController;
    private ColorGameController colorGameController;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("腳本控制")]
    //大門滑動控制
    public doorSlider doorSliderGame;

    //音樂祭壇控制
    public musicAltar musicAltarGame;

    [Header("操作控制")]
    public GameObject touchCanva;

    [Header("場景物件")]
    //開啟神廟大門
    public GameObject DoorCanva;

    //開啟音樂祭壇
    public GameObject MusicAltarCanva;

    //開啟神廟線索探索
    public GameObject templeDoor;
    public GameObject openTempleDoor;
    public GameObject treasure;
    public GameObject openTreasure;


    [Header("偵測控制")]
    public GameObject player;
    public float radius;

    [Header("按鈕控制")]
    public GameObject DetectObject;
    public Image btnImage;
    public Sprite searchImage;
    public Sprite openImage;
    //public Text DetectBtn;
    [Header("背包控制")]
    public GameObject inventoryUI;

    [Header("關卡變數")]
    public bool startTempleGame;
    public static bool templeGameOver;
    private bool openAR;


    [Header("神廟大門控制變數")]
    //神廟大門控制遊戲
    public bool findDoorClue;
    public bool startDoorGame;

    [Header("音樂祭壇變數")]
    //音樂祭壇控制變數
    public bool findMusicClue;
    public bool startMusicGame;
    public bool finishMusicGame;


    [Header("遊戲數據")]
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    //偵測看有沒有對話
    public DialogueDataSo liliDialogue;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;

    //靠近線索的時候用這個來代表
    private bool isClue;

    void OnEnable()
    {
        ResetDataEventSo.OnEventRaised += ResetTempleGameData;
    }

    void OnDisable()
    {
        ResetDataEventSo.OnEventRaised -= ResetTempleGameData;
    }



    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();
        colorGameController = GetComponent<ColorGameController>();

        //讀取資料
        startTempleGame = templeGameData.startTempleGame;
        templeGameOver = templeGameData.templeGameOver;
        startDoorGame = templeGameData.startDoorGame;
        findDoorClue = templeGameData.findDoorClue;
        startMusicGame = templeGameData.startMusicGame;
        finishMusicGame = templeGameData.finishMusicGame;
        findMusicClue = templeGameData.findMusicClue;

        if (templeGameOver)
        {
            //寶箱開啟狀態
            openTreasure.SetActive(true);
            treasure.SetActive(false);
        }



        if (startTempleGame)
        {
            //判斷最後對話進程 
            if (finishMusicGame)
            {

                if (PlayerPrefs.GetInt("playAnim") == 1)
                {
                    //旁白開啟
                    summerGameController.openNarrationSystem();
                    PlayerPrefs.SetInt("playAnim", 0);
                    PlayerPrefs.Save();
                }
            }

            if (startMusicGame)
            {

                openTempleDoor.SetActive(true);
                templeDoor.SetActive(false);
                return;
            }
        }
        else
        {
            startTempleGame = false;
        }
    }

    private void Update()
    {
        if (templeGameOver)
        {
            return;
        }

        //餅乾遊戲還未結束，不進行下一關
        if (!startTempleGame || openAR)
        {
            return;
        }

        //紀錄資料
        templeGameData.startDoorGame = startDoorGame;
        templeGameData.findDoorClue = findDoorClue;
        templeGameData.startMusicGame = startMusicGame;
        templeGameData.findMusicClue = findMusicClue;

        if (finishMusicGame)
        {
            if (liliDialogue.repeatText)
            {
                print("123");
                DetectObject.SetActive(false);

                templeGameData.templeGameOver = true;
                colorGameController.startGame();
            }
            return;

        }

        if (startTempleGame)
        {

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

            //如果player的狀態屬於 false 就不執行偵測的動作 
            if (player.activeInHierarchy == false)
            {
                return;
            }

            foreach (Collider collider in colliders)
            {
                if (startMusicGame)
                {
                    if (collider.tag == "musicAltar")
                    {
                        if (!findMusicClue)
                        {

                            //DetectBtn.text = "調查祭壇";
                            btnImage.sprite = searchImage;
                            //開啟音樂祭壇線索
                            DetectObject.SetActive(true);
                            return;
                        }

                        if (findMusicClue)
                        {
                            //DetectBtn.text = "開啟祭壇";
                            btnImage.sprite = openImage;
                            //開啟音樂祭壇線索
                            DetectObject.SetActive(true);
                            return;
                        }
                    }


                    if (collider.tag == "musicClue")
                    {
                        isClue = true;
                        if (findMusicClue)
                        {

                            //準備開啟AR
                            btnImage.sprite = searchImage;

                            //將要生成的物體帶入AR中
                            ARSystem.musicAltarClue(collider.gameObject);
                            DetectObject.SetActive(true);
                            return;
                        }
                    }
                }
                if (startDoorGame)
                {
                    //開啟第一部分任務 -> 開啟神廟大門
                    if (collider.transform.tag == "door")
                    {

                        //以調查線索
                        if (findDoorClue)
                        {
                            //開啟大門介面
                            btnImage.sprite = openImage;
                            DetectObject.SetActive(true);

                        }

                        //第一次開啟大門線索
                        if (!findDoorClue)
                        {

                            //DetectBtn.text = "調查";
                            btnImage.sprite = searchImage;
                            DetectObject.SetActive(true);
                        }
                        return;
                    }

                    if (collider.transform.tag == "doorClue")
                    {
                        isClue = true;
                        if (findDoorClue)
                        {
                            //點擊BTN可以調查線索．開啟ＡＲ調查，openAR = true
                            ARSystem.doorClue(collider.gameObject);
                            btnImage.sprite = searchImage;
                            DetectObject.SetActive(true);
                            return;
                        }
                    }
                }
            }
        }
        //遠離調查物後,將他變成false,靠近在變為true
        isClue = false;
        DetectObject.SetActive(false);
    }

    public void templeBtnController()
    {

        if (startMusicGame)
        {
            //初始調查
            if (!findMusicClue)
            {
                //跳出旁白
                findMusicClue = true;
                summerGameController.openNarrationSystem();
                DetectObject.SetActive(false);
                //顯示線索調查
                return;
            }

            //調查AR線索
            if (findMusicClue && isClue)
            {
                SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
                switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
                return;
            }

            //調查音樂祭壇
            if (findMusicClue)
            {
                //開啟祭壇畫面
                touchCanva.SetActive(false);
                DetectObject.SetActive(false);
                MusicAltarCanva.SetActive(true);
                return;
            }
        }
        if (startDoorGame)
        {

            //根據不同狀況判斷按紐要執行的程序
            if (!findDoorClue)
            {
                //開啟提示對話，跟他們説要去尋找線索
                print("旁白提示");
                summerGameController.openNarrationSystem();
                findDoorClue = true;
                DetectObject.SetActive(false);
                return;
            }

            if (findDoorClue && isClue)
            {
                //開啟AR
                //切換場景
                SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
                switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
                return;
            }

            if (findDoorClue)
            {
                inventoryUI.SetActive(false);
                DetectObject.SetActive(false);
                DoorCanva.SetActive(true);
                touchCanva.SetActive(false);
                print("開始破解大門遊戲");
                return;
            }
        }
    }

    public void startGame()
    {
        if (cookieGameData.cookieGameOver)
        {
            templeGameData.startTempleGame = true;
            templeGameData.startDoorGame = true;
        }
        else
        {
            startTempleGame = false;
        }
    }

    public void closeGameCanva()
    {
        inventoryUI.SetActive(true);
        touchCanva.SetActive(true);
        DoorCanva.SetActive(false);
        MusicAltarCanva.SetActive(false);
    }

    public void startMusicAltar()
    {
        if (doorSlider.openDoorGame)
        {
            inventoryUI.SetActive(true);
            //調整活動進度
            startMusicGame = true;

        }
    }

    private void ResetTempleGameData()
    {
        //清除資料
        templeGameData.startTempleGame = false;
        templeGameData.templeGameOver = false;
        templeGameData.startDoorGame = false;
        templeGameData.findDoorClue = false;
        templeGameData.startMusicGame = false;
        templeGameData.finishMusicGame = false;
        templeGameData.findMusicClue = false;
    }
}
