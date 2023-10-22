using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSlider : MonoBehaviour
{
    //滑動指定區塊->讓上方箭頭轉動->當密碼正確後開啟大門
    //改變這個的Ｚ軸旋轉角度
    public TempleGameController templeGameController;
    public GameObject touchCanva;

    //播放旁白
    public SummerGameController summerGameController;

    [Header("旋轉圖片")]
    public GameObject secretImage;

    [Header("旋轉控制")]
    public float rotationSpeed;

    [Header("滑動區跨判斷")]
    public RectTransform key;

    //初始觸控位置紀錄
    private Vector2 touchStartPos;

    //密碼正確性
    public bool secret1 = false;
    public bool secret2 = false;
    public bool secret3 = false;
    public bool secret4 = false;

    //手指狀態
    public bool onScreen =false;

    public static bool openDoorGame = false;

    private void Update()
    {
        if (!openDoorGame)
        {

            doorSecret();

            if (Input.touchCount > 0)
            {

                foreach (Touch touch in Input.touches)
                {
                    Vector2 touchPosition = touch.position;
                    if (RectTransformUtility.RectangleContainsScreenPoint(key, touchPosition))
                    {
                        //物體的旋轉變量紀錄
                        Vector3 rotationIncrement = Vector3.zero;
                      
                        switch (touch.phase)
                        {
                            
                            case TouchPhase.Began:
                                //記錄手指初始位置
                                //rotationIncrement = Vector3.zero;

                                touchStartPos = touch.position;
                                break;

                            case TouchPhase.Moved:
                                //rotationIncrement += new Vector3(0, 0, touch.deltaPosition.y * rotationSpeed * Time.deltaTime);
                                Vector2 touchDeltaPos = touch.position - touchStartPos;
                                float angle = Mathf.Atan2(touchDeltaPos.y, touchDeltaPos.x) * Mathf.Rad2Deg;
                                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                                secretImage.transform.rotation = Quaternion.Slerp(secretImage.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                                onScreen = true;
                                break;

                            case TouchPhase.Ended:
                                onScreen = false;
                                break;

                        }
                        //secretImage.transform.Rotate(rotationIncrement);
                    }
                }
            }
        }
    }

    //判斷大門的密碼
    //可以設定多組密碼
    //用secretImage的rotation.z 來判斷是否指導正確的密碼
    //先假設密碼為DBAC
    //A 90
    //B 0
    //C 270
    //D 180
    public void doorSecret()
    {
        //要在確定每個旋轉值的Ｚ印出來是什麼，畢面判斷錯誤
        //後面可以加入手指是否離開
        float secretRotation = (secretImage.transform.eulerAngles.z + 360) % 360;

        if (secretRotation <= 185 && secretRotation >= 175 && !onScreen || secret1)
        {
            secret1 = true;
            
        }
        else
        {
            secret1 = false;
        }

        //這邊判斷要在仔細一點，讓他可以０和３６０來判斷
        if (((secretRotation <= 5 && secretRotation >= 0)||(secretRotation - 360 >= -5 && secretRotation - 360 <=0))&& !onScreen && secret1 || secret2)
        {
            
            secret2 = true;
        }
        else
        {
            secret2 = false;
        }

        if (secretRotation <= 95 && secretRotation >= 85 && !onScreen && secret2 || secret3)
        {
            secret3 = true;
        }

        else
        {
            secret3 = false;
        }

        if (secretRotation <= 275 && secretRotation >= 265 && !onScreen && secret3 || secret4)
        {
            secret4 = true;
        }
        else
        {
            secret4 = false;
        }
    }

    public void correctSecret()
    {
        if(secret1 && secret2 && secret3 && secret4)
        {
            //解鎖大門開啟下一階段的遊戲（祭壇控制）
            //後期解鎖成功後播放動畫
            print("開啟神廟大門");
            openDoorGame = true;
            templeGameController.startMusicAltar();
            summerGameController.openNarrationSystem();
            templeGameController.closeGameCanva();
        }
    }
}
