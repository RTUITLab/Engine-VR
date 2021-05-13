using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
[HideInInspector]
public class VisablePart
{
    public MeshRenderer mesh;
    public Material[] startMaterials;
    public Collider collider;
    public DisolveScript disolve;
    public VisablePart(MeshRenderer Mesh, Material[] StartMaterials, Collider Collider)
    {
        mesh = Mesh;
        startMaterials = StartMaterials;
        collider = Collider;
    }

    public VisablePart(MeshRenderer Mesh, Material[] StartMaterials, Collider Collider, DisolveScript Disolve)
    {
        mesh = Mesh;
        startMaterials = StartMaterials;
        collider = Collider;
        disolve = Disolve;
    }

}

public class FixedPart : MonoBehaviour
{
    private PhotonView photonView;

    //public List<VisablePart> PartMeshes = new List<VisablePart>();
    //public List<VisablePart> AddPartMeshes = new List<VisablePart>();
    public VisablePart PartMeshe;
    public VisablePart AddPartMeshe;

    public Material Highlited;

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
                PartMeshe.collider.enabled = false;
                PartMeshe.mesh.enabled = false;

                if (AddPartMeshe.disolve != null)
                {
                    AddPartMeshe.collider.enabled = false;
                    AddPartMeshe.mesh.enabled = false;
                }


                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            else if (value == Stage.Highlighted)
            {
                PartMeshe.mesh.enabled = true;
                Material[] materials = PartMeshe.mesh.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = Highlited;
                }
                PartMeshe.mesh.materials = materials;

                gameObject.GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                connectingPart.GetComponent<Part>().currentStage = Part.Stage.Active;

            }
            else if (value == Stage.Visable)
            {
                PartMeshe.mesh.materials = PartMeshe.startMaterials;
                //PartMeshe.collider.enabled = true;

                if (AddPartMeshe.disolve != null)
                {
                    AddPartMeshe.mesh.enabled = true;
                    //AddPartMeshe.collider.enabled = true;
                    AddPartMeshe.disolve.DisolveProcess(false);
                }

                Destroy(GetComponent<BoxCollider>());
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
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ConstractingManager>();
        currentStage = Stage.Hidden;
        IsReadyToMove = false;
        part = connectingPart.GetComponent<Part>();
        part.connecntedFixed = GetComponent<FixedPart>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "smallPart" && syncTranshorm.isLastOwner())
        {
            if (IsReadyToMove)
            {
                syncTranshorm.SendGrav(false);
                connectingPart.transform.position = Vector3.MoveTowards(connectingPart.transform.position, transform.position, Time.deltaTime * 2);
                connectingPart.transform.rotation = Quaternion.RotateTowards(connectingPart.transform.rotation, transform.rotation, Time.deltaTime * 360 * 2);

                /*if (connectingPart.transform.position == transform.position && connectingPart.transform.rotation == transform.rotation)
                {
                    currentStage = Stage.Visable;
                    photonView.RPC("EditStage", RpcTarget.OthersBuffered, (int)Stage.Visable);
                }*/
                if ((connectingPart.transform.position - transform.position).magnitude < 0.1f && (connectingPart.transform.eulerAngles - transform.eulerAngles).magnitude < 0.1f)
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
            StartCoroutine(TeleportConnectingPart());
        }
    }

    private IEnumerator TeleportConnectingPart()
    {
        yield return new WaitForSeconds(0.3f);
        if (connectingPart != null)
        {
            connectingPart.GetComponent<Part>().SetInitialPosition();
            connectingPart.SetActive(true);
            syncTranshorm.SendGrav(true);
        }

    }

    public void SetConnected(GameObject connected)
    {
        connectingPart = connected;
        //connected.GetComponent<Part>().part = this;
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

