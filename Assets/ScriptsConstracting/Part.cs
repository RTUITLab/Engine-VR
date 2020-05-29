using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Part : MonoBehaviour
{
    [HideInInspector] public enum Stage { Hidden , Visable , Active}
    Stage _currentStage;
    //ConstractingManager manager;
    //public FixedPart part;

    public Stage currentStage
    {
        get
        {
            return _currentStage;
        }
        set
        {
            _currentStage = value;
            if (value == Stage.Hidden)
            {
                gameObject.SetActive(false);
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (value == Stage.Visable)
            {
                transform.parent.gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(true);
                GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    [HideInInspector] public FixedPart connecntedFixed;
    public void IsReadyChanger(bool IsReady)
    {
        connecntedFixed.IsReadyToMove = IsReady;

    }

    public void EnablePhys()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        //manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
    }
}
