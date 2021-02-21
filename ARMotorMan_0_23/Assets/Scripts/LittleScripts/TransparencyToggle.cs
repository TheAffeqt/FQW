using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyToggle : MonoBehaviour
{
    Material material;
    Color color;
    float TempColorG;

    [SerializeField]
    float opaqueValue = 1.0f;
    [SerializeField]
    float transparencyValue;

    private void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        color = material.color;
        TempColorG = color.g;
    }

    public void Toggle()
    {   
        if (color.g != 1.0f)
        {
            color.g = 1.0f;
        }
        else
        {
            color.g = TempColorG;
        }
        material.SetColor("_color", color);
    }

}
