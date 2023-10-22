using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class narrationSystem : MonoBehaviour
{
    //旁白系統
    //專門來顯示旁白的

    [Header("UI 物件")]
    public GameObject TextPanel;
    public Text textLabel;

    [Header("文本")]
    public TextAsset[] textAsset;
    List<string> textList = new List<string>();
    public int textFile = 0;

    [Header("文本目前進度")]
    public int index = 0;
    public float lateTime;

    [Header("文字判斷器的控制")]
    //文字的輸入是否完成
    public bool textFinsh;

    //是否有取消文字逐格輸入，變成直接顯示
    public bool cancelInput;

    //文檔的結束 
    //public bool textOver = false;

    //對話系統開啟與否
    public bool startDialogue = false;



    [Header("旁白開啟狀態")]
    public bool openNarration;


    private void Awake()
    {
        if (PlayerPrefs.GetInt("firstSave") == 1)
        {
            loadTextFile();
        }
        else
        {
            saveTextFile();
        }

        //仔入文檔
        GetTextFormFile(textAsset[textFile]);
    }

    private void OnEnable()
    {
        //任務進程判斷
        if (openNarration == true && textFile != 0)
        {
            //仔入文檔
            GetTextFormFile(textAsset[textFile]);
        }

        textFinsh = true;
        StartCoroutine(SetTextUI());
    }


    void Update()
    {
        
        //當文本結束時，將對話框關閉，必將各數值都歸位成初始值
        //這裡需要儲存index的值，並將的值給歸零
        //MissionController.missionFinish 用這個來當作是否完成任務的依據，讓我們決定要怎麼來進行下一步的對話
        //最後要在判斷是否需要重複語句
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && index == textList.Count   && textFinsh && startDialogue && openNarration)
        {
            //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
            this.gameObject.SetActive(false);

            TextPanel.SetActive(false);
            startDialogue = false;
            index = 0;
          

            openNarration = false;
            //下次旁白開始後可以進行下一個文本
            textFile++;
            print("關閉旁白");
            saveTextFile();
        }

        //判斷目前字元是不是最後一個
        //跳過對話顯示設定
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && startDialogue)
        {
            //按下Ｒ鍵，開始顯示下一段文字
            if (textFinsh && !cancelInput )
            {
                StartCoroutine(SetTextUI());
            }
            //再按一下，將文字直接顯示，並將canselInput＝true
            else if (!textFinsh)
            {
                cancelInput = !cancelInput;
            }
        }

    }

    private void GetTextFormFile(TextAsset file)
    {
        textList.Clear();

        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    //---------------文字填入器------------

    //每按一次Ｒ所要執行的指令
    IEnumerator SetTextUI()
    {
        //先將文字用成空的
        textLabel.text = "";

        //文字完成與否
        textFinsh = false;
        

        //判斷需不需要直接完成文字內容，跳過顯示動畫
        int letter = 0;
        //一次顯示一個字，並依照設定的間隔來顯示每一個字
        while (!cancelInput && letter < textList[index].Length - 1)
        {


            textLabel.text += textList[index][letter];
            letter++;
            //等待想要延遲的時間
            yield return new WaitForSeconds(lateTime);
        }

        //如果跳出的話，就直接將文字變成那一行 => cancelInput = true
        textLabel.text = textList[index];
        cancelInput = false;
        textFinsh = true;
        index++;
    }


    private void saveTextFile()
    {
        PlayerPrefs.SetInt("firstSave",1);
        PlayerPrefs.SetInt("textFile", textFile);
        PlayerPrefs.Save();
    }

    private void loadTextFile()
    {
        textFile = PlayerPrefs.GetInt("textFile");
    }
}
