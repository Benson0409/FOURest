using UnityEngine;

public class doorSlider : MonoBehaviour
{
    //滑動指定區塊->讓上方箭頭轉動->當密碼正確後開啟大門
    //改變這個的Ｚ軸旋轉角度
    public TempleGameController templeGameController;
    public GameObject touchCanva;
    public Canvas canvas;

    [Header("動畫轉場")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("旋轉圖片")]
    public GameObject secretImage;

    [Header("旋轉控制")]
    public float rotationSpeed;

    [Header("滑動區跨判斷")]
    public RectTransform key;

    [Header("大門線索圖片")]
    public GameObject MImage;
    public GameObject UImage;
    public GameObject SImage;
    public GameObject EImage;

    //初始觸控位置紀錄
    private Vector2 touchStartPos;

    //密碼正確性
    public bool secret1 = false;
    public bool secret2 = false;
    public bool secret3 = false;
    public bool secret4 = false;

    //手指狀態
    public bool onScreen = false;

    public static bool openDoorGame = false;



    //判斷大門的密碼
    //可以設定多組密碼
    //用secretImage的rotation.z 來判斷是否指導正確的密碼
    //先假設密碼為MUSE 0 270 90 270
    //D B A C
    //ADCD
    public void doorSecret()
    {
        //((secretRotation <= 5 && secretRotation >= 0) || (secretRotation - 360 >= -5 && secretRotation - 360 <= 0)) && !onScreen
        //要在確定每個旋轉值的Ｚ印出來是什麼，畢面判斷錯誤
        //後面可以加入手指是否離開
        float secretRotation = (secretImage.transform.eulerAngles.z + 360) % 360;

        if (secretRotation <= 95 && secretRotation >= 85 && !onScreen || secret1)
        {


            secret1 = true;
            MImage.SetActive(true);

        }
        else
        {
            secret1 = false;
        }

        //這邊判斷要在仔細一點，讓他可以０和３６０來判斷
        if (secretRotation <= 185 && secretRotation >= 175 && !onScreen && secret1 || secret2)
        {

            secret2 = true;
            UImage.SetActive(true);
        }
        else
        {
            secret2 = false;
        }

        if (secretRotation <= 275 && secretRotation >= 265 && !onScreen && secret2 || secret3)
        {
            secret3 = true;
            SImage.SetActive(true);
        }

        else
        {
            secret3 = false;
        }

        if (secretRotation <= 185 && secretRotation >= 175 && !onScreen && secret3 || secret4)
        {
            secret4 = true;
            EImage.SetActive(true);
        }
        else
        {
            secret4 = false;
        }
    }

    //大門解鎖後 轉場到動畫場景 播放動畫
    //按鈕觸發
    public void correctSecret()
    {
        if (secret1 && secret2 && secret3 && secret4)
        {
            //解鎖大門開啟下一階段的遊戲（祭壇控制）
            //後期解鎖成功後播放動畫
            print("開啟神廟大門");
            openDoorGame = true;
            templeGameController.startMusicAltar();
            templeGameController.closeGameCanva();
            PlayAnim();
        }
    }

    private void PlayAnim()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }

    public void OnDrag()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPos;
            onScreen = true;

            touchPos = Input.GetTouch(0).position; // 使用第一個觸控點的位置
            Vector2 dir = touchPos - secretImage.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (angle < 0) ? (angle + 360) : angle;
            Quaternion r = Quaternion.AngleAxis(angle, Vector3.forward);
            secretImage.transform.rotation = r;
        }
    }
    public void OnDrop()
    {
        onScreen = false;
        doorSecret();
    }
}