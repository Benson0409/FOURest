using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    //控制任務相關進程以及文檔輸出控制
    //要紀錄相關儲存的資訊以及任務目前狀況

    //private static bool firstSave = false;



    public static void saveFirst()
    {

        PlayerPrefs.SetInt("saveFirst", 1);
        PlayerPrefs.Save();


    }

    public static bool loadSaveFirst(ref bool saveFirst)
    {
        if (PlayerPrefs.GetInt("saveFirst") == 1)
        {
            saveFirst = true;
            return saveFirst;
        }
        else
        {
            saveFirst = false;
            return saveFirst;
        }
    }
    //----------------------------------
    public static void saveCurrentIndex(int index)
    {
        saveFirst();
        PlayerPrefs.SetInt("currentIndex", index);
        PlayerPrefs.Save();
    }

    public static int loadCurrentIndex(ref int index)
    {
        index = PlayerPrefs.GetInt("currentIndex");
        return index;
    }

    //----------------------------------
    public static void saveRepeatText(bool missiionText)
    {
        if (missiionText)
        {
            PlayerPrefs.SetInt("repeatText", 1);
        }
        else
        {
            PlayerPrefs.SetInt("repeatText", 0);
        }
        PlayerPrefs.Save();
    }

    public static bool loadRepeatText(ref bool repeatText)
    {
        if (PlayerPrefs.GetInt("repeatText") == 1)
        {
            repeatText = true;
            return repeatText;
        }

        else
        {
            repeatText = false;
            return repeatText;
        }
    }
    //----------------------------------
    //任務狀態判斷
    public static void saveMissionTextState(bool MissionState)
    {
        if (MissionState)
        {
            PlayerPrefs.SetInt("MissionState", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MissionState", 0);
        }
        PlayerPrefs.Save();
    }

    public static bool loadMissionTextState(ref bool MissionState)
    {
        if (PlayerPrefs.GetInt("MissionState") == 1)
        {
            MissionState = true;
            return MissionState;
        }

        else
        {
            MissionState = false;
            return MissionState;
        }
    }
}
