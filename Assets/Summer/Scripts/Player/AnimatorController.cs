using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimatorController : MonoBehaviour
{
    public Animator animator;
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetFloat("Walk", playerController.move.magnitude);
    }
}
