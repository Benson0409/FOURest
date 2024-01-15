using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGameController : MonoBehaviour
{
    //旁白系統
    private SummerGameController summerGameController;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("UI控制")]
    public GameObject touchCanva;

    [Header("顏色關卡設置")]
    public bool startColorGame;

    [Header("偵測控制")]
    public GameObject player;
    public float radius;

    [Header("三色鏡變量控制")]

    public bool startColorMirrorGame;

    [Header("調色盤變量控制")]
    public bool startFilterGame;
    //按下按鈕後才可以去尋找水晶球
    public bool startFindCrystalBall;
    //是否收集到水晶球
    public bool isFindCrystalBall;

    [Header("按鈕控制")]
    public GameObject DetectObject;
    public Image btnImage;
    public Sprite openImage;
    public Sprite searchImage;

    [Header("顏色關卡物體")]
    public GameObject colorFiliter;
    public GameObject colorFiliterCanva;
    //水晶球架子
    public GameObject crystalBall1;
    public GameObject crystalBall;

    [Header("收集水晶球範圍")]
    public RectTransform takeRange;

    [Header("三色鏡旋轉物體")]
    public GameObject colorMirrorRotate1;
    public GameObject colorMirrorRotate2;
    public GameObject colorMirrorRotate3;

    [Header("水仙子")]
    public GameObject water;
    public GameObject Castle;

    [Header("莉莉絲位置")]
    public VoidEventSo LiliChangeEventSo;

    [Header("莉莉絲對話腳本")]
    public DialogueDataSo liliDialogueData;
    public GameObject dialoguePanel;

    [Header("遊戲數據")]
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;
    private bool isClear;

    void OnEnable()
    {
        ResetDataEventSo.OnEventRaised += ResetColorGameData;
    }



    void OnDisable()
    {
        ResetDataEventSo.OnEventRaised -= ResetColorGameData;
    }

    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();
        ReadColorGameData();

        if (colorGameData.colorGameOver)
        {
            water.SetActive(true);
            Castle.SetActive(true);
            return;
        }



        if (startColorGame)
        {

            if (startColorMirrorGame)
            {
                //代表已經開始進行旋轉三色鏡
                if (PlayerPrefs.GetInt("firstRotate") == 1)
                {
                    loadColorMirrorRotate1();
                    loadColorMirrorRotate2();
                    loadColorMirrorRotate3();
                }
            }
        }

        //------------------------------------------------
        //以收集完成水晶球
        if (isFindCrystalBall)
        {
            water.SetActive(true);
            crystalBall.SetActive(false);
            return;
        }
        //------------------------------------------------

        //水晶球顯示
        if (startFindCrystalBall)
        {
            if (!colorGameData.isPlayCrystalBallAnim)
            {
                //說明點擊收集水晶球
                summerGameController.openNarrationSystem();
                colorGameData.isPlayCrystalBallAnim = true;
            }
            crystalBall1.SetActive(true);
            return;
        }
        //------------------------------------------------

        //完成三色鏡旋轉獲得道具
        if (colorGameData.isRotate && startFilterGame)
        {
            //調色盤控制器顯示
            colorFiliter.SetActive(true);
            if (!colorGameData.isPlayFiliterAnim)
            {

                //說明調色盤,並請他們根據線索去尋找
                summerGameController.openNarrationSystem();
                colorGameData.isPlayFiliterAnim = true;
            }
        }

    }


    private void Update()
    {
        //做最後設定
        if (colorGameData.colorGameOver)
        {
            return;
        }

        if (!startColorGame)
        {
            return;
        }

        SaveColorGameData();

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

        //如果player的狀態屬於 false 就不執行偵測的動作
        if (player.activeInHierarchy == false)
        {
            return;
        }

        if (isFindCrystalBall)
        {
            //要讓結束在讓水仙子出現,並對話就可以結束第一關
            //水仙子要出現，並且要靠近水仙子才能講話
            if (liliDialogueData.repeatText && !dialoguePanel.activeInHierarchy)
            {
                print("水仙子出現");
                colorGameData.colorGameOver = true;
                PlayAnim();
                return;
            }
        }

        if (startFilterGame)
        {
            foreach (Collider collider in colliders)
            {
                //用手指點擊水晶球來收集
                if (startFindCrystalBall)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        Vector2 touchPosition = touch.position;
                        if (!colorFiliterCanva.activeInHierarchy)
                        {
                            if (RectTransformUtility.RectangleContainsScreenPoint(takeRange, touchPosition))
                            {
                                switch (touch.phase)
                                {
                                    case TouchPhase.Began:

                                        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                                        RaycastHit hit;

                                        if (Physics.Raycast(ray, out hit))
                                        {
                                            if (collider.gameObject.tag == "crystalBall")
                                            {
                                                //莉莉絲出現
                                                LiliChangeEventSo.RaiseEvent();

                                                //收集水晶球
                                                isFindCrystalBall = true;
                                                crystalBall.SetActive(false);
                                                return;
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }

                //色彩分析器開啟時將Btn關閉
                if (colorFiliterCanva.activeInHierarchy)
                {
                    DetectObject.SetActive(false);
                    return;
                }

                //獲得道具後，靠近colorFiliter使用道具，尋找水晶球的碎片
                if (collider.tag == "colorFiliter")
                {
                    //準備開啟顏色濾鏡尋找線索
                    DetectObject.SetActive(true);
                    btnImage.sprite = openImage;
                    return;
                }
            }
            DetectObject.SetActive(false);
            return;
        }

        if (startColorGame && !startFilterGame)
        {

            foreach (Collider collider in colliders)
            {
                if (collider.tag == "colorMirror" && startColorMirrorGame)
                {
                    ARSystem.colorMirror(collider.gameObject);
                    DetectObject.SetActive(true);
                    btnImage.sprite = searchImage;
                    return;
                }

                if (collider.tag == "colorPlane" && !startColorMirrorGame)
                {
                    //靠近直接觸發旁白
                    //不在利用按鈕來運作
                    colorGameExplain();
                    return;
                }
            }
        }
        DetectObject.SetActive(false);
    }



    public void colorGameExplain()
    {
        summerGameController.openNarrationSystem();
        startColorMirrorGame = true;
    }

    public void colorGameBtnController()
    {
        if (startFilterGame)
        {
            DetectObject.SetActive(false);
            colorFiliterCanva.SetActive(true);
            touchCanva.SetActive(false);
            return;
        }

        if (startColorMirrorGame)
        {
            //進入AR環境調整三色鏡的旋轉角度
            //思考如何兩者連動
            DetectObject.SetActive(false);
            SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
            switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
            return;
        }
    }

    public void startGame()
    {

        if (templeGameData.templeGameOver)
        {
            colorGameData.startColorGame = true;
            startColorGame = true;

        }
    }


    //紀錄三色鏡角度旋轉
    public void loadColorMirrorRotate1()
    {
        // 从PlayerPrefs获取Y轴旋转值
        float newYRotation = PlayerPrefs.GetFloat("colorMirrorRotate1");
        // 创建一个新的Quaternion，只改变Y轴的旋转
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newYRotation, transform.rotation.eulerAngles.z);
        // 应用新的旋转到物体的transform.rotation属性
        colorMirrorRotate1.transform.rotation = newRotation;
    }

    public void loadColorMirrorRotate2()
    {
        // 从PlayerPrefs获取Y轴旋转值
        float newYRotation = PlayerPrefs.GetFloat("colorMirrorRotate2");
        // 创建一个新的Quaternion，只改变Y轴的旋转
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newYRotation, transform.rotation.eulerAngles.z);
        // 应用新的旋转到物体的transform.rotation属性
        colorMirrorRotate2.transform.rotation = newRotation;
    }

    public void loadColorMirrorRotate3()
    {
        // 从PlayerPrefs获取Y轴旋转值
        float newYRotation = PlayerPrefs.GetFloat("colorMirrorRotate3");
        // 创建一个新的Quaternion，只改变Y轴的旋转
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newYRotation, transform.rotation.eulerAngles.z);
        // 应用新的旋转到物体的transform.rotation属性
        colorMirrorRotate3.transform.rotation = newRotation;
    }

    private void PlayAnim()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }

    private void ReadColorGameData()
    {
        //讀取資料
        startColorGame = colorGameData.startColorGame;
        startColorMirrorGame = colorGameData.startColorMirrorGame;
        startFilterGame = colorGameData.startFilterGame;
        startFindCrystalBall = colorGameData.startFindCrystalBall;
        isFindCrystalBall = colorGameData.isFindCrystalBall;
    }

    private void SaveColorGameData()
    {
        if (!isClear)
        {
            //存取資料
            colorGameData.startColorGame = startColorGame;
            colorGameData.startColorMirrorGame = startColorMirrorGame;
            colorGameData.startFilterGame = startFilterGame;
            colorGameData.isFindCrystalBall = isFindCrystalBall;
        }
    }

    private void ResetColorGameData()
    {
        isClear = true;
        colorGameData.startColorGame = false;
        colorGameData.colorGameOver = false;
        colorGameData.startColorMirrorGame = false;
        colorGameData.isRotate = false;
        colorGameData.startFilterGame = false;
        colorGameData.isPlayFiliterAnim = false;
        colorGameData.startFindCrystalBall = false;
        colorGameData.isFindCrystalBall = false;
        colorGameData.isPlayCrystalBallAnim = false;
    }
}
