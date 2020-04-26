using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPart : MonoBehaviour
{
    [SerializeField] Material Highlited;
    Material VisibleMaterial;

    [SerializeField] GameObject connectingPart;

    [HideInInspector] public bool IsReadyToMove;

    ConstractingManager manager;

    [HideInInspector] public enum Stage { Hidden, Highlighted, Visable }
    Stage _currentStage;

    public Stage currentStage
    {
        get
        {
            return _currentStage;
        }
        set
        {
            _currentStage = value; 
            if(value == Stage.Hidden)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
            }
            else if (value == Stage.Highlighted)
            {
                gameObject.GetComponent<MeshRenderer>().material = Highlited;
                gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<Collider>().isTrigger = true;
            }
            else if (value == Stage.Visable)
            {
                gameObject.GetComponent<MeshRenderer>().material = VisibleMaterial;
                gameObject.GetComponent<Collider>().isTrigger = false;
                connectingPart.SetActive(false);
                manager.NextFixedPart();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        VisibleMaterial = GetComponent<MeshRenderer>().material;
        currentStage = Stage.Hidden;
        IsReadyToMove = false;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
        connectingPart.GetComponent<Part>().connecntedFixed = GetComponent<FixedPart>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == connectingPart)
        {
            if (IsReadyToMove)
            {
                connectingPart.GetComponent<Rigidbody>().useGravity = false;
                connectingPart.transform.position = Vector3.MoveTowards(connectingPart.transform.position, transform.position, Time.deltaTime);
                connectingPart.transform.rotation = Quaternion.RotateTowards(connectingPart.transform.rotation, transform.rotation, Time.deltaTime * 360);
                if (connectingPart.transform.position == transform.position && connectingPart.transform.rotation == transform.rotation)
                {
                    currentStage = Stage.Visable;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == connectingPart)
        {
            connectingPart.GetComponent<Rigidbody>().useGravity = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    
}

