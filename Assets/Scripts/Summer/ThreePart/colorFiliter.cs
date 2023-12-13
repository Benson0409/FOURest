using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorFiliter : MonoBehaviour
{
    //每一個按鈕都呈現不同的內容，玩家需要在每一個按鈕當中去尋找水晶球的碎片
    //收集完三片後關閉色彩分析器，並將色彩分析器消失，水晶球慢慢浮現
    public GameObject touchCanva;
    public ColorGameController colorGameController;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    // [Header("顏色遮罩")]
    // public Image colorFiliterPanel;

    [Header("各區塊顯示圖片")]
    public GameObject block1;
    public GameObject block2;
    public GameObject block3;

    [Header("發現各區塊顯示圖片")]
    public Sprite block1Image;
    public Sprite block2Image;
    public Sprite block3Image;

    [Header("各區塊水晶球碎片")]
    public Image crystalBallChip1;
    public Image crystalBallChip2;
    public Image crystalBallChip3;

    [Header("禁用BTN")]
    public Button findCrystalBallChip1;
    public Button findCrystalBallChip2;
    public Button findCrystalBallChip3;

    [Header("收集區域圖片")]
    private int count = 0;
    public Image blueBlock;
    public Image yellowBlock;
    public Image redBlock;
    public Sprite blueImage;
    public Sprite yellowImage;
    public Sprite redImage;

    private void Awake()
    {
        block1.SetActive(false);
        block2.SetActive(false);
        block3.SetActive(false);
    }

    void Update()
    {
        if (count >= 3)
        {
            //當物品都收集到後 關閉面板 進入動畫 水晶球出現
            //可以開始尋找水晶球
            StartCoroutine(StartCountdown());
            // PlayerPrefs.SetInt("startFindCrystalBall", 1);
            // PlayerPrefs.Save();
            // colorGameController.startFindCrystalBall = true;
            // colorGameController.crystalBall.SetActive(true);
            // this.gameObject.SetActive(false);
            // touchCanva.SetActive(true);
            // switchAnimScene();
        }
    }
    public void blueBtn()
    {
        block1.SetActive(true);
        block2.SetActive(false);
        block3.SetActive(false);
    }
    //找到水晶球碎片後的處理
    public void findBlock1CrystalBall()
    {
        findCrystalBallChip1.interactable = false;
        block1.GetComponent<Image>().sprite = block1Image;
        blueBlock.sprite = blueImage;
        count++;
    }

    public void yellowBtn()
    {
        block1.SetActive(false);
        block2.SetActive(true);
        block3.SetActive(false);
    }

    //找到水晶球碎片後的處理
    public void findBlock2CrystalBall()
    {
        findCrystalBallChip2.interactable = false;
        block2.GetComponent<Image>().sprite = block2Image;
        yellowBlock.sprite = yellowImage;
        count++;
    }
    public void redBtn()
    {
        block1.SetActive(false);
        block2.SetActive(false);
        block3.SetActive(true);
    }

    //找到水晶球碎片後的處理
    public void findBlock3CrystalBall()
    {
        findCrystalBallChip3.interactable = false;
        block3.GetComponent<Image>().sprite = block3Image;
        redBlock.sprite = redImage;
        count++;
    }

    public void closeBtn()
    {
        touchCanva.SetActive(true);
        this.gameObject.SetActive(false);
    }

    IEnumerator StartCountdown()
    {
        // 每一秒減去時間
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("startFindCrystalBall", 1);
        PlayerPrefs.Save();
        colorGameController.startFindCrystalBall = true;
        colorGameController.crystalBall.SetActive(true);
        this.gameObject.SetActive(false);
        touchCanva.SetActive(true);
        switchAnimScene();
    }

    public void switchAnimScene()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }

}
