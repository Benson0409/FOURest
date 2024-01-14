using UnityEngine;

[CreateAssetMenu(menuName = "GameData/ColorGameData")]
public class ColorGameDataSo : ScriptableObject
{
    //關卡判斷
    public bool startColorGame;
    public bool colorGameOver;

    //三色鏡
    public bool startColorMirrorGame;
    public bool isRotate;

    //調色盤
    public bool startFilterGame;

    //可以開始尋找水晶球
    public bool startFindCrystalBall;
    public bool isFindCrystalBall;
}
