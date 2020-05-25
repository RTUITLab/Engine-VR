using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ConstractingManager : MonoBehaviour
{
    [SerializeField] List<List<FixedPart>> fixedParts = new List<List<FixedPart>>();
    [SerializeField] DataBase dataBase;
    [SerializeField] GameObject MovingPartExample;
    [SerializeField] Transform spawnPoint;
    int ID = 20;


    int currentFixedPartIndex;
    // Start is called before the first frame update
    void Start()
    {
        //fixedParts.Add(new List<FixedPart>());
        Invoke("StartFunc", 1);
    }

    void StartFunc()
    {
        foreach(DataBase.ObjStructure structure in dataBase.objStructures)
        {
            GameObject part = GameObject.Find(structure.name);
            int depth = structure.depth.count;
            FixedPart fixedPart;

            while (depth > fixedParts.Count - 1)
            {
                fixedParts.Add(new List<FixedPart>());
            }

            GameObject connectedPart = Instantiate(part, spawnPoint.position, spawnPoint.rotation);
            GameObject connectedPartRoot = Instantiate(MovingPartExample, spawnPoint.position, spawnPoint.rotation);

            connectedPart.transform.SetParent(connectedPartRoot.transform);

            fixedPart = part.AddComponent<FixedPart>();

            foreach (GameObject smallPart in structure.parts.parts)
            {
                if (smallPart.GetComponent<MeshRenderer>())
                {
                    SetPartsProps(smallPart, fixedPart);
                }
                else
                {
                    foreach(Transform piece in smallPart.transform.GetComponentInChildren<Transform>())
                    {
                        if (piece.GetComponent<MeshRenderer>())
                        {
                            SetPartsProps(piece.gameObject, fixedPart);
                        }
                    }
                }
            }
            fixedParts[depth].Add(fixedPart);
            part.AddComponent<BoxCollider>();

            foreach (Transform child in connectedPart.transform)
            {
                foreach(DataBase.ObjStructure name in dataBase.objStructures)
                {
                    if (child.name == name.name)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            part.AddComponent<PhotonView>().ViewID = ID;
            ID++;


            connectedPartRoot.GetComponent<PhotonView>().ViewID = ID;
            ID++;

            fixedPart.connectingPart = connectedPartRoot;
            connectedPartRoot.SetActive(true);
        }
        currentFixedPartIndex = 0;
        NextFixedPart();
    }

    void SetPartsProps(GameObject part, FixedPart addToFixedPart)
    {
        MeshRenderer mesh = part.GetComponent<MeshRenderer>();
        Collider collider = part.AddComponent<MeshCollider>();
        addToFixedPart.PartMeshes.Add(new VisablePart(mesh, mesh.material, collider));

    }

    public void NextFixedPart()
    {
        if (fixedParts.Count > currentFixedPartIndex)
        {
            foreach(FixedPart fixedParts in fixedParts[currentFixedPartIndex])
            {
                fixedParts.currentStage = FixedPart.Stage.Highlighted;
            }
            currentFixedPartIndex++;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
