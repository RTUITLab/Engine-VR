using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Part : MonoBehaviour
{
    [HideInInspector] public FixedPart connecntedFixed;
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
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
