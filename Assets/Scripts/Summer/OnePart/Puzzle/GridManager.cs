using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public PuzzleGameDataSo puzzleGameData;
    public static GridManager instance;
    public List<Grids> allGrid;

    [Header("動畫轉場")]
    public SwitchScenes scenesCanvaPrefabs;

    private void Awake()
    {
        instance = this;
    }

    //當放置位置正確
    //拼圖完成後轉場到 Environment 場景播放動畫 播放完畢再轉場回來
    public void OnPutRight(Grids grid)
    {
        allGrid.Remove(grid);

        if (allGrid.Count == 0)
        {
            print("拼圖完成");

            puzzleGameData.puzzleGameOver = true;

            PlayAnim();
        }
    }

    private void PlayAnim()
    {
        SwitchScenes switchScenes = Instantiate(scenesCanvaPrefabs);
        switchScenes.StartCoroutine(switchScenes.loadFadeOutInScenes("Environment"));
    }
}
