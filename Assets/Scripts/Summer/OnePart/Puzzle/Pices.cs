using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pices : MonoBehaviour
{
    public int index;
    public Vector3 startPos;
    SpriteRenderer icon;


    public bool followEnable = true;//可不可以拖移
    public bool hasPut;

    private int currentGrid = -1;
    private Grids triggerGrid;


    private Vector3 touchOffset; //觸摸物體中心偏移量
    private bool isBeingDragged = false; //是否在拖曳

    private void Start()
    {
        startPos = transform.position;
        icon = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        //希望只移動單一碎片，不要整個移動
        //轉換成手不偵測會有問題

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isBeingDragged && IsTouchingObject(touch.position))
                    {
                        touchOffset = transform.position - Camera.main.ScreenToWorldPoint(touch.position);
                        isBeingDragged = true;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isBeingDragged)
                    {
                        icon.sortingOrder = 100;
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        touchPos.z = 0;
                        transform.position = touchPos + touchOffset;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isBeingDragged)
                    {
                        isBeingDragged = false;
                        icon.sortingOrder = 0;

                        if (currentGrid >= 0)
                        {
                            if (currentGrid == index)
                            {
                                hasPut = true;
                                transform.position = triggerGrid.transform.position;

                                //放置好正確位置後傳送訊息
                                triggerGrid.OnPutRight();

                                //最後將自己本身給刪除，避免後續繼續移動
                                triggerGrid.transform.GetChild(0).gameObject.SetActive(true);
                                Destroy(this.gameObject);

                            }

                            else
                            {
                                transform.position = startPos;
                            }
                        }

                        else
                        {
                            transform.position = startPos;
                        }
                    }
                    break;
            }
            
        }
    }


    private bool IsTouchingObject(Vector2 touchPosition)
    {
        Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touchPosition);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPosWorld2D, Vector2.zero);

        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    ////如果還沒有被放置正確位置，就可以拖曳
    //private void OnMouseDown()
    //{
    //    if (hasPut == false)
    //    {
    //        followEnable = true;
    //    }
    //}

    ////鼠標按下後再拖動時觸發
    //private void OnMouseDrag()
    //{
    //    if (followEnable == false)
    //    {
    //        return;
    //    }

    //    //在拖曳的時候，將圖片保持在最上方
    //    icon.sortingOrder = 100;

    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;
    //    transform.position = mousePos;
    //}


    ////當滑鼠按鍵起來的時候
    //private void OnMouseUp()
    //{
    //    //將圖片的層級回歸正常
    //    icon.sortingOrder = 0;

    //    if (currentGrid >= 0)
    //    {
    //        if (currentGrid == index)
    //        {
    //            hasPut = true;
    //            transform.position = triggerGrid.transform.position;

    //            //放置好正確位置後傳送訊息
    //            triggerGrid.OnPutRight();

    //            //最後將自己本身給刪除，避免後續繼續移動
    //            triggerGrid.transform.GetChild(0).gameObject.SetActive(true);
    //            Destroy(this.gameObject);

    //        }

    //        else
    //        {
    //            transform.position = startPos;
    //        }
    //    }

    //    else
    //    {
    //        transform.position = startPos;
    //    }

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Grid")
        {
            //讀取格子上的grid腳本
            Grids grid = collision.GetComponent<Grids>();


            //判斷格子上面是否已經放置拼圖
            //如果上方已經有拼圖的話，grid.hasPut ＝ true
            if (!grid.hasPut)
            {
                //獲取資訊後，mouseUP裏面進行利用
                triggerGrid = grid;

                //將grid的值與碰撞物的編號相同
                currentGrid = int.Parse(collision.name);
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //確保有跟格子進行碰撞觸發
        //並取消觸發狀態
        if (triggerGrid != null && collision.gameObject == triggerGrid.gameObject)
        {
            triggerGrid = null;
            currentGrid = -1;
        }
    }
}
