using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Part : MonoBehaviour
{
    
    [HideInInspector] public enum Stage { Hidden , Visable , Active}
    Stage _currentStage;
    public Transform Hint;
    Transform camera;
    ConstractingManager manager;

    private Vector3 partPosition;
    private Quaternion partRotation;
    //public FixedPart part;

    private HintManager hintManager;
    public string HintText;

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

    Vector3 LastPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (Hint)
            Hint.LookAt(camera);
        
        Vector3 diff = transform.position - LastPos;
        if (LastPos != Vector3.zero)
        {
            if (Hint)
                Hint.transform.position += diff;
        }
        LastPos = transform.position;
        //Debug.Log(currentStage);
    }

    private void OnEnable()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
        hintManager = FindObjectOfType<HintManager>();

        if (manager.Education)
        {
            if (Hint)
                Hint.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (Hint.gameObject) Hint.gameObject.SetActive(false);
    }

    private void Start()
    {
        //Hint.transform.parent = transform;
        partPosition = transform.position;
        partRotation = transform.rotation;
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
    }

    

    public void SetInitialPosition()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = true;
        transform.SetPositionAndRotation(partPosition, partRotation);
    }

    public void DisplayHint()
    {
        hintManager.DisplayHint(HintText);
    }

    public void StopDisplaying()
    {
        hintManager.StopDisplaying();
    }
}
