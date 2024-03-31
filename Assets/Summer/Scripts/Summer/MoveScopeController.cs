using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScopeController : MonoBehaviour
{
    //根據每一關的結束來開啟區域的限制
    public PuzzleGameDataSo puzzleGameData;
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    //讓玩家不會到處亂跑
    [Header("餅乾區域")]
    public GameObject startCookieScope;
    public GameObject overCookieScope;
    [Header("神廟區域")]
    public GameObject templeScope;
    public GameObject startTempleScope;
    public GameObject overTempleScope;
    [Header("色彩區域")]
    public GameObject colorScope;
    [Header("最終區域")]
    public GameObject finalScope;

    private void Update()
    {
        //成功召喚水仙子
        if (colorGameData.colorGameOver)
        {
            colorScope.SetActive(true);
            finalScope.SetActive(false);
        }

        //神廟遊戲結束
        //開啟色彩區域
        if (colorGameData.startColorGame)
        {
            colorScope.SetActive(false);
        }

        //開啟連通到下一個區域的限制
        if (templeGameData.templeGameOver)
        {
            overTempleScope.SetActive(false);
            startTempleScope.SetActive(true);
            return;
        }

        //餅乾遊戲結束
        //開啟神廟區域
        if (cookieGameData.cookieGameOver)
        {
            templeScope.SetActive(false);
            overTempleScope.SetActive(true);
        }

        //拼圖遊戲結束
        //開啟餅乾區域
        if (puzzleGameData.puzzleGameOver)
        {
            startCookieScope.SetActive(false);
        }

    }
}
