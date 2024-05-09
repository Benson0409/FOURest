using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARController : MonoBehaviour
{
    private Camera mCamera;
    [Header("新手關卡")]
    public TeachingGameDataSo teachingGameData;
    [Header("新手關卡物件")]
    // public GameObject TeachingPanel;
    // public Text TeachingTitle;
    public GameObject teachingCrystal;

    [Header("遊戲數據")]
    public CookieGameDataSo cookieGameData;
    public TempleGameDataSo templeGameData;
    public ColorGameDataSo colorGameData;

    [Header("掃描物體提示")]
    public CanvasGroup hintCanvasGroup;
    public GameObject hintPanel;
    public float fadeInDuration;
    public float fadeOutDuration;
    public Text hintText;

    [Header("場景切換特效")]
    public SwitchScenes scenesCanvaPrefabs;

    [Header("UI")]
    //顏色遊遊戲UI
    public GameObject colorPanel;
    public Image rotate1Image;
    public Image rotate2Image;
    public Image rotate3Image;

    //餅乾遊戲UI
    public GameObject findClueUI;
    public Text cookieName;
    public Image cookieImage;
    public Sprite squareImage;
    public Sprite circleImage;
    public Sprite triangleImage;

    [Header(" 餅乾遊戲生成物體")]
    public GameObject cookieObject1;
    public GameObject cookieObject2;
    public GameObject cookieObject3;

    [Header("神廟遊戲生成物體")]
    public GameObject doorClueObject1;

    [Header("音樂祭壇生成物體")]
    public GameObject musicAltarClue1;

    [Header("顏色遊戲三色鏡生成")]
    public GameObject colorMirror;

    //確認三色鏡的旋轉角度是正確
    private bool correctRotate1 = false;
    private bool correctRotate2 = false;
    private bool correctRotate3 = false;

    //---------------生成功能-------------------
    [Header("生成座標")]

    //要生成的物體
    private GameObject arTospawnObject;

    //這還沒生成物體時的地板圖標
    public GameObject placementIndicator;

    //生成後的物體
    private GameObject spawnObject;


    //地標的位置
    private Pose placementPose;

    //地標是否還存在
    private bool placementPoseValid = false;
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    //---------------控制判斷--------------------
    private bool isARController = false;
    //用來判斷目前要偵測的地方是什麼
    private bool planeDetect = false;
    private bool wallDetect = false;


    ////--------------放大功能--------------------

    ////手指間的距離
    //private float touchDistance;

    ////物體的初始放大程度
    //private Vector3 initialScale;

    //private bool isScale = false;

    ////--------------旋轉功能---------------------
    private Vector2 previousPosition;
    public float rotataSpeed;

    //[System.Obsolete]
    private void Awake()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        aRPlaneManager = FindObjectOfType<ARPlaneManager>();
        mCamera = Camera.main;

        // if (teachingGameData.isTeachingGameOver)
        // {
        //     TeachingPanel.SetActive(false);
        // }
        //顏色遊戲開啟
        if (colorGameData.startColorGame)
        {
            print("平面偵測");
            planeDetect = true;
            if (PlayerPrefs.GetString("colorMirror") == "colorMirror")
            {
                print("生成三色鏡");
                arTospawnObject = colorMirror;
                colorPanel.SetActive(true);
                return;
            }
            //開啟旋轉功能
            //記錄旋轉角度
            //讓三個柱體的光柱可以聚再一起並將遊戲場景內的光柱調整
            return;
        }
        //新增一個音符的AR生成

        //把後面的關卡放到前面，避免衝突
        if (templeGameData.startTempleGame)
        {
            print("牆壁偵測");
            wallDetect = true;

            if (templeGameData.startMusicGame)
            {
                if (PlayerPrefs.GetString("musicAltarClue") == "musicAltarClue1")
                {
                    arTospawnObject = musicAltarClue1;
                    return;
                }
            }
            if (templeGameData.startDoorGame)
            {
                switch (PlayerPrefs.GetString("doorClue"))
                {
                    case "doorClue1":

                        arTospawnObject = doorClueObject1;
                        break;
                }
            }
            return;
        }

        if (cookieGameData.startCookieGame)
        {
            print("平面偵測");
            planeDetect = true;
            switch (PlayerPrefs.GetString("cookie"))
            {
                case "field1":
                    arTospawnObject = cookieObject1;
                    cookieImage.sprite = squareImage;
                    break;

                case "field2":
                    arTospawnObject = cookieObject2;
                    cookieImage.sprite = circleImage;
                    break;

                case "field3":
                    arTospawnObject = cookieObject3;
                    cookieImage.sprite = triangleImage;
                    break;
            }
            return;
        }
        if (teachingGameData.isARPick)
        {
            print("牆壁偵測");
            planeDetect = true;
            // TeachingPanel.SetActive(true);
            // TeachingTitle.text = "AR控制介紹,生成後點擊目標";
            arTospawnObject = teachingCrystal;
        }

    }



    void Update()
    {

        //如果條件都符合就生成物體
        if (placementPoseValid && spawnObject == null && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            arSpawnObject();
            print("生成");
            isARController = true;
        }


        //射線功能（判斷是否有拿到正確物體）
        if (isARController && Input.touchCount > 0)
        {
            // 只處理第一個手指的觸碰事件
            Touch touch = Input.GetTouch(0);

            // 如果手指開始觸碰螢幕
            if (touch.phase == TouchPhase.Began)
            {
                // 將手指觸碰的位置轉換成 AR 相機座標系統下的一個點
                Vector2 touchPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);

                // 射出射線，檢查是否擊中物體
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    //每找到一個餅乾線索任務控制裡面的變數就＋1,等夾到某個數值就代表此次的任務完成 
                    if (hit.transform.gameObject.tag == "cookieClue")
                    {
                        //判斷是否尋找到了餅乾物體
                        switch (PlayerPrefs.GetString("cookie"))
                        {
                            case "field1":
                                arTospawnObject = cookieObject1;
                                cookieGameData.cookie1Field = true;
                                cookieGameData.findCookieCount++;
                                break;

                            case "field2":
                                arTospawnObject = cookieObject2;
                                cookieGameData.cookie2Field = true;
                                cookieGameData.findCookieCount++;
                                break;

                            case "field3":
                                arTospawnObject = cookieObject3;
                                cookieGameData.cookie3Field = true;
                                cookieGameData.findCookieCount++;
                                break;
                        }

                        //顯示撿到線索的面板
                        //將撿到的線索設為false -> 可以避免重複選用
                        hit.transform.gameObject.SetActive(false);

                        // 顯示找到的餅乾名稱
                        cookieName.text = hit.transform.gameObject.name.ToString();

                        // 開啟任務完成的面板，並按下BTN回到主世界
                        findClueUI.SetActive(true);
                    }
                    if (hit.transform.gameObject.tag == "door")
                    {
                        //teachingGameData.isTeachingGameOver = true;
                        //前進夏天關卡
                        backToWorld();
                    }

                }
            }
        }


        ////放大功能
        //if (Input.touchCount == 2)
        //{
        //    //偵測第一隻手指
        //    var touchOne = Input.GetTouch(0);
        //    //偵測第二隻手指
        //    var touchTwo = Input.GetTouch(1);

        //    if (touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled || touchTwo.phase == TouchPhase.Ended || touchTwo.phase == TouchPhase.Canceled)
        //    {
        //        //不符合放大標準，所以就不動作
        //        return;
        //    }

        //    if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
        //    {
        //        //計算初始兩個手指間的距離
        //        touchDistance = Vector2.Distance(touchOne.position, touchTwo.position);

        //        //初始放大程度等於物體一開始的放大程度
        //        //只有在一開始記錄物體初始的大小，後面就利用放大過後的大小來判斷
        //        if (!isScale)
        //        {
        //            initialScale = arTospawnObject.transform.localScale;
        //        }

        //        //除了第一次以外都紀錄spawnObject的大小變化，避免每次都要從新來放大
        //        initialScale = spawnObject.transform.localScale;
        //        Debug.Log("Initaial Scalse:" + initialScale + "Object Name:" + spawnObject.name);
        //    }

        //    // 當手指開始移動後
        //    else
        //    {

        //        //計算目前移動後手指互相的距離
        //        var currentDistiance = Vector2.Distance(touchOne.position, touchTwo.position);

        //        //判斷兩隻手的距離有沒有接近０,如果接近０就不執行
        //        if (Mathf.Approximately(touchDistance, 0))
        //        {
        //            return;
        //        }

        //        //看目前距離比例是多少來決定要放大多少倍
        //        var factor = currentDistiance / touchDistance;

        //        //放大我們的生成物根據計算出來的放大比例
        //        spawnObject.transform.localScale = initialScale * factor;
        //        isScale = true;


        //    }
        //}

        //旋轉功能
        //只需要水平旋轉
        //旋轉Y軸
        //要把個個物體的角度判斷寫進來

        colorRotateDetect();
        if (correctRotate1 && correctRotate2 && correctRotate3)
        {
            //三色鏡尋找完成
            //獲得三色鏡道具
            //轉場回到主世界
            //看要用btn來達成轉場效果還是要直接轉場
            //紀錄完成的變數因子
            colorGameData.isRotate = true;
            colorGameData.startFilterGame = true;
            print("完成三色鏡裝置設置");
            hintCanvasGroup.alpha = 1;
            changeToPlayAnimWorld();
            //backToWorld();
            return;
        }

        if (colorGameData.startColorGame)
        {

            if (Input.touchCount > 0 && spawnObject != null)
            {

                //紀錄第一隻手指的位置
                Touch touch = Input.GetTouch(0);

                // 射出射線，檢查是否擊中物體
                Vector2 touchPosition = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //第一個柱體
                    if (hit.transform.gameObject.tag == "colorMirrorRotate1")
                    {

                        if (touch.phase == TouchPhase.Began)
                        {
                            previousPosition = touch.position;
                        }

                        else if (touch.phase == TouchPhase.Moved)
                        {
                            //算出第一下碰觸的位置跟移動過後的最終位置來判斷距離差了多少，好讓物體根據這個比例來旋轉
                            Vector2 deltaDistance = touch.position - previousPosition;

                            //讓每一幀可以判斷距離差，每一幀都讓他進行旋轉
                            previousPosition = touch.position;

                            float rotationY = -deltaDistance.x * rotataSpeed * Time.deltaTime;
                            //物體在世界空間中繞著 Y 軸旋轉的程式
                            hit.transform.Rotate(Vector3.up, rotationY, Space.World);
                        }

                        ARSystem.saveColorMirrorRotate1(hit.transform.gameObject);

                        //確認每一個的旋轉角度
                        print("Rotata1 :" + hit.transform.eulerAngles.y);

                        if (hit.transform.eulerAngles.y < 21 && hit.transform.eulerAngles.y > 19)
                        {
                            //位置正確 紀錄狀況
                            correctRotate1 = true;
                        }
                        else
                        {
                            //位置錯誤 紀錄消除
                            correctRotate1 = false;
                        }
                    }

                    //第二個柱體
                    if (hit.transform.gameObject.tag == "colorMirrorRotate2")
                    {

                        if (touch.phase == TouchPhase.Began)
                        {
                            previousPosition = touch.position;
                        }

                        else if (touch.phase == TouchPhase.Moved)
                        {
                            //算出第一下碰觸的位置跟移動過後的最終位置來判斷距離差了多少，好讓物體根據這個比例來旋轉
                            Vector2 deltaDistance = touch.position - previousPosition;

                            //讓每一幀可以判斷距離差，每一幀都讓他進行旋轉
                            previousPosition = touch.position;

                            float rotationY = -deltaDistance.x * rotataSpeed * Time.deltaTime;

                            //物體在世界空間中繞著 Y 軸旋轉的程式
                            hit.transform.Rotate(Vector3.up, rotationY, Space.World);
                        }

                        ARSystem.saveColorMirrorRotate2(hit.transform.gameObject);

                        //確認每一個的旋轉角度
                        print("Rotata2 :" + hit.transform.eulerAngles.y);

                        if (hit.transform.eulerAngles.y < 146 && hit.transform.eulerAngles.y > 144)
                        {
                            //位置正確 紀錄狀況
                            correctRotate2 = true;
                        }
                        else
                        {
                            //位置錯誤 紀錄消除
                            correctRotate2 = false;
                        }
                    }

                    //第三個柱體
                    if (hit.transform.gameObject.tag == "colorMirrorRotate3")
                    {

                        if (touch.phase == TouchPhase.Began)
                        {
                            previousPosition = touch.position;
                        }

                        else if (touch.phase == TouchPhase.Moved)
                        {
                            //算出第一下碰觸的位置跟移動過後的最終位置來判斷距離差了多少，好讓物體根據這個比例來旋轉
                            Vector2 deltaDistance = touch.position - previousPosition;

                            //讓每一幀可以判斷距離差，每一幀都讓他進行旋轉
                            previousPosition = touch.position;

                            float rotationY = -deltaDistance.x * rotataSpeed * Time.deltaTime;

                            //物體在世界空間中繞著 Y 軸旋轉的程式
                            hit.transform.Rotate(Vector3.up, rotationY, Space.World);
                        }

                        ARSystem.saveColorMirrorRotate3(hit.transform.gameObject);

                        //確認每一個的旋轉角度
                        print("Rotata3 :" + hit.transform.eulerAngles.y);

                        if (hit.transform.eulerAngles.y < 277 && hit.transform.eulerAngles.y > 275)
                        {
                            //位置正確 紀錄狀況
                            correctRotate3 = true;
                        }
                        else
                        {
                            //位置錯誤 紀錄消除
                            correctRotate3 = false;
                        }
                    }
                }

            }
        }
        //有生成物體後就不在進行下面的指示物判斷
        if (spawnObject != null)
        {
            //生成物體後再將提示關閉
            StartCoroutine(FadeIn(fadeInDuration));
            hintPanel.SetActive(false);
            placementIndicator.SetActive(false);
            return;
        }
        updatePlacementPose();
        updatePlacementIndicator();
    }


    //更新地標的位置
    private void updatePlacementPose()
    {
        if (spawnObject == null && placementPoseValid)
        {
            hintText.text = "點擊螢幕生成物體";

            placementIndicator.SetActive(true);
            var cameraBearing = new Vector3(mCamera.transform.forward.x, 0, mCamera.transform.forward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            //在牆壁偵測的時候,希望我的指示牌是垂直的
            if (wallDetect)
            {
                Quaternion rotation = Quaternion.Euler(270, 0, 0);
                placementPose.rotation *= rotation;
            }

            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }

        else
        {
            if (wallDetect)
            {
                hintText.text = "請掃瞄牆壁";
                hintPanel.SetActive(true);
                hintCanvasGroup.alpha = 1;
                //StartCoroutine(FadeOut(fadeOutDuration));
            }

            if (planeDetect)
            {
                hintText.text = "請掃瞄地板";
                hintPanel.SetActive(true);
                hintCanvasGroup.alpha = 1;
                //StartCoroutine(FadeOut(fadeOutDuration));
            }
            placementIndicator.SetActive(false);
        }
    }

    //那地標生成在新的位置
    private void updatePlacementIndicator()
    {

        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        //偵測放置位置的地方
        //須用兩種方法來判斷
        if (wallDetect)
        {
            aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds);

            placementPoseValid = hits.Count > 0;

            if (placementPoseValid)
            {
                placementPose = hits[0].pose;
            }

            return;
        }

        if (planeDetect)
        {
            aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds);

            placementPoseValid = hits.Count > 0;

            if (placementPoseValid)
            {
                placementPose = hits[0].pose;
            }

            return;
        }

    }

    //生成物體
    void arSpawnObject()
    {
        print("生成物體");
        spawnObject = Instantiate(arTospawnObject, placementPose.position, Quaternion.identity);
        spawnObject.transform.localScale /= 10;
    }

    //轉換到動畫場景播放相對應的動畫
    //有成功完成線索的才轉換去動畫場景
    //其餘都回到主世界
    public void changeToPlayAnimWorld()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }

    //返回到主世界
    public void backToWorld()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        if (!cookieGameData.cookieGameOver && cookieGameData.cookie1Field && cookieGameData.cookie2Field && cookieGameData.cookie3Field)
        {

            switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
            return;
        }
        //代表是從練習關卡來的
        if (!teachingGameData.isTeachingGameOver && !cookieGameData.startCookieGame)
        {
            switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TeachingGame"));
            return;
        }

        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestScene"));
    }


    public IEnumerator FadeOut(float time)
    {

        while (hintCanvasGroup.alpha < 1)
        {
            hintCanvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (hintCanvasGroup.alpha > 0)
        {
            hintCanvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

    private IEnumerator Countdown(int seconds)
    {
        // 倒數計時的秒數
        int timer = seconds;
        // 使用while迴圈實現倒數計時
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f); // 等待1秒

            // 減少計時器的秒數
            timer--;
        }
    }

    public void colorRotateDetect()
    {
        if (correctRotate1)
        {
            rotate1Image.color = Color.green;
        }
        else
        {
            rotate1Image.color = Color.red;
        }


        if (correctRotate2)
        {
            rotate2Image.color = Color.green;
        }
        else
        {
            rotate2Image.color = Color.red;
        }

        if (correctRotate3)
        {
            rotate3Image.color = Color.green;
        }
        else
        {
            rotate3Image.color = Color.red;
        }
    }
}
