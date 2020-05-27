using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ConstractingManager : MonoBehaviour
{
    [SerializeField] List<List<FixedPart>> fixedParts = new List<List<FixedPart>>();
    [SerializeField] DataBase dataBase;
    [SerializeField] MovingPartDataBase movingPartDataBase;
    [SerializeField] GameObject MovingPartExample;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] Material HighlightMaterial;
    [SerializeField] Material DisolveMaterial;
    
    int ID = 20;

    public bool Education;

    int currentFixedPartIndex;
    // Start is called before the first frame update
    void Start()
    {
        SettingUpEngine();
        Invoke("StartFunc", 1);
    }

    void SettingUpEngine()
    {
        foreach (DataBase.ObjStructure structure in dataBase.objStructures)
        {
            GameObject part = GameObject.Find(structure.name);
            int depth = structure.depth.count;
            FixedPart fixedPart = part.AddComponent<FixedPart>();

            while (depth > fixedParts.Count - 1)
            {
                fixedParts.Add(new List<FixedPart>());
            }

            foreach (GameObject smallPart in structure.parts.parts)
            {
                if (smallPart.GetComponent<MeshRenderer>())
                {
                    SetPartsProps(smallPart, fixedPart.PartMeshes, false);
                }
            }

            int PartIndex = fixedParts[depth].Count;
            Transform spawnPos = spawnPoints[PartIndex];
            GameObject connectedPart = Instantiate(part, spawnPoints[PartIndex].position, spawnPoints[PartIndex].rotation);
            connectedPart.name = part.name;
            GameObject connectedPartRoot = Instantiate(MovingPartExample, spawnPoints[PartIndex].position, spawnPoints[PartIndex].rotation);
            connectedPart.transform.SetParent(connectedPartRoot.transform);
            Destroy(connectedPart.GetComponent<FixedPart>());
            if (structure.additionalRoot)
            {
                Destroy(connectedPart.transform.Find(structure.additionalRoot.name).gameObject);
            }


            foreach (GameObject addPart in structure.additionalParts.parts)
            {
                if (addPart.GetComponent<MeshRenderer>())
                {
                    SetPartsProps(addPart, fixedPart.AddPartMeshes, true);
                }
            }

            foreach (Transform child in connectedPart.transform)
            {
                foreach (DataBase.ObjStructure name in dataBase.objStructures)
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
            fixedPart.Highlited = Instantiate(HighlightMaterial);
            fixedPart.Highlited.SetColor("_TintColor", Random.ColorHSV(0,1,0.2f,0.5f,0.2f,0.5f));
            part.AddComponent<BoxCollider>();
            fixedParts[depth].Add(fixedPart);

            movingPartDataBase.FillArrays(spawnPos, fixedPart.transform, connectedPartRoot);

            if (!Education)
            {
                connectedPartRoot.SetActive(true);
            }
        }
    }

    void StartFunc()
    { 
        currentFixedPartIndex = 0;
        NextFixedPart();
    }

    void SetPartsProps(GameObject part, List<VisablePart> PartsList, bool AddDisolving)
    {
        MeshRenderer mesh = part.GetComponent<MeshRenderer>();
        MeshCollider collider = part.AddComponent<MeshCollider>();
        part.tag = "smallPart";
        collider.convex = true;
        if (AddDisolving)
        {
            part.AddComponent<DisolveScript>().DisolveMaterial = DisolveMaterial;
            part.GetComponent<DisolveScript>().DisolveProcess(true);

        }
        PartsList.Add(new VisablePart(mesh, mesh.material, collider));
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
