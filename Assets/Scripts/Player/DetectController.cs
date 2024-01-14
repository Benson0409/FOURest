using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectController : MonoBehaviour
{

    //要多少距離才可以發現線索
    public float radius;
    public LayerMask sectionMask;
    public GameObject dialogueBtn;
    public DialogueManager dialogueManager;
    [Header("遊戲數據")]
    public ColorGameDataSo colorGameData;


    void Update()
    {
        //看附近是否有可以對話的人物，可以的話開啟對話

        if (!dialogueManager.btnClose)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, sectionMask);

            foreach (Collider collider in colliders)
            {
                if (collider != null)
                {
                    //最後一章,只能跟水仙子對話
                    if (colorGameData.colorGameOver && collider.gameObject.tag == "water")
                    {
                        print("可以開啟對話");
                        dialogueBtn.SetActive(true);
                        return;
                    }

                    if (PlayerPrefs.GetInt("finishColorGame") != 1)
                    {
                        print("可以開啟對話");
                        DialogueDataSo currentData = collider.GetComponent<DialogueSetting>().dialogueData;
                        dialogueBtn.SetActive(true);
                        dialogueManager.ReadTextAsset(currentData);
                        return;
                    }
                }
            }
        }
        //如果身邊沒有掃到section的部分，就將調查扭關閉
        dialogueBtn.SetActive(false);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
