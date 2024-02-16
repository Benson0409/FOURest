using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzlePices : MonoBehaviour
{


    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                this.gameObject.layer = LayerMask.NameToLayer("OutLine");
                return;
            }
        }
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
