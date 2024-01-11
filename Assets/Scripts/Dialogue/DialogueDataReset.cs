using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDataReset : MonoBehaviour
{
    //文本
    public DialogueDataSo[] characterDialogueData;
    public VoidEventSo ResetDataEventSo;

    void OnEnable()
    {
        ResetDataEventSo.OnEventRaised += ResetTextData;
    }

    void OnDisable()
    {
        ResetDataEventSo.OnEventRaised -= ResetTextData;
    }

    public void ResetTextData()
    {
        for (int i = 0; i < characterDialogueData.Length; i++)
        {


            print("資料清除");
            characterDialogueData[i].currentIndex = 0;

            characterDialogueData[i].currentState = 0;
            characterDialogueData[i].previousState = 0;

            characterDialogueData[i].icon = 0;

            characterDialogueData[i].repeatText = false;
            characterDialogueData[i].missionState = false;
        }
    }
}
