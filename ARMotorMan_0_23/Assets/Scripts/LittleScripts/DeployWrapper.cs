using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using MagicLeap;
using TSUXRLab;

public class DeployWrapper : MonoBehaviour
{
    Collider[] allColliders;

    private void Awake()
    {
        allColliders = transform.GetComponentsInChildren<Collider>();
        SwitchColliders(false);
    }

    void SwitchColliders(bool status)
    {
        foreach (Collider col in allColliders)
        {
            col.enabled = status;
        }
    }

    public void UnWrap()
    {
        SwitchColliders(true);
    }
}
