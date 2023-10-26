using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    //根據不同的遊戲進程來決定背包要呈現哪些資訊
    //需要顯示背包實在顯示
    public GameObject inventoryPanel;
    [Header("Item1")]
    public GameObject item1;
    public Image item1Image;
    public Text item1Text;
    public Text informationText1;

    [Header("Item2")]
    public GameObject item2;
    public Image item2Image;
    public Text item2Text;
    public Text informationText2;

    [Header("Item3")]
    public GameObject item3;
    public Image item3Image;
    public Text item3Text;
    public Text informationText3;

    [Header("圖片樣式")]
    public Sprite puzzleSprite;
    public Sprite cookieSprite;
    public Sprite mussicSheetSprite;
    public Sprite mirrorSprite;
    public Sprite crystalBallSprite;

    [Header("關卡判斷")]
    public PuzzleGameController puzzleGameController;
    public cookieGameController cookieGameController;


    private void Update()
    {
        //成功召喚水仙子
        if (PlayerPrefs.GetInt("finishColorGame") == 1)
        {
            inventoryPanel.SetActive(false);
            return;
        }

        //找到水晶球
        if (PlayerPrefs.GetInt("collectCrystalBall") == 1)
        {

            inventoryPanel.SetActive(true);

            item2.SetActive(true);
            item2Image.sprite = crystalBallSprite;
            item2Text.text = "1";
            informationText1.text = "水晶球";

            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "1";
            informationText1.text = "音樂樂譜";
            return;
        }

        //使用調色盤
        if (PlayerPrefs.GetInt("startFindCrystalBall") == 1)
        {
            inventoryPanel.SetActive(true);
            
            item2.SetActive(true);
            item2Image.sprite = crystalBallSprite;
            item2Text.text = "0";
            informationText2.text = "根據線索找到水晶球";

            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "1";
            informationText1.text = "音樂樂譜";
            return;
        }

        //色彩分析器完成
        if (PlayerPrefs.GetInt("finishRotate") == 1)
        {
            
            inventoryPanel.SetActive(true);

            item2.SetActive(true);
            item2Image.sprite = mirrorSprite;
            item2Text.text = "1";
            informationText2.text = "三稜鏡，用來開啟調色盤";

            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "1";
            informationText1.text = "音樂樂譜";
            return;
        }

        //神廟遊戲結束
        if (PlayerPrefs.GetInt("startColorGame") == 1)
        {
            inventoryPanel.SetActive(true);

            item2.SetActive(true);
            item2Image.sprite = mirrorSprite;
            item2Text.text = "0";
            informationText2.text = "找到三稜鏡，修理色彩分析器";

            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "1";
            informationText1.text = "音樂樂譜";
            return;
        }

        if (PlayerPrefs.GetInt("finishAltarGame") == 1)
        {
            inventoryPanel.SetActive(true);
            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "1";
            informationText1.text = "音樂樂譜";
            return;
        }

        //餅乾遊戲結束
        if (PlayerPrefs.GetInt("finishCookieGame") == 1)
        {
            inventoryPanel.SetActive(true);
            item1.SetActive(true);
            item1Image.sprite = mussicSheetSprite;
            item1Text.text = "0";
            informationText1.text = "尋找音樂樂譜，完成莉莉絲任務";
            return;
        }

        //拼圖遊戲結束
        if (PlayerPrefs.GetInt("puzzleGameOver") == 1)
        {
            //代表餅乾已經找齊，已經用餅乾呼喚出莉莉絲，這時候可以將背包系統關閉
            if(cookieGameController.findCookieCount == 3)
            {
                inventoryPanel.SetActive(false);
                item1.SetActive(false);
                return;
            }
            
            inventoryPanel.SetActive(true);
            item1.SetActive(true);
            item1Image.sprite = cookieSprite;
            item1Text.text = cookieGameController.findCookieCount.ToString();
            informationText1.text = "找尋不同餅乾碎片";
            return;
        }

        //開始進行遊戲在開啟
        //並紀錄拼圖碎片數量
        if (puzzleGameController.findPuzzle)
        {
            inventoryPanel.SetActive(true);

            if (puzzleGameController.openPuzzleGame)
            {
                inventoryPanel.SetActive(false);
            }
            
            item1.SetActive(true);
            item1Image.sprite = puzzleSprite;
            item1Text.text = puzzleGameController.puzzleCount.ToString();
            informationText1.text = "收集告示牌碎片，修補告示牌";
            return;
        }



        
    }
}
