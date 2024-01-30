using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PickTargetTrigger : MonoBehaviour
{
    public TeachingGameDataSo teachingGameData;
    public GameObject teachingPanel;
    public Text teachingTitle;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!teachingGameData.isDetect && teachingGameData.isBtnActive)
            {
                teachingPanel.SetActive(true);
                teachingTitle.text = "物品收集介紹";
                teachingGameData.isDetect = true;
            }
        }
    }
}
