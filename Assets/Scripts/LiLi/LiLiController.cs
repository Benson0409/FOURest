using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class LiLiController : MonoBehaviour
{
    //控制莉莉絲位置
    public GameObject initialLiLi;
    public GameObject templeLiLi;
    public GameObject finalLiLi;

    [Header("Data")]
    public LiLiDataSo liLiData;
    private int currentPositionIndex;

    [Header("遊戲數據")]
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    [Header("事件監聽")]
    public VoidEventSo LiliChangeEventSo;
    public VoidEventSo ResetDataEventSo;

    void Awake()
    {
        currentPositionIndex = liLiData.liliPositionIndex;
        LiliPos();
    }

    void OnEnable()
    {
        LiliChangeEventSo.OnEventRaised += LiliPositionChange;
        ResetDataEventSo.OnEventRaised += ResetLiliData;
    }


    void OnDisable()
    {
        LiliChangeEventSo.OnEventRaised -= LiliPositionChange;
        ResetDataEventSo.OnEventRaised -= ResetLiliData;
    }


    private void LiliPositionChange()
    {
        print(123);
        //currentPositionIndex++;
        if (cookieGameData.isFindCookie && !templeGameData.startTempleGame)
        {
            currentPositionIndex = 1;
        }
        else if (templeGameData.finishMusicGame && !colorGameData.startColorGame)
        {
            currentPositionIndex = 2;
        }
        else
        {
            currentPositionIndex = 3;
        }
        liLiData.liliPositionIndex = currentPositionIndex;
        LiliPos();
    }

    private void LiliPos()
    {
        switch (currentPositionIndex)
        {
            case 1:
                initialLiLi.SetActive(true);
                templeLiLi.SetActive(false);
                finalLiLi.SetActive(false);
                break;
            case 2:
                initialLiLi.SetActive(false);
                templeLiLi.SetActive(true);
                finalLiLi.SetActive(false);
                break;
            case 3:
                initialLiLi.SetActive(false);
                templeLiLi.SetActive(false);
                finalLiLi.SetActive(true);
                break;
        }
    }

    //清除資料
    private void ResetLiliData()
    {
        liLiData.liliPositionIndex = 0;
    }
}
