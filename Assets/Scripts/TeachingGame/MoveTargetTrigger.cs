using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetTrigger : MonoBehaviour
{
    public TeachingGameDataSo teachingGameData;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("到達目的地");
            teachingGameData.isMove = true;
        }
    }
}
