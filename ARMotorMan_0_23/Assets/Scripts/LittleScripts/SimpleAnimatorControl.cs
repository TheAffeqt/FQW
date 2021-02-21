using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimatorControl : MonoBehaviour
{
    [SerializeField]
    Animator animatorController;

    [SerializeField]
    string paramToSwitch;

    public void Switch()
    {
        animatorController.SetBool(paramToSwitch, !animatorController.GetBool(paramToSwitch));
    }
}
