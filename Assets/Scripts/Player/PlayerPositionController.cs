using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerPositionController : MonoBehaviour
{
    public GameObject player;
    //看先前有沒有紀錄過
    private bool isSave = false;

    private void Awake()
    {
        
        if (PlayerPrefs.GetInt("PlayerIsSave") == 1)
        {
            LoadPlayerPos();
            return;
        }

    }

    private void Update()
    {

        SavePlayerPos();
    }

    public void SavePlayerPos()
    {
        isSave = true;
        //紀錄移動過後的位置
        PlayerPrefs.SetFloat("playerPosX", player.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("playerPosY", player.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("playerPosZ", player.gameObject.transform.position.z);

        //紀錄旋轉的角度
        PlayerPrefs.SetFloat("playerRotaX", player.gameObject.transform.rotation.x);
        PlayerPrefs.SetFloat("playerRotaY", player.gameObject.transform.rotation.y);
        PlayerPrefs.SetFloat("playerRotaZ", player.gameObject.transform.rotation.z);


        //紀錄是否存過檔
        PlayerPrefs.SetInt("PlayerIsSave", isSave ? 1 : 0);

        PlayerPrefs.Save();

    }

    public void LoadPlayerPos()
    {
        //讀取player位置
        player.transform.position =
            new Vector3(
                PlayerPrefs.GetFloat("playerPosX"),
                PlayerPrefs.GetFloat("playerPosY"),
                PlayerPrefs.GetFloat("playerPosZ")
            );

        //讀取player旋轉角度
        player.transform.rotation = Quaternion.Euler(
            new Vector3(
               PlayerPrefs.GetFloat("playerRotaX"),
               PlayerPrefs.GetFloat("playerRotaY"),
               PlayerPrefs.GetFloat("playerRotaZ")
            )
        );
    }

    //public void SaveCamPos()
    //{
    //    isSave = true;
    //    //紀錄移動過後的位置
    //    PlayerPrefs.SetFloat("camPosX", cam.transform.position.x);
    //    PlayerPrefs.SetFloat("camPosY", cam.transform.position.y);
    //    PlayerPrefs.SetFloat("camPosZ", cam.transform.position.z);

    //    //紀錄旋轉的角度
    //    PlayerPrefs.SetFloat("camRotaX", cam.transform.rotation.x);
    //    PlayerPrefs.SetFloat("camRotaY", cam.transform.rotation.y);
    //    PlayerPrefs.SetFloat("camRotaZ", cam.transform.rotation.z);

    //    PlayerPrefs.Save();

    //}

    //public void LoadCamPos()
    //{
    //    //讀取player位置
    //    cam.transform.position =
    //        new Vector3(
    //            PlayerPrefs.GetFloat("camPosX"),
    //            PlayerPrefs.GetFloat("camPosY"),
    //            PlayerPrefs.GetFloat("camPosZ")
    //        );

    //    //讀取player旋轉角度
    //    cam.transform.rotation = Quaternion.Euler(
    //        new Vector3(
    //           PlayerPrefs.GetFloat("camRotaX"),
    //           PlayerPrefs.GetFloat("camRotaY"), 
    //           PlayerPrefs.GetFloat("camRotaZ")
    //        )
    //    );
    //}
}