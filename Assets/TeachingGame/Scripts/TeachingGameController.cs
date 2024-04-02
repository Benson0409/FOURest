using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class TeachingGameController : MonoBehaviour
{
    //任務流程
    //1.教學移動視角與人物移動（學會之後讓他們可以去尋找智者講話）
    //2.到智者旁邊時，介紹BTN的功能（讓他們知道要如何跟敵人去對話）
    //3.對話完之要去尋找仿在地板上的鑰匙（讓他們知道如何撿起物品）
    //4.收集完鑰匙之後即可前往大門（介紹靠近不同物體右下會有BTN顯示可以互動）
    //5.按下BTN後進入到AR場景，讓玩家生成大門並點擊門即可進入到夏天關卡中
    //來顯示當前的任務目標是什麼 讓他們沒有看的對話也可以知道
    public CameraController cameraController;

    [Header("偵測控制")]
    public GameObject player;
    public float radius;
    public RectTransform takeRange;

    // [Header("任務文字")]
    // public Text missionText;
    [Header("教學通知")]
    public GameObject teachingPanel;
    public Text teachingTitle;
    [Header("教學變量")]
    public Transform target;
    public GameObject viewController;
    public GameObject dialogueBtn;
    //利用新手訓練的按鈕來觸發
    [HideInInspector] public bool isStartDialogue;
    public GameObject findCrystalBtn;
    public GameObject magnifier;
    public GameObject openDoorBtn;

    [Header("旁白系統")]
    public narrationSystem narration;
    public NarrationDataSo narrationData;
    public GameObject narrationPanel;
    public GameObject narrationSystem;
    //----------Input system---------
    //input system 的利用
    private Player playerInput;

    [Header("教學關卡數據")]
    public TeachingGameDataSo teachingGameData;

    [Header("場景淡入淡出")]
    public SwitchScenes scenesCanvaPrefabs;

    void Awake()
    {
        Application.targetFrameRate = 300;
        playerInput = new Player();
        if (!teachingGameData.isFirst && !narrationData.isPlayAwake)
        {
            openNarrationSystem(0);
            teachingGameData.isFirst = true;
            narrationData.isPlayAwake = true;
        }
        if (teachingGameData.isPick)
        {
            magnifier.SetActive(false);
        }
        if (teachingGameData.isViewTip)
        {
            viewController.SetActive(true);
        }
    }
    //腳本啟用的時候，接受玩家的輸入
    private void OnEnable()
    {
        playerInput.Enable();
    }

    //腳本禁用時，禁用玩家輸入
    private void OnDisable()
    {
        playerInput.Disable();
    }


    void Update()
    {
        if (teachingPanel.activeInHierarchy)
        {
            PlayerController.uiDisplay = true;
        }
        else
        {
            PlayerController.uiDisplay = false;
        }
        // if (teachingGameData.isPick && !teachingGameData.isTeachingGameOver)
        // {
        //     missionText.text = "前去解鎖大門";
        // }
        GoToNewWorld();
        //偵測物體
        FindCrystalAR();
        //教學關卡
        TeachingBtn();
        TeachingView();
        TeachingMove();
    }
    private void GoToNewWorld()
    {
        //查看完水晶要在回到世界來讓玩家前往開啟大門
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

        //如果player的狀態屬於 false 就不執行偵測的動作 
        if (player.activeInHierarchy == false)
        {
            return;
        }

        foreach (Collider collider in colliders)
        {
            if (teachingGameData.isARPick)
            {
                if (collider.gameObject.tag == "door")
                {
                    openDoorBtn.SetActive(true);
                    return;
                }

            }
            openDoorBtn.SetActive(false);
        }
    }
    private void FindCrystalAR()
    {
        //查看完水晶要在回到世界來讓玩家前往開啟大門
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

        //如果player的狀態屬於 false 就不執行偵測的動作 
        if (player.activeInHierarchy == false)
        {
            return;
        }

        foreach (Collider collider in colliders)
        {
            TeachingPick(collider);
            if (teachingGameData.isPick)
            {
                if (collider.gameObject.tag == "crystal")
                {
                    findCrystalBtn.SetActive(true);
                    return;
                }

            }
            findCrystalBtn.SetActive(false);
        }
    }


    private void TeachingPick(Collider collider)
    {
        if (teachingGameData.isBtnActive && !teachingGameData.isPick)
        {
            // missionText.text = "靠近並點擊物品即可收集";
            if (collider.gameObject.name == "magnifier")
            {
                //手指觸控
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
                                    if (collider.gameObject.tag == "magnifier")
                                    {
                                        GameObject doorKey = collider.gameObject;
                                        doorKey.SetActive(false);
                                        // missionText.text = "前往大門解鎖！";
                                        teachingGameData.isPick = true;
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }
    }

    private void TeachingBtn()
    {

        // if (!teachingGameData.isBtnActive && teachingGameData.isView)
        // {
        //     missionText.text = "與人物互動";
        // }

        if (!teachingGameData.isBtnActive && isStartDialogue)
        {
            teachingGameData.isBtnActive = true;
        }

        //對話按鈕出來後，教學說如果有可互動之物件可以點擊右下方BTN來互動
        if (teachingGameData.isView && dialogueBtn.activeInHierarchy && !teachingGameData.isBtnTip)
        {
            teachingPanel.SetActive(true);
            teachingGameData.isBtnTip = true;
            teachingTitle.text = "人物互動介紹";
        }
    }

    private void TeachingView()
    {
        // if (!teachingGameData.isView && teachingGameData.isMove)
        // {
        //     missionText.text = "讓人物的視角轉動並尋找周圍";
        // }

        if (cameraController.canLook && !teachingGameData.isView)
        {
            teachingGameData.isView = true;
            return;
        }

        if (teachingGameData.isMove && !teachingGameData.isViewTip)
        {
            teachingPanel.SetActive(true);
            teachingGameData.isViewTip = true;
            viewController.SetActive(true);

            teachingTitle.text = "讓人物的視角轉動";
        }
    }

    private void TeachingMove()
    {
        //第一次嘗試移動人物時候跳出介紹資訊
        Vector3 moveInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        // if (!teachingGameData.isMove)
        // {
        //     missionText.text = "嘗試移動";
        // }
        //走到指定位置會利用Trigger來讓判斷結束,並開啟下一階段
        //彈出教學關卡
        if (moveInput.magnitude > 0.1f && !teachingGameData.isMoveTip)
        {
            teachingPanel.SetActive(true);

            teachingTitle.text = "移動方向鍵讓人物移動";
            teachingGameData.isMoveTip = true;
        }
    }

    public void openNarrationSystem(int playNarrationIndex)
    {
        narration.narrationTextAsset = narrationData.narrationTextAsset[playNarrationIndex];
        narration.openNarration = true;
        narration.startDialogue = true;
        narrationSystem.SetActive(true);
        narrationPanel.SetActive(true);
    }

    public void startDialogue()
    {
        isStartDialogue = true;
    }

    public void OpenDoorBtn()
    {
        //開啟AR生成大門
        teachingGameData.isTeachingGameOver = true;
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }
    //去開啟AR大門
    public void FindCrystalBtn()
    {
        //開啟AR生成大門
        teachingGameData.isARPick = true;
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARScene"));
    }
    public void StartSummerGame()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }

    public void BackMainMenu()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Menu"));
    }

}
