using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private int icon;

    [Header("結束遊戲設定")]
    public bool finishGame = false;
    public GameObject gameOverCanva;
    [Header("UI 物件")]
    //停只顯示開啟對話之按鈕
    public bool btnClose;

    public GameObject TextPanel;
    public Text textLabel;
    public Text nameText;
    public GameObject nameBg;
    public Text[] option;
    public GameObject BtnOption;

    [Header("文本")]
    public TextAsset[] textAsset;


    //判斷選項字元有幾個，用它來迴圈並判斷驚嘆號在哪
    [Header("文本目前進度")]
    public int index = 0;
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
    [SerializeField] private int currentState;
    [SerializeField] private int previousState;

    //紀錄儲存
    [SerializeField] public bool missionTextState;

    //紀錄上一個index儲存的值
    [SerializeField] private int currentTextAssestIndex;

    [SerializeField] private bool firstSave;

    //是否要重複文字判斷
    [SerializeField] private bool repeatText;

    ////任務文字啟動判斷
    //[SerializeField] private bool missionText;

    //現在重複狀態的判斷
    [SerializeField] private bool textRepeateState;

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

    void Awake()
    {

        //-----------------------
        if (!DialogueData.loadSaveFirst(ref firstSave))
        {
            DialogueData.saveCurrentIndex(currentTextAssestIndex);

            DialogueData.saveRepeatText(repeatText);

            //判斷任務狀態
            //DialogueData.saveMissionTextState(missionTextState);
        }
        else
        {
            DialogueData.loadCurrentIndex(ref index);

            DialogueData.loadRepeatText(ref repeatText);

            //判斷任務狀態
            DialogueData.loadMissionTextState(ref missionTextState);
            loadCurrentState();
        }

        //判斷是否需要重複語句
        if (previousState != currentState)
        {
            missionTextState = false;
            DialogueData.saveMissionTextState(missionTextState);

            //更改 missionText 的狀態
            repeatText = false;
            DialogueData.saveRepeatText(repeatText);

            //文本進程儲存
            previousState++;
            saveCurrentState();
        }


        //仔入文檔
        GetTextFormFile(textAsset[currentState]);

    }

    //在打開對話框時，就先將第一句話進行運作
    private void OnEnable()
    {
        //任務進程判斷
        if (previousState != currentState)
        {
            //重置任務判斷，好讓我們可以執行下一階段的任務
            missionTextState = false;
            DialogueData.saveMissionTextState(missionTextState);

            //更改 missiomText 的狀態
            repeatText = false;
            DialogueData.saveRepeatText(repeatText);

            print("第二文檔");
            //仔入文檔
            GetTextFormFile(textAsset[currentState]);

            previousState++;
            saveCurrentState();
        }
        //-----------------------

        textFinsh = true;
        StartCoroutine(SetTextUI());
    }

    void Update()
    {
        
        //當文本結束時，將對話框關閉，必將各數值都歸位成初始值
        //這裡需要儲存index的值，並將的值給歸零
        //MissionController.missionFinish 用這個來當作是否完成任務的依據，讓我們決定要怎麼來進行下一步的對話
        //最後要在判斷是否需要重複語句
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && (index == textList.Count || textOver) && !btnClick && textFinsh && startDialogue && !repeatText)
        {
            //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
            this.gameObject.SetActive(false);

            //遊戲結束
            if (finishGame)
            {
                gameOverCanva.SetActive(true);
            }

            TextPanel.SetActive(false);
            startDialogue = false;
            index = 0;
            textOver = false;
            btnClose = false;
            return;
        }




        //任務未完成的話，就重複說這句話
        //這裡必需讀取currentIndex的值，然後讓index的值等於他
        //最後要在判斷是否需要重複語句
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && startDialogue && textFinsh && repeatText)
        {

            //讓對話系統只在需要的時候開啟，也讓awake可以進行啟動判斷
            this.gameObject.SetActive(false);

            TextPanel.SetActive(false);
            startDialogue = false;


            //載入要的語句
            DialogueData.loadCurrentIndex(ref index);

            //載入需要的文檔內容
            loadIcon();

            textOver = false;
            btnClose = false;
            return;
        }


        //判斷目前字元是不是最後一個
        //跳過對話顯示設定
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && startDialogue)
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

        //----------------------------------------

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
                //選項符號
                case '-':
                    //a = 0;
                    btnClick = true;

                    //n控制選項有幾個
                    for (int n = 0; n < option.Length; n++)
                    {
                        //如果我的下下下個字是 ! 就跳出回圈
                        if (index + n + 2 >= textList.Count || textList[index + n + 2] == "!")
                        {
                            break;
                        }
                        //index + n + 2 = 選項的第一個，2是從目前的跳過 - 到第一個選項
                        option[n].text = textList[index + n + 2];
                    }

                    //在文字完成後再執行index的字符，避免顯示錯誤
                    if (textFinsh)
                    {
                        //option.Length + 2會讓文字剛好指到！在 +1 就可以等於驚嘆號的下一個字
                        index += option.Length + 2 + 1;
                        //讓文字顯示完成後，在把btn顯示出來
                        BtnOption.SetActive(true);
                    }
                    break;

                //判斷任務之標示
                case '/':
                    //紀錄目前的index的值
                    currentTextAssestIndex = index;
                    DialogueData.saveCurrentIndex(currentTextAssestIndex);

                    //如果任務完成就呈現任務完成，反之就任務失敗
                    //MissionController.missionFinish 判斷任務是否完成
                    //要把MissionController.missionFinish要把的值儲存起來並調用

                    //需要重複語句的話代表任務還沒完成
                    if (DialogueData.loadMissionTextState(ref missionTextState))
                    {
                        //避免加太多次
                        //任務進程判斷
                        if (currentState - previousState < 1)
                        {
                            currentState++;
                            print(currentState);
                            print(previousState);
                            saveCurrentState();
                        }

                        ////這個等於true的話就要說出下一句話
                        textRepeateState = true;

                        //這個等於true的話就代表任務未完成，需要重複話語
                        repeatText = false;
                        DialogueData.saveRepeatText(repeatText);
                        print("任務完成");

                        ////為下一階段對話做準備
                        //missionTextState = false;
                        //DialogueData.saveMissionTextState(missionTextState);
                    }
                    else
                    {
                        //任務沒完成，就將重複語句的判斷繼續設定成true
                        textRepeateState = false;
                        repeatText = true;
                        //missionText = true;
                        DialogueData.saveRepeatText(repeatText);

                        print("任務未完成");
                    }
                    break;

                //希望在對話結束後也可以自動跳到下一個對話,而且可以有任務的分配
                //要想一下要怎麼在我開啟任務的狀況，怎麼完成任務後結束任務狀態
                //旁白說完就會給予一個任務
                //此時任務狀態會是開啟狀態
                //我們需要將任務狀態用為false
                case '=':
                    if (currentState - previousState < 1)
                    {
                        currentState++;
                        print(currentState);
                        print(previousState);
                        saveCurrentState();
                    }
                    repeatText = true;
                    DialogueData.saveRepeatText(repeatText);

                    textOver = true;
                    break;

                //對話結束的符號
                case '~':
                    textOver = true;
                    finishGame = true;
                    break;
            }
        }



    }

    private void GetTextFormFile(TextAsset file)
    {
        textList.Clear();

        //要加上任務判斷
        if (repeatText)
        {
            DialogueData.loadCurrentIndex(ref index);
            loadIcon();
        }
        else
        {
            index = 0;
        }
        //-----------------

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
                icon = 1;
                saveIconResource();
                nameText.text = "水仙子";
                index++;
                break;

            //如果是Ｂ就切換到Ｂ的圖片，並將index++切換到下一行
            case 'B':
                nameBg.SetActive(true);
                icon = 2;
                saveIconResource();
                nameText.text = "Lilium";
                index++;
                break;

            case 'C':
                nameBg.SetActive(true);
                icon = 3;
                saveIconResource();
                nameText.text = "Season";
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

    //------------------選項控制------------------

    //按鈕被觸發後的執行
    public void optionChoose1()
    {
        //index++;
        btnClick = false;
        BtnOption.SetActive(false);
        print("choose1");
        StartCoroutine(SetTextUI());
    }

    public void optionChoose2()
    {
        for (int i = index; i < textList.Count; i++)
        {
            //代表第二分支，選項後面的對應話句
            if (textList[i][0] == '?')
            {
                print(textList[i]);
                index = i + 1;
            }
        }
        //index += 3;
        btnClick = false;
        BtnOption.SetActive(false);
        print("choose2");
        StartCoroutine(SetTextUI());
    }

    //------------------文檔語句紀錄------------------

    //讓我可以切換腳本來說出不一樣的話語
    //儲存目前任務進程的數值,還有儲存先前的數值來讓劇本可以在任務進程前進時仔入文本內容
    public void saveCurrentState()
    {
        PlayerPrefs.SetInt("currentState", currentState);
        PlayerPrefs.SetInt("previousState", previousState);
    }

    //讀取目前任務數值的進程
    public void loadCurrentState()
    {
        currentState = PlayerPrefs.GetInt("currentState");
        previousState = PlayerPrefs.GetInt("previousState");

    }

    public void saveIconResource()
    {
        PlayerPrefs.SetInt("Icon", icon);
    }

    public void loadIcon()
    {
        switch (PlayerPrefs.GetInt("Icon"))
        {
            case 1:
                nameText.text = "水仙子";
                nameBg.SetActive(true);
                break;

            case 2:
                nameText.text = "Lilium";
                nameBg.SetActive(true);
                break;

            case 3:
                nameText.text = "Season";
                nameBg.SetActive(true);
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
}