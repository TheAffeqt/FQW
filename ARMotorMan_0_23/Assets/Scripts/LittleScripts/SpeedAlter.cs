using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAlter : MonoBehaviour
{

    [SerializeField]
    Animator animator;
    [SerializeField]
    float animationSpeed;
    void Awake()
    {
        animator.speed = animationSpeed;
    }
}
