using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{

    //要再把對話系統的判別加進來

    [SerializeReference] private float lookSpeed;
    private CinemachineFreeLook cinemachine;
    private Player playerInput;
    public Vector2 lastPos = Vector2.zero;
    //紀錄初始的變化量，來判斷是往右還是往左


    //判斷目前對話有沒有開啟，開啟後不能移動
    public DialogueManager dialogueManager;


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
    private void LateUpdate()
    {
        //如果我在正確的地方運行，我就可以讓一個值為true,代表我目前可以旋轉
        Vector2 inputLimit = playerInput.PlayerMain.Look.ReadValue<Vector2>();

        if (inputLimit.x != 0 && inputLimit != lastPos && !dialogueManager.startDialogue)
        {

            cinemachine.m_XAxis.Value += inputLimit.x * lookSpeed * Time.deltaTime * 10;

            lastPos = inputLimit;
        }

    }
}
