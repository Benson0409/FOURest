using UnityEngine;

[CreateAssetMenu(menuName = "MainMenu/MainMenuGameData")]
public class MainMenuGameDataSo : ScriptableObject
{
    //紀錄是要開啟新遊戲還是進入先前的遊戲
    public bool creatNewGame;
}
