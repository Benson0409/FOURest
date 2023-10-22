using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cookieGameController : MonoBehaviour
{
    public DialogueManager dialogueManager;
    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    private TempleGameController templeGameController;
    private SummerGameController summerGameController;

    [Header("餅乾遊戲變量")]
    public static bool finishCookieGame;
    private bool startCookieGame;

    [Header("餅乾遊戲生成")]
    public GameObject cookieField;

    [Header("偵測物體範圍")]
    public GameObject player;
    public float radius;

    [Header("AR生成物控制")]
    //public bool openAR;
    public bool cookie1;
    public bool cookie2;
    public bool cookie3;
    public int findCookieCount = 0;

    [Header("按鈕控制")]
    public GameObject DetectObject;
    public Text DetectBtn;

    [Header("角色生成")]
    public GameObject lilisi;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("startCookieGame") == 1 )
        {
            if (PlayerPrefs.GetInt("finishCookieGame") == 1)
            {
                finishCookieGame = true;
            }
            startCookieGame = true;
            cookieField.SetActive(true);
        }
        else
        {
           startCookieGame = false;
        }

        findCookieGameDetect();
        templeGameController = GetComponent<TempleGameController>();
        summerGameController = GetComponent<SummerGameController>();
        
    }

    void Update()
    {
        //偵測有沒有靠近餅乾區域
        //開啟BTN -> 按下進入ＡＲ場景
        //需繼承點擊的物體給ＡＲ生成
        //要做好數據控管，因為是場景切換
        //以及餅乾的數據控管

        if (finishCookieGame)
        {
            return;
        }

        if (cookie1 && cookie2 && cookie3 && !finishCookieGame)
        {
            //莉莉絲出現
            //餅乾遊戲結束，進入下一部分，神殿開啟，找尋樂譜
            //引導去找莉莉絲對話
            if (!lilisi.activeInHierarchy)
            {
                summerGameController.openNarrationSystem();
                lilisi.SetActive(true);
            }
            if (dialogueManager.startDialogue)
            {
                finishCookieGame = true;
                PlayerPrefs.SetInt("finishCookieGame", 1);
                PlayerPrefs.Save();
                templeGameController.startGame();
            }
        }


        if (startCookieGame && !finishCookieGame)
        {
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);

            //如果player的狀態屬於 false 就不執行偵測的動作 
            if (player.activeInHierarchy == false)
            {
                return;
            }

            foreach(Collider collider in colliders)
            {
                if(collider.gameObject.tag == "cookie1AR" && !cookie1)
                {
                    DetectObject.SetActive(true);
                    DetectBtn.text = "探索區域1";
                    ARSystem.cookie1Field(collider.gameObject);
                    return;
                }

                if (collider.gameObject.tag == "cookie2AR" && !cookie2)
                {
                    DetectObject.SetActive(true);
                    DetectBtn.text = "探索區域2";
                    ARSystem.cookie2Field(collider.gameObject);
                    return;
                }

                if (collider.gameObject.tag == "cookie3AR" && !cookie3)
                {
                    DetectObject.SetActive(true);
                    DetectBtn.text = "探索區域3";
                    ARSystem.cookie3Field(collider.gameObject);
                    return;
                }

                DetectObject.SetActive(false);
            }
            
        }

      
        

      
    }

    public void startGame()
    {
        if (!PuzzleGameController.puzzleGameOver)
        {
            //代表上一關還沒結束
            return;
        }
        else
        {
            //顯示找餅乾區域
            startCookieGame = true;
            saveStartCookieGame();
            cookieField.SetActive(true);

        }
    }

    public void findCookieGameDetect()
    {
        if (PlayerPrefs.GetInt("findCookie1") == 1)
        {
            cookie1 = true;
            findCookieCount++;
        }
        else
        {
            cookie1 = false;
        }

        if (PlayerPrefs.GetInt("findCookie2") == 1)
        {
            cookie2 = true;
            findCookieCount++;
        }
        else
        {
            cookie2 = false;
        }

        if (PlayerPrefs.GetInt("findCookie3") == 1)
        {
            cookie3 = true;
            findCookieCount++;
        }
        else
        {
            cookie3 = false;
        }
    }

    public void saveStartCookieGame()
    {
        PlayerPrefs.SetInt("startCookieGame", 1);
        PlayerPrefs.Save();
    }

    public void findCookieGameBtn()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("TestARCookieScene"));
    }

}
