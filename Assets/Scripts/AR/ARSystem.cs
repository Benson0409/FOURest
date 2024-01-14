using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARSystem : MonoBehaviour
{
    //負責將要生成的物體傳遞給ＡＲ生成場景
    //故此物體不可破壞，避免資料消失
    public static GameObject arObject;
    private static float colorMirrorRotate;

    //當我點擊調查時，會將點擊的物品傳送到ＡＲ場景讓他生成
    //會根據此個關卡的控制器將任務變數+1
    //當餅乾升級完成後，跳動畫並將人物移動到中間，召喚出莉莉絲在進行下一步驟
    public static void cookie1Field(GameObject cookie1)
    {
        arObject = cookie1;
        PlayerPrefs.SetString("cookie", "field1");
        //print(arObject);
    }

    public static void cookie2Field(GameObject cookie2)
    {
        arObject = cookie2;
        PlayerPrefs.SetString("cookie", "field2");
        //print(arObject);
    }

    public static void cookie3Field(GameObject cookie3)
    {
        arObject = cookie3;
        PlayerPrefs.SetString("cookie", "field3");
        //print(arObject);
    }

    //大門AR開啟
    //目前先用一個探索當做線索，後面需要增加再來改變
    public static void doorClue(GameObject doorClue1)
    {
        arObject = doorClue1;
        PlayerPrefs.SetString("doorClue", "doorClue1");
        //print(arObject);
    }


    public static void musicAltarClue(GameObject musicAltarClue1)
    {
        arObject = musicAltarClue1;
        PlayerPrefs.SetString("musicAltarClue", "musicAltarClue1");
        PlayerPrefs.Save();
    }

    public static void colorMirror(GameObject colorMirror1)
    {
        arObject = colorMirror1;
        PlayerPrefs.SetString("colorMirror", "colorMirror");
        PlayerPrefs.Save();
    }

    public static void saveColorMirrorRotate1(GameObject colorMirrorRotate1)
    {
        colorMirrorRotate = colorMirrorRotate1.transform.eulerAngles.y;
        //有儲存的話在進行讀取資料
        PlayerPrefs.SetInt("firstRotate", 1);
        PlayerPrefs.SetFloat("colorMirrorRotate1", colorMirrorRotate);
        PlayerPrefs.Save();
    }

    public static void saveColorMirrorRotate2(GameObject colorMirrorRotate2)
    {
        colorMirrorRotate = colorMirrorRotate2.transform.eulerAngles.y;
        //有儲存的話在進行讀取資料
        PlayerPrefs.SetInt("firstRotate", 1);
        PlayerPrefs.SetFloat("colorMirrorRotate2", colorMirrorRotate);
        PlayerPrefs.Save();
    }

    public static void saveColorMirrorRotate3(GameObject colorMirrorRotate3)
    {
        colorMirrorRotate = colorMirrorRotate3.transform.eulerAngles.y;
        //有儲存的話在進行讀取資料
        PlayerPrefs.SetInt("firstRotate", 1);
        PlayerPrefs.SetFloat("colorMirrorRotate3", colorMirrorRotate);
        PlayerPrefs.Save();
    }
}
