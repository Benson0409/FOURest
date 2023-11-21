using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorGameController : MonoBehaviour
{
    //旁白系統
    private SummerGameController summerGameController;
    public DialogueManager dialogueManager;


    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("UI控制")]
    public GameObject touchCanva;

    [Header("顏色關卡設置")]
    public bool finishColorGame;

    [Header("偵測控制")]
    public GameObject player;
    public float radius;

    [Header("三色鏡變量控制")]
    public bool startColorGame;
    public bool findColorMirror;

    [Header("調色盤變量控制")]
    public bool startFilterGame;
    public bool openColorFiliterCanva;
    //按下按鈕後才可以去尋找水晶球
    public bool startFindCrystalBall;
    //是否收集到水晶球
    public bool collectCrystalBall;

    [Header("按鈕控制")]
    public GameObject DetectObject;
    public Text DetectBtn;

    [Header("顏色關卡物體")]
    public GameObject colorMirror;
    public GameObject colorFiliter;
    public GameObject colorFiliterCanva;
    public GameObject crystalBall;

    [Header("收集水晶球範圍")]
    public RectTransform takeRange;

    [Header("三色鏡旋轉物體")]
    public GameObject colorMirrorRotate1;
    public GameObject colorMirrorRotate2;
    public GameObject colorMirrorRotate3;

    [Header("水仙子")]
    public GameObject water;

    [Header("水晶球旁莉莉絲位置")]
    public Transform LiLi;
    public Transform finalLiLiPosition;

    private void Awake()
    {
        summerGameController = GetComponent<SummerGameController>();

        if (PlayerPrefs.GetInt("finishColorGame") == 1)
        {
            finishColorGame = true;
            return;
        }



        if (PlayerPrefs.GetInt("startColorGame") == 1)
        {
            startColorGame = true;
            colorMirror.SetActive(true);

            if (PlayerPrefs.GetInt("findColorMirror") == 1)
            {
                findColorMirror = true;

                //代表已經開始進行旋轉三色鏡
                if (PlayerPrefs.GetInt("firstRotate") == 1)
                {
                    loadColorMirrorRotate1();
                    loadColorMirrorRotate2();
                    loadColorMirrorRotate3();
                }
            }
            else
            {
                findColorMirror = false;
            }
        }

        else
        {
            startColorGame = false;
        }
        //------------------------------------------------
        //完成三色鏡旋轉獲得道具
        if (PlayerPrefs.GetInt("finishRotate") == 1)
        {
            //開啟調色盤遊戲
            startFilterGame = true;
            colorFiliter.SetActive(true);

            //說明調色盤,並請他們根據線索去尋找
            summerGameController.openNarrationSystem();
        }
        //------------------------------------------------
        if (PlayerPrefs.GetInt("collectCrystalBall") == 1)
        {
            collectCrystalBall = true;
            crystalBall.SetActive(false);
            return;
        }
        //------------------------------------------------
        if (PlayerPrefs.GetInt("startFindCrystalBall") == 1)
        {
            startFindCrystalBall = true;
            crystalBall.SetActive(true);
            return;
        }




    }

    private void Update()
    {
        //做最後設定
        if (finishColorGame)
        {
            return;
        }

        if (!startColorGame)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

        //如果player的狀態屬於 false 就不執行偵測的動作
        if (player.activeInHierarchy == false)
        {
            return;
        }

        if (collectCrystalBall)
        {
            //要讓結束在讓水仙子出現,並對話就可以結束第一關
            //水仙子要出現，並且要靠近水仙子才能講話
            if (PlayerPrefs.GetInt("currentState") == 2 && !dialogueManager.startDialogue)
            {
                print("水仙子出現");
                PlayerPrefs.SetInt("finishColorGame", 1);
                PlayerPrefs.Save();
                water.SetActive(true);
                finishColorGame = true;
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
                                                //莉莉絲移動過來
                                                LiLi.position = finalLiLiPosition.position;
                                                LiLi.rotation = finalLiLiPosition.rotation;

                                                //收集水晶球
                                                crystalBall.SetActive(false);
                                                PlayerPrefs.SetInt("collectCrystalBall", 1);
                                                //任務完成
                                                DialogueData.saveMissionTextState(true);
                                                collectCrystalBall = true;
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

                //獲得道具後，靠近colorFiliter使用道具，開啟色彩濾鏡尋找水晶球的正確位置
                if (collider.tag == "colorFiliter")
                {
                    //準備開啟顏色濾鏡尋找線索
                    DetectObject.SetActive(true);
                    DetectBtn.text = "使用三色鏡";
                    openColorFiliterCanva = true;
                    return;
                }
            }
            DetectObject.SetActive(false);
            return;
        }

        if (startColorGame)
        {

            foreach (Collider collider in colliders)
            {
                if (collider.tag == "colorMirror" && findColorMirror)
                {
                    ARSystem.colorMirror(collider.gameObject);
                    DetectObject.SetActive(true);
                    DetectBtn.text = "調查三色鏡";
                    return;
                }

                if (collider.tag == "colorPlane" && !findColorMirror)
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
        findColorMirror = true;

        PlayerPrefs.SetInt("findColorMirror", 1);
        PlayerPrefs.Save();
    }

    public void colorGameBtnController()
    {
        if (openColorFiliterCanva)
        {
            DetectObject.SetActive(false);
            colorFiliterCanva.SetActive(true);
            touchCanva.SetActive(false);
            //可以開始尋找水晶球
            PlayerPrefs.SetInt("startFindCrystalBall", 1);
            PlayerPrefs.Save();
            startFindCrystalBall = true;
            crystalBall.SetActive(true);
            return;
        }

        if (findColorMirror)
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

        if (PlayerPrefs.GetInt("templeGameFinish") == 1)
        {

            startColorGame = true;
            //儲存設定
            saveColorGame();

            //開啟三色鏡
            colorMirror.SetActive(true);

            //重置任務狀態
            DialogueData.saveMissionTextState(false);
        }
    }

    public void saveColorGame()
    {
        PlayerPrefs.SetInt("startColorGame", 1);
        PlayerPrefs.Save();
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
}
