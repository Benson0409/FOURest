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
    public Text dialogueBtnText;
    public DialogueManager dialogueManager;


    void Update()
    {
        //看附近是否有可以對話的人物，可以的話開啟對話

        if (!dialogueManager.btnClose)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, sectionMask);

            foreach (Collider collider in colliders)
            {
                //最後一章,只能跟水仙子對話
                if(collider != null && PlayerPrefs.GetInt("finishColorGame") == 1 && collider.gameObject.tag == "water")
                {
                    print("可以開啟對話");
                    dialogueBtn.SetActive(true);
                    dialogueBtnText.text = "對話";
                    return;
                }

                if (collider != null && PlayerPrefs.GetInt("finishColorGame") != 1)
                {
                    print("可以開啟對話");
                    dialogueBtn.SetActive(true);
                    dialogueBtnText.text = "對話";
                    return;
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
