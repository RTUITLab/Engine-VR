using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
[HideInInspector]
public class VisablePart
{
    public MeshRenderer mesh;
    public Material startMaterial;
    public Collider collider;
    public VisablePart(MeshRenderer Mesh, Material StartMaterial, Collider Collider)
    {
        mesh = Mesh;
        startMaterial = StartMaterial;
        collider = Collider;
    }
    
}

public class AdditionalParts
{
    public MeshRenderer mesh;
    public AdditionalParts(MeshRenderer Mesh)
    {
        mesh = Mesh;
    }

}

public class FixedPart : MonoBehaviour
{
    private PhotonView photonView;

    public List<VisablePart> PartMeshes = new List<VisablePart>();
    public List<AdditionalParts> AddPartMeshes = new List<AdditionalParts>();

    [SerializeField] Material Highlited;
    //Material VisibleMaterial;

    public GameObject connectingPart;
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
                foreach (VisablePart visablePart in PartMeshes)
                {
                    visablePart.mesh.enabled = false;
                }
                gameObject.GetComponent<Collider>().enabled = false;
            }
            else if (value == Stage.Highlighted)
            {
                foreach (VisablePart visablePart in PartMeshes)
                {
                    visablePart.mesh.enabled = true;
                    visablePart.mesh.material = Highlited;
                }
                gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Collider>().isTrigger = true;
            }
            else if (value == Stage.Visable)
            {
                foreach (VisablePart visablePart in PartMeshes)
                {
                    visablePart.mesh.material = visablePart.startMaterial;
                }
                Destroy(GetComponent<Collider>());
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
                    currentStage = Stage.Visable;
                    photonView.RPC("EditStage", RpcTarget.OthersBuffered, (int)Stage.Visable);
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

