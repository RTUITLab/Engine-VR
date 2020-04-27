using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FixedPart : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField] Material Highlited;
    Material VisibleMaterial;

    [SerializeField] GameObject connectingPart;
    private Part part;
    private SyncTranshorm syncTranshorm;

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
        photonView = gameObject.GetComponent<PhotonView>();
        syncTranshorm = connectingPart.GetComponent<SyncTranshorm>();
        VisibleMaterial = GetComponent<MeshRenderer>().material;
        currentStage = Stage.Hidden;
        IsReadyToMove = false;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
        part = connectingPart.GetComponent<Part>();
        part.connecntedFixed = GetComponent<FixedPart>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == connectingPart && syncTranshorm.isLastOwner())
        {
            if (IsReadyToMove)
            {
                syncTranshorm.SendGrav(false);
                connectingPart.transform.position = Vector3.MoveTowards(connectingPart.transform.position, transform.position, Time.deltaTime);
                connectingPart.transform.rotation = Quaternion.RotateTowards(connectingPart.transform.rotation, transform.rotation, Time.deltaTime * 360);
                if (connectingPart.transform.position == transform.position && connectingPart.transform.rotation == transform.rotation)
                {
                    photonView.RPC("EditStage", RpcTarget.All, (int)Stage.Visable);
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == connectingPart && syncTranshorm.isLastOwner())
        {
            syncTranshorm.SendGrav(true);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC] private void EditStage(int StageNum)
    {
        currentStage = (Stage)StageNum;
    }
    
}

