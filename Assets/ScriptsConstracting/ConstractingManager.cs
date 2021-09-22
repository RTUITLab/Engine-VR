using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ConstractingManager : MonoBehaviour
{
    [SerializeField] List<List<FixedPart>> fixedParts = new List<List<FixedPart>>();
    [SerializeField] DBForOptimizedEngine dataBase;
    [SerializeField] MovingPartDataBase movingPartDataBase;
    [SerializeField] GameObject MovingPartExample;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] Material HighlightMaterial;
    [SerializeField] Material GlowMaterial;
    [SerializeField] Material HintColor;
    [SerializeField] Material DisolveMaterial;
    [SerializeField] Transform Hints;

    private BuildProgress progress;
    [SerializeField] private Animator[] presentationAnimations; // Начинают проигрываться при завершении сборки

    int ID = 20;

    public bool Education;

    int currentFixedPartDepth;
    int LeftFixedParts;

    // Start is called before the first frame update
    void Start()
    {
        SettingUpEngine();
        Invoke("StartFunc", 1);
        progress = FindObjectOfType<BuildProgress>();

        //foreach (Transform transform in GameObject.FindGameObjectWithTag("Hints").transform)
        //{
        //   Hints.Add(transform);
        //}

    }

    void SettingUpEngine()
    {
        foreach (DBForOptimizedEngine.ObjStructure structure in dataBase.objStructures)
        {
            partsCount++;
            GameObject part = GameObject.Find(structure.name);
            int depth = structure.depth.count;
            FixedPart fixedPart = part.AddComponent<FixedPart>();

            while (depth > fixedParts.Count - 1)
            {
                fixedParts.Add(new List<FixedPart>());
            }

           ;

            SetPartsProps(structure.mainRoot, ref fixedPart.PartMeshe, false);


            /*foreach (GameObject smallPart in structure.parts.parts)
            {
                if (smallPart.GetComponent<MeshRenderer>())
                {
                    SetPartsProps(smallPart, fixedPart.PartMeshes, false);
                }
            }*/

            int PartIndex = fixedParts[depth].Count;
            Transform spawnPos = spawnPoints[PartIndex];



            GameObject connectedPartRoot = Instantiate(MovingPartExample, spawnPoints[PartIndex].position, spawnPoints[PartIndex].rotation);
            Transform Hint = Hints.Find(structure.name);
            connectedPartRoot.GetComponent<Part>().Hint = Hint;

            GameObject connectedPart = Instantiate(part, spawnPoints[PartIndex].position, spawnPoints[PartIndex].rotation);
            connectedPart.name = part.name;
            GlowNearGrab glow = connectedPart.AddComponent<GlowNearGrab>();
            glow.GlowMaterial = GlowMaterial;
            glow.Listener = connectedPartRoot;
            connectedPart.transform.SetParent(connectedPartRoot.transform);
            Hint.position = connectedPart.transform.position;
            Hint.position += new Vector3(0, 0.7f, 0);

            Destroy(connectedPart.GetComponent<FixedPart>());
            if (structure.additionalRoot)
            {
                Destroy(connectedPart.transform.Find(structure.additionalRoot.name).gameObject);

                SetPartsProps(structure.additionalRoot, ref fixedPart.AddPartMeshe, true);
            }

            if (connectedPart.GetComponent<DBForOptimizedEngine>())
                Destroy(connectedPart.GetComponent<DBForOptimizedEngine>());

            foreach (Transform child in connectedPart.transform)
            {
                foreach (DBForOptimizedEngine.ObjStructure name in dataBase.objStructures)
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

            fixedPart.SetConnected(connectedPartRoot);
            fixedPart.Highlited = Instantiate(HighlightMaterial);

            Color PartColor = Random.ColorHSV(0, 1, 0.176f, 0.196f, 0.196f, 0.216f);
            if (Education)
            {
                fixedPart.Highlited.SetColor("_TintColor", PartColor);
            }

            Material HintMaterail = new Material(HintColor);
            HintMaterail.color = PartColor;
            Hint.transform.GetChild(1).GetComponent<Image>().material = HintMaterail;
            Hint.gameObject.SetActive(false);

            part.AddComponent<BoxCollider>();

            // Если деталь слишком маленькая, то увеличиваем ее коллайдер
            BoxCollider installRange = part.GetComponent<BoxCollider>();
            float minSize = 0.3f;
            Vector3 newSize = installRange.size;
            if (newSize.x < minSize)
                newSize.x *= 1.5f;
            if (newSize.y < minSize)
                newSize.y *= 1.5f;
            if (newSize.z < minSize)
                newSize.z *= 1.5f;
            installRange.size = newSize;

            fixedParts[depth].Add(fixedPart);
            movingPartDataBase.FillArrays(spawnPos, fixedPart.transform, connectedPartRoot, Hint.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

            if (!Education)
            {
                //connectedPartRoot.SetActive(true);
            }

            part.GetComponent<MeshCollider>().enabled = false;
        }
    }

    void StartFunc()
    {
        currentFixedPartDepth = 0;
        NextFixedPart();
    }

    void SetPartsProps(GameObject part, ref VisablePart visablePart, bool AddDisolving)
    {
        MeshRenderer mesh = part.GetComponent<MeshRenderer>();

        MeshCollider collider = part.GetComponent<MeshCollider>();

        //List<Material> materials = new List<Material>();
        //mesh.GetMaterials(materials);
        part.tag = "smallPart";
        if (AddDisolving)
        {
            DisolveScript disolve = part.AddComponent<DisolveScript>();
            disolve.DisolveMaterial = DisolveMaterial;
            disolve.DisolveProcess(true);
            visablePart = new VisablePart(mesh, mesh.materials, collider, disolve);
        }
        else
        {
            visablePart = new VisablePart(mesh, mesh.materials, collider);
        }
        //PartsList.Add(new VisablePart(mesh, mesh.material, collider));
    }

    public void NextFixedPart()
    {
        currentPlacedPartsCount++;
        placedPartsCount++;

        if (fixedParts.Count > currentFixedPartDepth)
        {
            LeftFixedParts--;
            if (LeftFixedParts <= 0)
            {
                NextStage();
            }
        }

        if (partsCount <= placedPartsCount)
        {
            BuildFinished();
        }

        DisplayProgress();
    }

    private void NextStage()
    {
        foreach (FixedPart fixedParts in fixedParts[currentFixedPartDepth])
        {
            fixedParts.currentStage = FixedPart.Stage.Highlighted;
        }
        LeftFixedParts = fixedParts[currentFixedPartDepth].Count;
        currentPartsCount = LeftFixedParts;
        currentPlacedPartsCount = 0;
        currentFixedPartDepth++;
    }

    private void BuildFinished()
    {
        foreach (var anim in presentationAnimations)
        {
            anim.enabled = true;
        }
    }

    [Header("Прогресс")]
    private int partsCount = 0;
    private int placedPartsCount = -1;
    private int currentPartsCount = 0;
    private int currentPlacedPartsCount = -1;

    private void DisplayProgress()
    {
        int currentStage = currentFixedPartDepth;

        progress.ChangeData(partsCount, placedPartsCount, currentStage, currentPartsCount, currentPlacedPartsCount);
    }
}
