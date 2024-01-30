using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTransController : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public MainMenuGameDataSo mainMenuGameData;
    public CinemachineVirtualCamera creatGameCamera;
    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;
        currentCamera = target;
        currentCamera.Priority++;
    }

    //如果點擊繼續遊戲，但先前沒有遊戲紀錄的話就轉換到創建遊戲
    public void CameraChoese(CinemachineVirtualCamera continueCamera)
    {
        currentCamera.Priority--;
        if (mainMenuGameData.isPlaySave)
        {
            currentCamera = continueCamera;
        }
        else
        {
            currentCamera = creatGameCamera;
        }
        currentCamera.Priority++;
    }
}
