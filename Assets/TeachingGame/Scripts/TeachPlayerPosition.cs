using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachPlayerPosition : MonoBehaviour
{
    public GameObject player;
    //看先前有沒有紀錄過
    private bool isSave = false;

    private void Awake()
    {

        if (PlayerPrefs.GetInt("PlayerTeachSave") == 1)
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
        PlayerPrefs.SetFloat("playerTeachPosX", player.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("playerTeachPosY", player.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("playerTeachPosZ", player.gameObject.transform.position.z);

        //紀錄旋轉的角度
        PlayerPrefs.SetFloat("playerTeachRotaX", player.gameObject.transform.rotation.x);
        PlayerPrefs.SetFloat("playerTeachRotaY", player.gameObject.transform.rotation.y);
        PlayerPrefs.SetFloat("playerTeachRotaZ", player.gameObject.transform.rotation.z);


        //紀錄是否存過檔
        PlayerPrefs.SetInt("PlayerTeachSave", isSave ? 1 : 0);

        PlayerPrefs.Save();

    }

    public void LoadPlayerPos()
    {
        //讀取player位置
        player.transform.position =
            new Vector3(
                PlayerPrefs.GetFloat("playerTeachPosX"),
                PlayerPrefs.GetFloat("playerTeachPosY"),
                PlayerPrefs.GetFloat("playerTeachPosZ")
            );

        //讀取player旋轉角度
        player.transform.rotation = Quaternion.Euler(
            new Vector3(
               PlayerPrefs.GetFloat("playerTeachRotaX"),
               PlayerPrefs.GetFloat("playerTeachRotaY"),
               PlayerPrefs.GetFloat("playerTeachRotaZ")
            )
        );
    }
}
