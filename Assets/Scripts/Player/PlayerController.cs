using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //要在補充對話系統的判斷

    //----------對話系統設定-----------
    //判斷目前對話有沒有開啟，開啟後不能移動

    public DialogueManager dialogueManager;
    public narrationSystem narrationSystem;
    private float speed;


    //----------Input system---------
    //input system 的利用
    private Player playerInput;

    //----------變數-------------
    private Rigidbody rb;
    private Vector3 movementInput;

    private Vector3 playerVelocity;

    //相機
    private Transform cameraMain;

    //----------操作設定------------
    [Header("變量設定")]
    //人物移動速度
    [SerializeField] private float playerSpeed = 2.0f;


    //人物旋轉速度
    [SerializeField] private float rotationSpeed = 4;

    //爬樓梯力量
    public float climbStairForce;

    //人物跳躍高度
    //[SerializeField] private float jumpHeight = 1.0f;

    //-----------重力設定------------

    //重力影響
    [SerializeField] private float gravityValue = -9.81f;

    //重力加成數值
    [SerializeField] private float gravityMultiple;
    private float initGravityMultiple;

    //爬樓梯時將重力關閉
    //太高的物品不要觸發爬樓提功能
    public Transform lowDetect;
    public Transform highDetect;

    private void Awake()
    {
        playerInput = new Player();
        rb = GetComponent<Rigidbody>();

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
        cameraMain = Camera.main.transform;
        speed = playerSpeed;
        initGravityMultiple = gravityMultiple;
    }

    private void Update()
    {

        movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        if (stairsDetect() && movementInput.magnitude > 0.1f)
        {
            print("爬樓梯");
            rb.AddForce(Vector3.up * 20 * climbStairForce, ForceMode.Impulse);
            gravityMultiple = 0;
        }
        else
        {
            gravityMultiple = initGravityMultiple;
        }


        if (dialogueManager.startDialogue || narrationSystem.startDialogue)
        {
            playerSpeed = 0;
        }
        else
        {
            playerSpeed = speed;
        }

    }

    void FixedUpdate()
    {
        Vector3 move = cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x;
        move.y = 0;

        rb.velocity = move.normalized * playerSpeed * 100 * Time.deltaTime;


        //角色的整個物件(transform)轉向移動方向(move)，也就是面向移動的方向，這樣可以讓玩家更容易判斷角色的移動方向
        //move.magnitude 表示向量 move 的长度,移動方向的大小
        //只有当移动方向的大小大于0.1时，才会执行旋转操作，避免在微小的移动方向下无谓的旋转
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }


        //------------跳躍功能-------------
        //Changes the height position of the player.
        //if (playerInput.PlayerMain.Jump.triggered && IsGrounded())
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}



        // Apply gravity
        if (IsGrounded())
        {
            // 物體在地面上，直接將重力加速度歸零
            gravityValue = 0f;
        }
        else
        {
            // 物體離開地面，繼續受到恆定的重力加速度
            gravityValue += Physics.gravity.y * gravityMultiple;
        }
        // 將重力加速度套用到物體上
        rb.AddForce(new Vector3(0f, gravityValue, 0f), ForceMode.Acceleration);

    }

    //爬樓梯判斷
    private bool stairsDetect()
    {
        if (Physics.Raycast(lowDetect.position, Vector3.forward, GetComponent<Collider>().bounds.extents.y + 0.1f))
        {
            if (!Physics.Raycast(highDetect.position, Vector3.forward, GetComponent<Collider>().bounds.extents.y + 0.1f))
            {
                return true;
            }
        }
        return false;

    }

    //如果偵測到有collider的物體，才判斷在地面
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

}

