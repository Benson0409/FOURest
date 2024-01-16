using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueDataSo")]
public class DialogueDataSo : ScriptableObject
{
    public TextAsset[] textAsset;
    //當前的語句
    public int currentIndex;

    //任務進程判斷
    public int currentState;
    public int previousState;

    public int icon;
    //是否需要重複
    public bool repeatText;
    //此對話的任務狀態
    public bool missionState;
}
