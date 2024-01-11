using UnityEngine;

[CreateAssetMenu(menuName = "GameData/CookieGameData")]
public class CookieGameDataSo : ScriptableObject
{
    public bool startCookieGame;
    public bool cookieGameOver;

    //判斷目前哪一個草叢已經還未被尋找
    public int findCookieCount;
    public bool cookie1Field;
    public bool cookie2Field;
    public bool cookie3Field;
}