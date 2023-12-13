using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiLiController : MonoBehaviour
{
    //控制莉莉絲的位置以及莉莉絲的存在狀況

    public GameObject LiLi;
    public Transform liliInitial;

    private void Awake()
    {
        // //print(PlayerPrefs.GetInt("555"));
        // if (PlayerPrefs.GetInt("LiLiIsSave") == 1)
        // {
        //     // print("123");
        //     return;
        // }
        // LiLi.SetActive(false);
        // PlayerPrefs.SetInt("LiLiIsSave", 1);
        // PlayerPrefs.Save();
        if (PlayerPrefs.GetInt("LiLiIsSave") == 1)
        {
            loadLiLiPosition();
            loadLiLiState();
            return;
        }
        LiLi.SetActive(false);
        liliInitial.transform.position = liliInitial.position;
        liliInitial.transform.rotation = liliInitial.rotation;
    }

    private void Update()
    {
        if (LiLi.activeInHierarchy)
        {
            saveLiLiPosition();
            saveLiLiState();
        }
    }

    //儲存莉莉絲的位置
    public void saveLiLiPosition()
    {
        //紀錄移動過後的位置
        PlayerPrefs.SetFloat("LiLiPosX", LiLi.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("LiLiPosY", LiLi.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("LiLiPosZ", LiLi.gameObject.transform.position.z);

        //紀錄旋轉的角度
        PlayerPrefs.SetFloat("LiLiRotaX", LiLi.gameObject.transform.rotation.x);
        PlayerPrefs.SetFloat("LiLiRotaY", LiLi.gameObject.transform.rotation.y);
        PlayerPrefs.SetFloat("LiLiRotaZ", LiLi.gameObject.transform.rotation.z);


        //紀錄是否存過檔
        PlayerPrefs.SetInt("LiLiIsSave", 1);

        PlayerPrefs.Save();
    }

    public void loadLiLiPosition()
    {
        //讀取player位置
        LiLi.transform.position =
            new Vector3(
                PlayerPrefs.GetFloat("LiLiPosX"),
                PlayerPrefs.GetFloat("LiLiPosY"),
                PlayerPrefs.GetFloat("LiLiPosZ")
            );

        //讀取player旋轉角度
        LiLi.transform.rotation = Quaternion.Euler(
            new Vector3(
               PlayerPrefs.GetFloat("LiLiRotaX"),
               PlayerPrefs.GetFloat("LiLiRotaY"),
               PlayerPrefs.GetFloat("LiLiRotaZ")
            )
        );
    }

    //儲存莉莉絲的存在狀態
    public void saveLiLiState()
    {
        //莉莉絲顯示
        if (LiLi.activeInHierarchy == true)
        {
            PlayerPrefs.SetInt("LiLiState", 1);
        }
        //莉莉絲不顯示
        else if (LiLi.activeInHierarchy != true)
        {
            PlayerPrefs.SetInt("LiLiState", 0);
        }
    }

    public void loadLiLiState()
    {
        if (PlayerPrefs.GetInt("LiLiState") == 1)
        {
            LiLi.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("LiLiState") == 0)
        {
            LiLi.SetActive(false);
        }
    }
}
