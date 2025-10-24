using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicAltar : MonoBehaviour
{

    //音樂祭壇的初始音樂先設置為 Si So Do Do So Si La
    //如果有一個按鍵按錯的話就重新開始
    public TempleGameController templeGameController;
    public TempleGameDataSo templeGameData;

    public Text musicText;
    public string lastmusicName = "";
    public string currentName = "";

    private int musicInt = 0;
    public string musicName = "";

    [Header("動畫轉場")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("音符正確順序")]
    public bool music1 = false;
    public bool music2 = false;
    public bool music3 = false;
    public bool music4 = false;
    public bool music5 = false;
    public bool music6 = false;
    public bool music7 = false;


    private void Update()
    {
        if (templeGameData.templeGameOver)
        {
            return;
        }

        musicText.text = lastmusicName;
        musicCorrect();
    }


    public void ClearMusic()
    {
        musicInt = 0;
        lastmusicName = "";
        musicText.text = "";
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;
    }

    public void musicCorrect()
    {
        if (music1 && music2 && music3 && music4 && music5 && music6 && music7)
        {
            //播放影片
            //獲得樂譜

            //任務結束，判斷設置
            print("神廟遊戲結束");

            templeGameData.finishMusicGame = true;

            //關閉此頁面
            templeGameController.closeGameCanva();
            PlayAnim();
            return;
        }

    }

    //音符按鈕判斷
    public void playDo()
    {

        musicInt++;
        if (musicInt > 7)
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

        /////////
        // if (music3 && !music5)
        // {
        //     music4 = true;
        // }
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
            music6 = false;
            music7 = false;
        }

        musicName = "Do";
        //播放Fa的聲音
        print("Do");
    }
    public void playRe()
    {

        musicInt++;
        if (musicInt > 7)
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
            lastmusicName = currentName;
        }

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "Re";
        //播放Fa的聲音
        print("Re");
    }
    public void playMi()
    {

        musicInt++;
        if (musicInt > 7)
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
            lastmusicName = currentName;
        }

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "Mi";
        //播放Fa的聲音
        print("Mi");
    }
    public void playFa()
    {

        musicInt++;
        if (musicInt > 7)
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

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "Fa";
        //播放Fa的聲音
        print("Fa");
    }
    public void playSo()
    {

        musicInt++;
        if (musicInt > 7)
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
        ////////////////
        if (music4 && !music6)
        {

            music5 = true;
        }

        else if (music1 && !music3)
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
            music6 = false;
            music7 = false;
        }


        musicName = "So";
        //播放Re的聲音
        print("So");

    }
    public void playLa()
    {

        musicInt++;

        if (musicInt > 7)
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

        if (music6)
        {
            music7 = true;
        }
        else
        {
            music1 = false;
            music2 = false;
            music3 = false;
            music4 = false;
            music5 = false;
            music6 = false;
            music7 = false;
        }

        musicName = "La";
        //播放#Do的聲音
        print("La");
    }
    public void playSi()
    {

        musicInt++;
        if (musicInt > 7)
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

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "Si";
        //播放Fa的聲音
        print("Si");

    }
    public void play_Do()
    {

        musicInt++;
        if (musicInt > 7)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "_Do";
            lastmusicName = "_Do";
        }
        else
        {
            currentName = lastmusicName + " _Do";
            lastmusicName = currentName;
        }

        /////////
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
            music6 = false;
            music7 = false;
        }

        musicName = "_Do";
        //播放Fa的聲音
        print("_Do");
    }
    public void play__Do()
    {

        musicInt++;
        if (musicInt > 7)
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

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "#Do";
        //播放Fa的聲音
        print("#Do");
    }
    public void play__Fa()
    {

        musicInt++;
        if (musicInt > 7)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "#Fa";
            lastmusicName = "#Fa";
        }
        else
        {
            currentName = lastmusicName + " #Fa";
            lastmusicName = currentName;
        }

        /////////
        music1 = false;
        music2 = false;
        music3 = false;
        music4 = false;
        music5 = false;
        music6 = false;
        music7 = false;

        musicName = "#Fa";
        //播放Fa的聲音
        print("#Fa");
    }
    public void play__Si()
    {



        ///////////////
        musicInt++;
        if (musicInt > 7)
        {
            musicInt = 1;
            lastmusicName = "";
            musicText.text = "";
        }

        if (lastmusicName == "")
        {
            currentName = "bSi";
            lastmusicName = "bSi";
        }
        else
        {
            currentName = lastmusicName + " bSi";
            lastmusicName = currentName;
        }

        ////////////////
        if (music5 && !music7)
        {
            music6 = true;
        }

        else if (!music1)
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
            music6 = false;
            music7 = false;
        }

        musicName = "bSi";
        //播放Si的聲音
        print("bSi");

    }
    private void PlayAnim()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }
}
