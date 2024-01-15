using UnityEngine;

[CreateAssetMenu(menuName = "GameData/CookieGameData")]
public class CookieGameDataSo : ScriptableObject
{
    public bool startCookieGame;
    public bool cookieGameOver;

    //判斷否已完成動畫播放,莉莉絲要出現
    public bool isPlayAnim;

    //判斷目前哪一個草叢已經還未被尋找
    public int findCookieCount;
    public bool isFindCookie;
    public bool cookie1Field;
    public bool cookie2Field;
    public bool cookie3Field;
}