using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Part : MonoBehaviour
{
    [HideInInspector] public FixedPart connecntedFixed;
    public void IsReadyChanger(bool IsReady)
    {
        connecntedFixed.IsReadyToMove = IsReady;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
