using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class PickTargetTrigger : MonoBehaviour
{
    public TeachingGameDataSo teachingGameData;
    public GameObject teachingPanel;
    //public Text teachingTitle;
    [Header("教學影片")]
    public VideoPlayer videoPlayer;
    public VideoClip pickClip;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!teachingGameData.isDetect && teachingGameData.isBtnActive)
            {
                videoPlayer.clip = pickClip;
                teachingPanel.SetActive(true);
                //teachingTitle.text = "物品收集介紹";
                teachingGameData.isDetect = true;
            }
        }
    }
}
