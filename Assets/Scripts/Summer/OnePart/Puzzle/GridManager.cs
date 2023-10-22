using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public List<Grids> allGrid;



    private void Awake()
    {
        instance = this;
    }

    //當放置位置正確
    public void OnPutRight(Grids grid)
    {
        allGrid.Remove(grid);

        if (allGrid.Count == 0)
        {
            print("拼圖完成");

            PuzzleGameController.puzzleGameOver = true;
        }
    }
}
