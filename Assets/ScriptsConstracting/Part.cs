using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [HideInInspector] public FixedPart connecntedFixed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void IsReadyChanger(bool IsReady)
    {
        connecntedFixed.IsReadyToMove = IsReady;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
