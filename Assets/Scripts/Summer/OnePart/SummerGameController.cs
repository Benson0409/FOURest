using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerGameController : MonoBehaviour
{
    //開啟旁白
    //控制遊戲進行
    public narrationSystem narration;
    public GameObject narrationPanel;
    public GameObject narrationSystem;

    [Header("事件監聽")]
    public VoidEventSo ResetDataEventSo;

    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("玩家初始位置")]
    public GameObject player;
    public Transform initialPlayerLocate;

    //在遊戲一開始得時候啟動旁白
    private void Start()
    {

        if (PlayerPrefs.GetInt("narrationFirstSave") == 0)
        {

            saveSummerGamestart();
            //開啟對話
            openNarrationSystem();
        }
    }

    private void saveSummerGamestart()
    {
        //startNarration = true;
        PlayerPrefs.SetInt("narrationFirstSave", 1);
        PlayerPrefs.Save();

    }

    //開啟旁白
    public void openNarrationSystem()
    {
        //避免有多餘的旁白出現
        if (PlayerPrefs.GetInt("startFindCrystalBall") == 1)
        {
            return;
        }

        narration.openNarration = true;
        narration.startDialogue = true;
        narrationSystem.SetActive(true);
        narrationPanel.SetActive(true);
    }

    //將資料清除，並將人物移動到最初始的位置
    public void clearGameInformation()
    {
        //要優先執行 因為要物體在enable狀態下才可以進行狀態的監聽
        ResetDataEventSo.RaiseEvent();

        player.transform.position = initialPlayerLocate.position;
        player.transform.rotation = initialPlayerLocate.rotation;


        PlayerPrefs.DeleteAll();

        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
        //print("資料重開");
    }

}
