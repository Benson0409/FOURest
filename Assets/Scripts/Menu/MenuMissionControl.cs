using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMissionControl : MonoBehaviour
{
    //來顯示當前的任務目標是什麼 讓他們沒有看的對話也可以知道
    [Header("任務文字")]
    public Text misiionText;
    [Header("關卡判斷")]
    public PuzzleGameDataSo puzzleGameData;
    public cookieGameController cookieGameController;


    private void Update()
    {
        //成功召喚水仙子
        if (PlayerPrefs.GetInt("finishColorGame") == 1)
        {
            misiionText.text = "尋找水仙子並與它對話";
            return;
        }

        //找到水晶球
        if (PlayerPrefs.GetInt("collectCrystalBall") == 1)
        {
            misiionText.text = "尋找莉莉絲並與它對話";
            return;

        }

        //使用調色盤
        if (PlayerPrefs.GetInt("startFindCrystalBall") == 1)
        {
            misiionText.text = "點擊水晶球完成收集";
            return;
        }

        //色彩分析器完成
        if (PlayerPrefs.GetInt("finishRotate") == 1)
        {
            misiionText.text = "前往色彩分析器，尋找藍色藥水、黃色圓球、紅色火焰";
            return;

        }

        //神廟遊戲結束
        if (PlayerPrefs.GetInt("startColorGame") == 1)
        {
            misiionText.text = "前往三稜鏡，並將三者都轉於正確位置";
            return;
        }

        if (PlayerPrefs.GetInt("templeGameFinish") == 1)
        {
            misiionText.text = "繼續往前移動尋找莉莉絲，並完成最後任務";
            return;
        }
        if (PlayerPrefs.GetInt("finishAltarGame") == 1)
        {
            misiionText.text = "到音樂神殿外尋找莉莉絲";
            return;
        }

        if (PlayerPrefs.GetInt("starMusicAltar") == 1)
        {
            misiionText.text = "找尋牆壁附近線索，開啟音樂寶箱";
            return;
        }

        if (PlayerPrefs.GetInt("startDoorGame") == 1)
        {
            misiionText.text = "找尋附近線索，破解大門密碼";
            return;
        }

        //餅乾遊戲結束
        if (PlayerPrefs.GetInt("finishCookieGame") == 1)
        {
            misiionText.text = "走上樓梯到達神廟，找尋音樂樂譜";
            return;
        }

        //拼圖遊戲結束
        if (PlayerPrefs.GetInt("puzzleGameOver") == 1)
        {
            //代表餅乾已經找齊，已經用餅乾呼喚出莉莉絲，這時候可以將背包系統關閉
            if (cookieGameController.findCookieCount == 3)
            {
                misiionText.text = "找尋莉莉絲，與他進行對話";
                return;
            }
            misiionText.text = "前往告示牌指引方向，找尋草叢中餅乾碎片";
            return;
        }

        //開始進行遊戲在開啟
        //並紀錄拼圖碎片數量
        if (puzzleGameData.isFindPuzzle)
        {
            misiionText.text = "收集告示牌拼圖碎片，收集完成回到告示牌進行修復";
        }
        else
        {
            misiionText.text = "前往告示牌接下第一個任務";
        }
    }
}
