using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PuzzleGameData")]
public class PuzzleGameDataSo : ScriptableObject
{
    //紀錄目前撿起拼圖的狀態

    //是否開始收集拼圖
    public bool isFindPuzzle;

    //紀錄每一個拼圖的撿起狀況
    public bool[] puzzleState = new bool[6];

    //紀錄撿起拼圖的數量
    public int puzzleClipCount;

    //拼圖遊戲結束
    public bool puzzleGameOver;

    //看有沒有需要播放影片
    public bool isPlayAnim;
}
