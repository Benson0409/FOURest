using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSetting : MonoBehaviour
{
    //文本
    public DialogueDataSo dialogueData;
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
        print("資料清除");
        dialogueData.currentIndex = 0;

        dialogueData.currentState = 0;
        dialogueData.previousState = 0;

        dialogueData.icon = 0;

        dialogueData.repeatText = false;
        dialogueData.missionState = false;
    }
}
