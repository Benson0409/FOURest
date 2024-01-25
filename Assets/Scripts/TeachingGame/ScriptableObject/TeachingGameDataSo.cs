using UnityEngine;

[CreateAssetMenu(menuName = "TeachingGame/TeachingGameData")]
public class TeachingGameDataSo : ScriptableObject
{
    //是否第一次進入新手關卡
    public bool isFirst;

    //移動教學
    public bool isMove;

    //視角教學
    public bool isView;

    //Btn功能
    public bool isBtnActive;

    //撿起功能
    public bool isPick;

    //新手教學結束
    public bool isTeachingGameOver;
}
