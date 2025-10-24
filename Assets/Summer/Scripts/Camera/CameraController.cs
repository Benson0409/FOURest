using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{


    [Header("滑動區跨判斷")]
    public RectTransform scope;
    [SerializeReference] private float lookSpeed;
    private CinemachineFreeLook cinemachine;
    private Player playerInput;
    private Vector2 touchStartPos;
    public Vector2 lastPos = Vector2.zero;
    //紀錄初始的變化量，來判斷是往右還是往左


    //判斷目前對話有沒有開啟，開啟後不能移動
    public DialogueManager dialogueManager;
    public bool canLook;
    private void Awake()
    {
        playerInput = new Player();
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    //腳本啟用的時候，接受玩家的輸入
    private void OnEnable()
    {
        playerInput.Enable();

    }
    //腳本禁用時，禁用玩家輸入
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        //調整幀率到300
        Application.targetFrameRate = 300;
    }

    //希望可以解決向右滑動時突然向左滑的瞬間變化，讓這裡可以變得比較順暢
    void LateUpdate()
    {
        if (canLook)
        {
            if (Input.touchCount > 0)
            {

                foreach (Touch touch in Input.touches)
                {
                    Vector2 touchPosition = touch.position;

                    if (RectTransformUtility.RectangleContainsScreenPoint(scope, touchPosition))
                    {
                        switch (touch.phase)
                        {

                            case TouchPhase.Began:

                                touchStartPos = touch.position;
                                break;

                            case TouchPhase.Moved:

                                Vector2 touchDeltaPos;

                                touchDeltaPos = touch.position - touchStartPos;

                                print(touchDeltaPos.x + " : " + lastPos.x);
                                if (touchDeltaPos.x != 0 && touchDeltaPos != lastPos && !dialogueManager.startDialogue)
                                {

                                    // float rotationAmount = inputLimit.x * lookSpeed * Time.deltaTime * 10;
                                    // cinemachine.m_XAxis.Value += rotationAmount;
                                    if ((touchDeltaPos.x < lastPos.x && touchDeltaPos.x > 0.1f) || (touchDeltaPos.x > lastPos.x && touchDeltaPos.x < 0.1f))
                                    {
                                        float targetValue = cinemachine.m_XAxis.Value + touchDeltaPos.x * lookSpeed * Time.deltaTime * 3.5f;
                                        cinemachine.m_XAxis.Value -= Mathf.Lerp(cinemachine.m_XAxis.Value, targetValue, lookSpeed * Time.deltaTime);
                                    }
                                    else
                                    {
                                        float targetValue = cinemachine.m_XAxis.Value + touchDeltaPos.x * lookSpeed * Time.deltaTime * 3.5f;
                                        cinemachine.m_XAxis.Value += Mathf.Lerp(cinemachine.m_XAxis.Value, targetValue, lookSpeed * Time.deltaTime);
                                    }

                                    //cinemachine.m_XAxis.Value += inputLimit.x * lookSpeed * Time.deltaTime * 10;

                                    //上一秒的移動
                                    lastPos = touchDeltaPos;

                                }
                                break;

                            case TouchPhase.Ended:
                                break;

                        }

                    }
                }
            }
        }
    }
    // private void LateUpdate()
    // {
    //     if (canLook)
    //     {
    //         //如果我在正確的地方運行，我就可以讓一個值為true,代表我目前可以旋轉
    //         Vector2 inputLimit = playerInput.PlayerMain.Look.ReadValue<Vector2>();
    //         // //在左右切換時，將inputLimit 以及lastpos都設為０就可以避免那段緩衝時間

    //         if (inputLimit.x != 0 && inputLimit != lastPos && !dialogueManager.startDialogue)
    //         {

    //             // float rotationAmount = inputLimit.x * lookSpeed * Time.deltaTime * 10;
    //             // cinemachine.m_XAxis.Value += rotationAmount;


    //             float targetValue = cinemachine.m_XAxis.Value + inputLimit.x * lookSpeed * Time.deltaTime * 5;
    //             cinemachine.m_XAxis.Value += Mathf.Lerp(cinemachine.m_XAxis.Value, targetValue, lookSpeed * Time.deltaTime);


    //             //cinemachine.m_XAxis.Value += inputLimit.x * lookSpeed * Time.deltaTime * 10;

    //             //上一秒的移動
    //             lastPos = inputLimit;
    //         }

    //     }
    // }
    public void PointDetect()
    {
        canLook = true;
    }

    public void PointCancel()
    {
        canLook = false;
    }


    // public void touchMove()
    // {
    //     if (Input.touchCount > 0)
    //     {

    //         foreach (Touch touch in Input.touches)
    //         {
    //             Vector2 touchPosition = touch.position;

    //             if (RectTransformUtility.RectangleContainsScreenPoint(scope, touchPosition))
    //             {
    //                 switch (touch.phase)
    //                 {

    //                     case TouchPhase.Began:

    //                         touchStartPos = touch.position;
    //                         break;

    //                     case TouchPhase.Moved:

    //                         Vector2 touchDeltaPos;

    //                         touchDeltaPos = touch.position - touchStartPos;

    //                         print(touchDeltaPos.x + " : " + lastPos.x);
    //                         if (touchDeltaPos.x != 0 && touchDeltaPos != lastPos && !dialogueManager.startDialogue)
    //                         {

    //                             // float rotationAmount = inputLimit.x * lookSpeed * Time.deltaTime * 10;
    //                             // cinemachine.m_XAxis.Value += rotationAmount;
    //                             if ((touchDeltaPos.x < lastPos.x && touchDeltaPos.x > 0) || (touchDeltaPos.x > lastPos.x && touchDeltaPos.x < 0))
    //                             {
    //                                 float targetValue = cinemachine.m_XAxis.Value + touchDeltaPos.x * lookSpeed * Time.deltaTime * 5;
    //                                 cinemachine.m_XAxis.Value -= Mathf.Lerp(cinemachine.m_XAxis.Value, targetValue, lookSpeed * Time.deltaTime);
    //                             }
    //                             else
    //                             {
    //                                 float targetValue = cinemachine.m_XAxis.Value + touchDeltaPos.x * lookSpeed * Time.deltaTime * 5;
    //                                 cinemachine.m_XAxis.Value += Mathf.Lerp(cinemachine.m_XAxis.Value, targetValue, lookSpeed * Time.deltaTime);
    //                             }

    //                             //cinemachine.m_XAxis.Value += inputLimit.x * lookSpeed * Time.deltaTime * 10;

    //                             //上一秒的移動
    //                             lastPos = touchDeltaPos;

    //                         }
    //                         break;

    //                     case TouchPhase.Ended:
    //                         break;

    //                 }

    //             }
    //         }
    //     }
    // }
}
