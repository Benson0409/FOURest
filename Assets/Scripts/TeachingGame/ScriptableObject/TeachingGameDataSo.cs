using UnityEngine;

[CreateAssetMenu(menuName = "TeachingGame/TeachingGameData")]
public class TeachingGameDataSo : ScriptableObject
{
    //每一個都增加變數的宣告看有沒有跳出教學面板
    //是否第一次進入新手關卡
    public bool isFirst;

    //移動教學
    public bool isMove;
    public bool isMoveTip;
    //視角教學
    public bool isView;
    public bool isViewTip;
    //Btn功能
    public bool isBtnActive;
    public bool isBtnTip;

    //撿起功能
    public bool isPick;
    public bool isDetect;
    //AR功能是否已經撿起
    public bool isARPick;

    //新手教學結束
    public bool isTeachingGameOver;
}
