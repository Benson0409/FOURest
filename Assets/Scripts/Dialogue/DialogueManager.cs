using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //文本資訊紀錄，讓每個文本的資訊都獨立
    [HideInInspector] public DialogueDataSo dialogueData;
    //紀錄當前圖標設定
    private int icon;

    [Header("結束遊戲設定")]
    public bool finishGame = false;
    public static bool openGameOverCanva;

    [Header("UI 物件")]
    //停只顯示開啟對話之按鈕
    public bool btnClose;

    public GameObject TextPanel;
    public Text textLabel;
    public Text nameText;
    public GameObject nameBg;

    // public Text[] option;
    // public GameObject BtnOption;

    //根據文本來跟換
    [Header("對話名字背景")]
    public Image nameImage;
    public Sprite liliSprite;
    public Sprite seasonSprite;
    public Sprite waterSprite;

    [Header("文本")]
    public TextAsset[] textAsset;

    //判斷選項字元有幾個，用它來迴圈並判斷驚嘆號在哪
    [Header("文本目前進度")]
    private int index = 0;
    public float lateTime;

    [Header("文字判斷器的控制")]
    //文字的輸入是否完成
    public bool textFinsh;

    //是否有取消文字逐格輸入，變成直接顯示
    public bool cancelInput;

    //按鈕是否有被觸發
    public bool btnClick = false;

    //文檔的結束
    public bool textOver = false;

    //對話系統開啟與否
    public bool startDialogue = false;
    //------------------------------------

    [Header("多腳本判斷")]
    private int currentState;
    private int previousState;

    //紀錄儲存
    public bool missionTextState;

    //紀錄上一個index儲存的值
    private int currentTextAssestIndex;

    //是否要重複文字判斷
    private bool repeatText;

    //現在重複狀態的判斷
    private bool textRepeateState;

    List<string> textList = new List<string>();

    //文檔規則
    // - 需要玩家進行問答選擇
    // ! 代表選項的結尾，後面沒有選項了
    // ~ 代表對話結束
    // ? 代表選項的第二個對應話語，這樣就可以增加活性
    // > 代表文檔必須要跳到共同的話語（因為選項的選不同導致對話不同，但是結局是一樣的，所以後面說的話也是一樣的）
    // < 代表要跳到的位置，找到的方法會跟尋找?時的發法差不多
    // / 代表在還沒完成指定事情的時候他必須一直重複上一句話
    // = 代表對話結束後有任務產生，但此句不會重複顯示
    // 要根據選擇的字來決定要說什麼



    //如果需要進遊戲馬上有旁白出沒，gameManager會呼叫
    //但可能用一個模塊來勾選會比較好

    //在打開對話框時，就先將第一句話進行運作
    private void OnEnable()
    {
        //基礎資料讀取
        ReadDialogueInformation();

        //任務完成不需重複語句
        MissionDialogueStateDetect();

        //仔入文檔
        GetTextFormFile(textAsset[currentState]);
        //-------------------------------------

        //UI資訊顯示
        StartCoroutine(SetTextUI());
    }



    void Update()
    {
        //對話結束後的判斷
        OverDialogueStateDetect();

        //在任務未完成前 重複同一句話
        RepeatDialogueState();

        //判斷目前文字狀況呈現如何，是否要直接顯示全部
        DialogueShowState();

        //對話下一個字元判斷，來看文本要呈現什麼內容
        NextDialogueTextState();

    }

    //---------------文字填入器------------
    //將文本填入List中
    private void GetTextFormFile(TextAsset file)
    {
        textList.Clear();

        //如果是重複語句就讀取需要重複話語的語句index
        ReadMissionTextIndex();
        //將textAsset填入到list中
        IputTextList(file);
    }

    //如果是重複語句就讀取需要重複話語的語句index
    private void ReadMissionTextIndex()
    {
        //要加上任務判斷
        if (repeatText)
        {
            index = dialogueData.currentIndex;
            loadIcon();
        }
        else
        {
            index = 0;
        }
    }
    //將textAsset填入到list中
    private void IputTextList(TextAsset file)
    {
        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    //---------------UI顯示---------------

    //每按一次Ｒ所要執行的指令
    IEnumerator SetTextUI()
    {
        //先將文字用成空的
        textLabel.text = "";

        //文字完成與否
        textFinsh = false;

        //判斷當前的字元
        char currentText = textList[index][0];

        //任務系統判斷需不需要重複語句
        if (textRepeateState)
        {
            index += 2;
            textRepeateState = false;
        }

        switch (currentText)
        {
            //如果是Ａ就切換到Ａ的圖片，並將index++切換到下一行
            case 'A':
                nameBg.SetActive(true);
                nameImage.sprite = waterSprite;

                icon = 1;
                dialogueData.icon = icon;
                nameText.text = "Narcissus";
                index++;
                break;

            //如果是Ｂ就切換到Ｂ的圖片，並將index++切換到下一行
            case 'B':
                nameBg.SetActive(true);
                nameImage.sprite = liliSprite;

                icon = 2;
                dialogueData.icon = icon;
                nameText.text = "Lilium";
                index++;
                break;

            //如果是C就切換到C的圖片，並將index++切換到下一行
            case 'C':
                nameBg.SetActive(true);
                nameImage.sprite = seasonSprite;

                icon = 3;
                dialogueData.icon = icon;
                nameText.text = "Season";
                index++;
                break;
            case 'D':
                nameBg.SetActive(true);
                nameImage.sprite = seasonSprite;

                icon = 4;
                dialogueData.icon = icon;
                nameText.text = "OldMan";
                index++;
                break;


            //判斷任務目前執行狀況，如果任務完成我們才把繼續進行對話，如果沒有的話我們就重複說這句話
            //update 裏面判斷，如果repeatText==true 我們就一直跑這行
            //反之就進行下一句
            //一定要在update 裏面完成
            //遇到段落切換時，就去尋找我要移動到的地方
            //判斷可能要想辦法放在update裏面，不能放在協程裡面，因為這樣就不能及時判斷，但也要看要如何讓我按下R鍵後在將文字做更換
            case '>':
                for (int i = index; i < textList.Count; i++)
                {
                    //代表第二分支，選項後面的對應話句
                    //因為我們是每按一次R鍵才會執行一次，所以我+1會直接等於把人名給顯示出來
                    //但這樣假如我們要和人說的話，就要先在>的前面打上下一個人要說話的名字，讓圖片可以先換
                    if (textList[i][0] == '<')
                    {
                        index = i + 2;
                    }
                }
                break;
        }

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

        //如果目前是沒有選項的，我就將index的值+1
        if (!btnClick && !repeatText)
        {
            index++;
        }
    }

    //------------------文檔語句紀錄------------------

    //基礎資料讀取
    private void ReadDialogueInformation()
    {
        index = dialogueData.currentIndex;

        repeatText = dialogueData.repeatText;

        missionTextState = dialogueData.missionState;

        currentState = dialogueData.currentState;

        previousState = dialogueData.previousState;
    }

    //任務完成不需重複語句
    private void MissionDialogueStateDetect()
    {
        if (previousState != currentState)
        {
            missionTextState = false;
            dialogueData.missionState = missionTextState;

            //更改 missionText 的狀態
            repeatText = false;
            dialogueData.repeatText = repeatText;

            //文本進程儲存
            previousState++;
            dialogueData.previousState = previousState;
        }
    }

    //對話結束後的判斷
    private void OverDialogueStateDetect()
    {
        //當文本結束時，將對話框關閉，必將各數值都歸位成初始值
        //這裡需要儲存index的值，並將的值給歸零
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && (index == textList.Count || textOver) && !btnClick && textFinsh && startDialogue && !repeatText)
        {
            //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
            this.gameObject.SetActive(false);

            //遊戲結束執行
            GameOverDetect();
            //將基礎資訊回到一開始的值
            ResetDialogueInformation();
            return;
        }
    }

    //將基礎資訊回到一開始的值
    private void ResetDialogueInformation()
    {
        TextPanel.SetActive(false);
        startDialogue = false;
        index = 0;
        textOver = false;
        btnClose = false;
    }

    //重複對話的執行
    private void RepeatDialogueState()
    {

        //任務未完成的話，就重複說這句話
        //這裡必需讀取currentIndex的值，然後讓index的值等於他
        //最後要在判斷是否需要重複語句

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && startDialogue && textFinsh && repeatText)
        {

            //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
            this.gameObject.SetActive(false);

            TextPanel.SetActive(false);
            startDialogue = false;


            //載入要的語句
            index = dialogueData.currentIndex;

            //載入需要的文檔內容
            loadIcon();

            textOver = false;
            btnClose = false;
            return;
        }
    }

    //遊戲結束執行
    private void GameOverDetect()
    {
        //遊戲結束
        if (finishGame)
        {
            openGameOverCanva = true;
        }
    }

    //判斷目前文字狀況呈現如何，是否要直接顯示全部
    private void DialogueShowState()
    {
        //判斷目前字元是不是最後一個
        //跳過對話顯示設定
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && startDialogue)
        {
            //按下Ｒ鍵，開始顯示下一段文字
            if (textFinsh && !cancelInput && !btnClick)
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

    //對話下一個字元判斷，來看文本要呈現什麼內容
    private void NextDialogueTextState()
    {
        //判斷下一個字元
        char nextText = ' ';
        //如果他不是最後一個字元就執行
        //腳本內控制判斷
        //要加上任務性質的判斷

        if (index < textList.Count - 1)
        {
            //紀錄下一個字詞
            //可能要加入一個判斷章節的符號
            nextText = textList[index + 1][0];
            switch (nextText)
            {
                //判斷任務之標示
                case '/':
                    //任務狀態判斷
                    MissionStateDetect();
                    break;

                //對話結束遊戲結束的符號
                case '~':
                    GameOverState();
                    break;
            }
        }
    }

    //任務狀態判斷
    private void MissionStateDetect()
    {
        //紀錄目前的index的值
        currentTextAssestIndex = index;
        dialogueData.currentIndex = currentTextAssestIndex;

        //如果任務完成就呈現任務完成，反之就任務失敗
        //MissionController.missionFinish 判斷任務是否完成
        //要把MissionController.missionFinish要把的值儲存起來並調用

        //需要重複語句的話代表任務還沒完成
        if (missionTextState = dialogueData.missionState)
        {
            //任務完成
            MissionFinish();
        }
        else
        {
            //任務未完成
            MissionFail();
        }
    }
    //遊戲結束判斷
    private void GameOverState()
    {
        textOver = true;
        finishGame = true;
    }
    //任務完成
    private void MissionFinish()
    {
        //避免加太多次
        //任務進程判斷
        if (currentState - previousState < 1)
        {
            currentState++;

            dialogueData.currentState = currentState;
            dialogueData.previousState = previousState;
        }

        ////這個等於true的話就要說出下一句話
        textRepeateState = true;

        //這個等於true的話就代表任務未完成，需要重複話語
        repeatText = false;
        dialogueData.repeatText = repeatText;
        print("任務完成");
    }
    //任務未完成
    private void MissionFail()
    {
        //任務沒完成，就將重複語句的判斷繼續設定成true
        textRepeateState = false;
        repeatText = true;

        dialogueData.repeatText = repeatText;

        print("任務未完成");
    }
    public void loadIcon()
    {
        switch (dialogueData.icon)
        {
            case 1:
                nameText.text = "Narcissus";
                nameBg.SetActive(true);
                nameImage.sprite = waterSprite;
                break;

            case 2:
                nameText.text = "Lilium";
                nameBg.SetActive(true);
                nameImage.sprite = liliSprite;
                break;

            case 3:
                nameText.text = "Season";
                nameBg.SetActive(true);
                nameImage.sprite = seasonSprite;
                break;
            case 4:
                nameText.text = "OldMan";
                nameBg.SetActive(true);
                nameImage.sprite = seasonSprite;
                break;
        }

    }

    public void startTextBtn()
    {

        startDialogue = true;
        btnClose = true;
        //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
        this.gameObject.SetActive(true);
        TextPanel.SetActive(true);
    }

    //讀取目前資訊訊息
    public void ReadTextAsset(DialogueDataSo currentData)
    {
        dialogueData = null;
        dialogueData = currentData;
        textAsset = currentData.textAsset;
    }
}
