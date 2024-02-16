using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
    //用來判斷有沒有手指點擊螢幕，如果有的話我們就進入遊戲
    [Header("場景轉場物體")]
    public SwitchScenes scenesCanvaPrefabs;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchPhase touchPhase = Input.GetTouch(0).phase;
            switch (touchPhase)
            {
                case TouchPhase.Ended:
                    PlayGame();
                    break;
            }
        }
    }

    private void PlayGame()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Menu"));
    }
}
