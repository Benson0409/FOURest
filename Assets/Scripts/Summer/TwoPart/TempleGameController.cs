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
    public bool templeGameStart;
    public static bool templeGameFinish;
    private bool openAR;


    [Header("神廟大門控制變數")]
    //神廟大門控制遊戲
    public static bool doorGame;
    public bool findDoorClue;
    public bool startDoorGame;

    [Header("音樂祭壇變數")]
    //音樂祭壇控制變數
    public bool altarGame;
    public bool canStart;
    public bool findAltarGame;
    public bool startAltarGame;
    public bool finishAltarGame;

    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();
        colorGameController = GetComponent<ColorGameController>();

        if (PlayerPrefs.GetInt("templeGameFinish") == 1)
        {
            openTreasure.SetActive(true);
            treasure.SetActive(false);
            templeGameFinish = true;
        }

        if (PlayerPrefs.GetInt("templeGameStart") == 1)
        {
            templeGameStart = true;

            //判斷最後對話進程 
            if (PlayerPrefs.GetInt("finishAltarGame") == 1)
            {
                finishAltarGame = true;

                if (PlayerPrefs.GetInt("playAnim") == 1)
                {
                    //旁白開啟
                    summerGameController.openNarrationSystem();
                    PlayerPrefs.SetInt("playAnim", 0);
                    PlayerPrefs.Save();
                }
            }

            if (PlayerPrefs.GetInt("starMusicAltar") == 1)
            {
                altarGame = true;
                openTempleDoor.SetActive(true);
                templeDoor.SetActive(false);
                if (PlayerPrefs.GetInt("canStart") == 1)
                {
                    canStart = true;
                    startAltarGame = true;
                }
                if (PlayerPrefs.GetInt("findAltarGame") == 1)
                {
                    findAltarGame = true;
                    //startAltarGame = true;
                }
                return;
            }

            if (PlayerPrefs.GetInt("startDoorGame") == 1)
            {
                startDoorGame = true;
                doorGame = true;
                return;
            }
        }
        else
        {
            templeGameStart = false;
        }
    }

    private void Update()
    {
        if (templeGameFinish)
        {
            return;
        }

        //餅乾遊戲還未結束，不進行下一關
        if (!templeGameStart || openAR)
        {
            return;
        }

        if (finishAltarGame)
        {
            DetectObject.SetActive(false);

            //任務狀態是false開啟顏色關卡

            print("color game Satrt");
            PlayerPrefs.SetInt("templeGameFinish", 1);
            PlayerPrefs.Save();
            colorGameController.startGame();
            templeGameFinish = true;
            return;

        }

        if (templeGameStart)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

            //如果player的狀態屬於 false 就不執行偵測的動作 
            if (player.activeInHierarchy == false)
            {
                return;
            }

            foreach (Collider collider in colliders)
            {
                if (altarGame)
                {
                    // openTempleDoor.SetActive(true);
                    // templeDoor.SetActive(false);
                    if (collider.tag == "musicAltar")
                    {
                        if (!findAltarGame)
                        {

                            //DetectBtn.text = "調查祭壇";
                            btnImage.sprite = searchImage;
                            //開啟音樂祭壇線索
                            DetectObject.SetActive(true);
                            return;
                        }

                        if (!summerGameController.narration.openNarration && !canStart)
                        {
                            startAltarGame = true;
                            canStart = true;

                            PlayerPrefs.SetInt("canStart", 1);
                            PlayerPrefs.Save();
                        }

                        if (canStart)
                        {
                            startAltarGame = true;
                        }

                        if (startAltarGame)
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
                        startAltarGame = false;
                        if (findAltarGame)
                        {

                            //準備開啟AR
                            //DetectBtn.text = "調查線索";
                            btnImage.sprite = searchImage;

                            //將要生成的物體帶入AR中
                            ARSystem.musicAltarClue(collider.gameObject);
                            DetectObject.SetActive(true);
                            return;
                        }
                    }
                }
                if (!altarGame)
                {
                    //開啟第一部分任務 -> 開啟神廟大門
                    if (collider.transform.tag == "door")
                    {

                        //以調查線索
                        if (startDoorGame)
                        {
                            //開啟大門介面
                            //DetectBtn.text = "開啟大門";
                            btnImage.sprite = openImage;
                            DetectObject.SetActive(true);
                            //return;
                        }

                        //第一次開啟大門線索
                        //Btn按下提示附近找尋線索，與修理告示牌相同道理
                        if (!doorGame)
                        {

                            //DetectBtn.text = "調查";
                            btnImage.sprite = searchImage;
                            DetectObject.SetActive(true);
                        }
                        return;
                    }

                    if (collider.transform.tag == "doorClue" && doorGame)
                    {
                        findDoorClue = true;

                        //點擊BTN可以調查線索．開啟ＡＲ調查，openAR = true
                        ARSystem.doorClue(collider.gameObject);

                        //DetectBtn.text = "調查線索";
                        btnImage.sprite = searchImage;
                        DetectObject.SetActive(true);
                        return;
                    }
                }
            }
        }
        //遠離調查物後,將他變成false,靠近在變為true
        startAltarGame = false;
        findDoorClue = false;
        DetectObject.SetActive(false);
    }

    public void templeBtnController()
    {

        if (altarGame)
        {
            //初始調查
            if (!findAltarGame && !startAltarGame)
            {
                //跳出旁白
                summerGameController.openNarrationSystem();
                findAltarGame = true;
                DetectObject.SetActive(false);
                //顯示線索調查
                return;
            }

            //調查AR線索
            if (findAltarGame && !startAltarGame)
            {
                //startAltarGame = true;
                //調查AR
                PlayerPrefs.SetInt("findAltarGame", 1);
                PlayerPrefs.Save();
                SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
                switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
                return;
            }

            //調查音樂祭壇
            if (findAltarGame && startAltarGame)
            {
                //開啟祭壇畫面
                touchCanva.SetActive(false);
                DetectObject.SetActive(false);
                MusicAltarCanva.SetActive(true);
                return;
            }
        }
        if (!altarGame)
        {

            //根據不同狀況判斷按紐要執行的程序
            if (!doorGame && !findDoorClue && !startDoorGame)
            {
                //開啟提示對話，跟他們説要去尋找線索
                print("旁白提示");
                summerGameController.openNarrationSystem();
                doorGame = true;
                DetectObject.SetActive(false);
                return;
            }

            if (doorGame && findDoorClue)
            {
                //開啟AR
                //紀錄可開啟大門變數之變化
                startDoorGame = true;

                PlayerPrefs.SetInt("startDoorGame", 1);
                PlayerPrefs.Save();

                //切換場景
                SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
                switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
                return;
            }

            if (startDoorGame)
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
        if (cookieGameController.finishCookieGame)
        {
            templeGameStart = true;
            saveTempleGame();
        }
        else
        {
            templeGameStart = false;
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
            //開啟音樂大門
            //templeDoor.transform.rotation = Quaternion.Euler(0, 90, 0);
            //調整活動進度
            altarGame = true;

            PlayerPrefs.SetInt("starMusicAltar", 1);
            PlayerPrefs.Save();
        }
    }

    public void saveTempleGame()
    {
        PlayerPrefs.SetInt("templeGameStart", 1);
        PlayerPrefs.Save();
    }
}
