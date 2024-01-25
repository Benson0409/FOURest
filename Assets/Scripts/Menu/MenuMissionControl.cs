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
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;

    [Header("場景淡入淡出")]
    public SwitchScenes scenesCanvaPrefabs;

    private void Update()
    {
        SummerGameMissionTip();
    }

    private void SummerGameMissionTip()
    {
        //成功召喚水仙子
        if (colorGameData.colorGameOver)
        {
            misiionText.text = "尋找水仙子並與它對話";
            return;
        }

        //找到水晶球
        if (colorGameData.isFindCrystalBall)
        {
            misiionText.text = "尋找莉莉絲並與它對話";
            return;

        }

        //使用調色盤
        if (colorGameData.startFindCrystalBall)
        {
            misiionText.text = "點擊水晶球完成收集";
            return;
        }

        //色彩分析器完成
        if (colorGameData.isRotate)
        {
            misiionText.text = "前往色彩分析器，尋找藍色藥水、黃色圓球、紅色火焰";
            return;

        }

        //神廟遊戲結束
        if (colorGameData.startColorGame)
        {
            misiionText.text = "前往三稜鏡，並將三者都轉於正確位置";
            return;
        }

        if (templeGameData.finishMusicGame)
        {
            misiionText.text = "到音樂神殿外尋找莉莉絲";
            return;
        }

        if (templeGameData.startMusicGame)
        {
            misiionText.text = "找尋牆壁附近線索，開啟音樂寶箱";
            return;
        }

        if (templeGameData.startDoorGame)
        {
            misiionText.text = "找尋附近線索，破解大門密碼";
            return;
        }

        //餅乾遊戲結束
        if (cookieGameData.cookieGameOver)
        {
            misiionText.text = "走上樓梯到達神廟，找尋音樂樂譜";
            return;
        }

        //拼圖遊戲結束
        if (cookieGameData.startCookieGame)
        {
            //代表餅乾已經找齊，已經用餅乾呼喚出莉莉絲，這時候可以將背包系統關閉
            if (cookieGameData.findCookieCount == 3)
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

    public void GoToCookieGame()
    {
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();
        //拼圖遊戲結束
        puzzleGameData.isPlayAnim = true;
        puzzleGameData.puzzleGameOver = true;
        puzzleGameData.isFindPuzzle = true;
        //開始餅乾遊戲
        cookieGameData.startCookieGame = true;
        ReLoadGame();
    }
    public void GoToTempleGame()
    {
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();
        GoToCookieGame();
        //餅乾遊戲結束
        cookieGameData.isFindCookie = true;
        cookieGameData.findCookieCount = 3;
        cookieGameData.cookie1Field = true;
        cookieGameData.cookie2Field = true;
        cookieGameData.cookie3Field = true;
        ReLoadGame();
    }
    public void GoToColorGame()
    {
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();
        cookieGameData.cookieGameOver = true;
        GoToTempleGame();
        templeGameData.finishMusicGame = true;
        templeGameData.startMusicGame = true;
        templeGameData.startTempleGame = true;
        ReLoadGame();
    }
    public void ReLoadGame()
    {
        this.gameObject.SetActive(false);
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }


}
