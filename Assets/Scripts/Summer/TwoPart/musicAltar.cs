using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicAltar : MonoBehaviour
{

    //音樂祭壇的初始音樂先設置為 So Re #Do Fa Si
    //如果有一個按鍵按錯的話就重新開始
    public TempleGameController templeGameController;
    public SummerGameController summerGameController;

    public Text musicText;
    private string lastmusicName="";
    private string currentName = "";

    private int musicInt = 0;
    public string musicName = "";

    [Header("音符正確順序")]
    public bool music1 = false;
    public bool music2 = false;
    public bool music3 = false;
    public bool music4 = false;
    public bool music5 = false;

    [Header("莉莉絲位置")]
    public Transform liliPosition;
    public Transform targetPosition;

    private void Update()
    {
        if (PlayerPrefs.GetInt("templeGameFinish") == 1)
        {
            return;
        }
        print(music1);
        musicText.text = lastmusicName;
        musicCorrect();
    }


    public void musicCorrect()
    {
        if (music1 && music2 && music3 && music4 && music5)
        {
            //播放影片
            //獲得樂譜

            //莉莉絲位置移動
            liliPosition.position = targetPosition.position;
            liliPosition.rotation = targetPosition.rotation;


            //任務結束，判斷設置
            print("神廟遊戲結束");

            templeGameController.finishAltarGame = true;
           

            PlayerPrefs.SetInt("finishAltarGame", 1);
            PlayerPrefs.Save();

            //對話任務結束
            DialogueData.saveMissionTextState(true);

            //旁白開啟
            summerGameController.openNarrationSystem();
            //關閉此頁面
            templeGameController.closeGameCanva();
            return;
        }

    }

    //音符按鈕判斷
    public void playDo()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "Do";
            lastmusicName = "Do";
        }
        else
        {
            currentName = lastmusicName + " Do";
            lastmusicName = currentName;
        }

        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;

        //播放Do的聲音
        musicName = "Do";
        
        print("Do");
    }

    public void playRe()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "Re";
            lastmusicName = "Re";
        }
        else
        {
            currentName = lastmusicName + " Re";
            lastmusicName =  currentName;
        }

        if (music1 && !music3)
        {
            music2 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
        }


        musicName = "Re";
        //播放Re的聲音
        print("Re");
    }
    public void playMi()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "Mi";
            lastmusicName = "Mi";
        }
        else
        {
            currentName = lastmusicName + " Mi";
            lastmusicName =  currentName;
        }

        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;

        musicName = "Mi";
        //播放Mi的聲音
        print("Mi");
    }
    public void playFa()
    {

        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "Fa";
            lastmusicName = "Fa";
        }
        else
        {
            currentName = lastmusicName + " Fa";
            lastmusicName = currentName;
        }

        if (music3 && !music5)
        {
            music4 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
        }

        musicName = "Fa";
        //播放Fa的聲音
        print("Fa");
    }
    public void playSo()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "So";
            lastmusicName = "So";
        }
        else
        {
            currentName = lastmusicName + " So";
            lastmusicName = currentName;
        }

        if (!music1)
        {
            music1 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
        }

        musicName = "So";
        //播放Do的聲音
        print("So");
    }
    public void playLa()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "La";
            lastmusicName = "La";
        }
        else
        {
            currentName = lastmusicName + " La";
            lastmusicName = currentName;
        }

        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;

        musicName = "La";
        //播放La的聲音
        print("La");
    }
    public void playSi()
    {
        musicInt++;
        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "Si";
            lastmusicName = "Si";
        }
        else
        {
            currentName = lastmusicName + " Si";
            lastmusicName = currentName;
        }

        if (music4 && !music5)
        {
            music5 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
        }

        musicName = "Si";
        //播放Do的聲音
        print("Si");
    }
    public void playDoUp()
    {
        musicInt++;

        if (musicInt > 5)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "#Do";
            lastmusicName = "#Do";
        }
        else
        {
            currentName = lastmusicName + " #Do";
            lastmusicName = currentName;
        }

        if (music2 && !music4)
        {
            music3 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
        }

        musicName = "#Do";
        //播放#Do的聲音
        print("#Do");
    }
}
