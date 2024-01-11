using UnityEngine;

[CreateAssetMenu(menuName = "GameData/TempleGameData")]
public class TempleGameDataSo : ScriptableObject
{
    //神廟區域的關卡控制
    public bool startTempleGame;
    public bool templeGameOver;

    //大門關卡判斷
    public bool startDoorGame;

    //大門進入AR的判斷
    public bool findDoorClue;

    //音樂寶箱判斷
    public bool startMusicGame;
    public bool finishMusicGame;
    //樂譜進入AR的判斷
    public bool findMusicClue;
}
