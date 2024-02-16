using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    public bool hasPut;

    //完成之後，會傳訊息給manager讓他把該序列刪除
    public void OnPutRight()
    {
        hasPut = true;
        GridManager.instance.OnPutRight(this);
    }
}
