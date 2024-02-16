using UnityEngine;

[CreateAssetMenu(menuName = "Narration/NarrationData")]
public class NarrationDataSo : ScriptableObject
{
    //判斷一開始的旁白是否有運作
    public bool isPlayAwake;
    //需要播放的旁白段落
    public TextAsset[] narrationTextAsset;
}