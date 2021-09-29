using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presentation : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Material> materials = new List<Material>();
    [System.Serializable]
    [HideInInspector]
    public class Depth
    {
        public int count;
        public Depth(int Count)
        {
            count = Count;
        }

        public static int FindDepth(Transform transform, int d) //Поиск глубины нахождения объекта
        {
            if (transform.parent)
            {
                d++;
                return FindDepth(transform.parent, d);
            }
            else
            {
                return d;
            }
        }
    }

    [System.Serializable]
    [HideInInspector]
    public class Parts
    {
        public List<GameObject> parts;
        public Parts(List<GameObject> Parts)
        {
            parts = Parts;
        }

        public static Parts AdditionalPartCollection(GameObject obj) //Находит дополнительный объект в дочерних у объекта
        {
            List<GameObject> list = new List<GameObject>();
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("дополнительно"))
                {
                    list.Add(child.gameObject);
                }
            }
            Parts parts = new Parts(list);
            return parts;
        }
        public static Parts PartCollection(GameObject obj) // Находит все дочерние детали объекта
        {
            List<GameObject> list = new List<GameObject>();
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("Деталь"))
                    list.Add(child.gameObject);
            }
            Parts parts = new Parts(list);
            return parts;
        }
    }


    [System.Serializable]
    [HideInInspector]
    public class NameContainer
    {
        public string nameContainer;
        public NameContainer(string NameContainer)
        {
            nameContainer = NameContainer;
        }
    }
    [System.Serializable]
    [HideInInspector]
    public class Materials
    {
        public int metallic;
        public int smoothness;
        public Colors color;
        public Materials(int Metallic, int Smoothness, Colors Color)
        {
            metallic = Metallic;
            smoothness = Smoothness;
            color = Color;
        }
    }
    [System.Serializable]
    [HideInInspector]
    public class Colors
    {
        public float r;
        public float g;
        public float b;
        public Colors(float R, float G, float B)
        {
            r = R;
            g = G;
            b = B;
        }
    }
    [System.Serializable]
    public class ObjStructure
    {
        public string name;
        public Depth depth;
        public NameContainer status;
        public Parts parts;
        public Parts additionalParts;
        public GameObject moving; // ВОТ ТУТ СКРИПТ МАКСА ТИПА

        public static void DataCollection(List<ObjStructure> objects, Transform transform)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (!child.name.Contains("Деталь") && !child.name.Contains("дополнительно"))
                {
                    objects.Add(new ObjStructure(child.name, new Depth(Depth.FindDepth(child, 0)), new NameContainer("Visible"), Parts.PartCollection(child.gameObject), Parts.AdditionalPartCollection(child.gameObject)));
                }
            }
        }

        public ObjStructure(string Name, Depth Depth, NameContainer Status, Parts Parts, Parts AdditionalParts, GameObject Moving)
        {
            name = Name;
            depth = Depth;
            status = Status;
            parts = Parts;
            additionalParts = AdditionalParts;
            moving = Moving;
        }


        public ObjStructure(string Name, Depth Depth, NameContainer Status, Parts Parts)
        {
            name = Name;
            depth = Depth;
            status = Status;
            parts = Parts;
        }
        public ObjStructure(string Name, Depth Depth)
        {
            name = Name;
            depth = Depth;

        }

        public ObjStructure(string Name, Depth Depth, NameContainer Status, Parts Parts, Parts AdditionalParts)
        {
            name = Name;
            depth = Depth;
            status = Status;
            parts = Parts;
            additionalParts = AdditionalParts;
        }

        public ObjStructure(string Name)
        {
            name = Name;

        }

        public ObjStructure()
        { }
    }
    public bool check = true;
    public bool Stop = false;
    float f = 0;
    float Timer = 0;
    float Timer2 = 0;
    public List<ObjStructure> objStructures = new List<ObjStructure>();
    public List<Depth> depths = new List<Depth>();
    public List<NameContainer> statuses = new List<NameContainer>();
    public List<Colors> colors = new List<Colors>();
    public List<Parts> parts = new List<Parts>();
    public List<Parts> additionalParts = new List<Parts>();
    public List<Materials> customMaterials = new List<Materials>();
    public List<NameContainer> sources = new List<NameContainer>();

    void Start()
    {
        depths.Add(new Depth(0)); depths.Add(new Depth(1)); depths.Add(new Depth(2)); depths.Add(new Depth(3)); depths.Add(new Depth(4)); depths.Add(new Depth(5)); depths.Add(new Depth(6)); depths.Add(new Depth(7)); depths.Add(new Depth(8));
        statuses.Add(new NameContainer("Visable")); statuses.Add(new NameContainer("Invisible")); statuses.Add(new NameContainer("Highlighted"));
        for (int i = 0; i <= materials.Count - 1; i++)
            colors.Add(new Colors(materials[i].color.r, materials[i].color.g, materials[i].color.b));
        //----------------------------------------------------------------------------------------------------------------
        ObjStructure.DataCollection(objStructures, transform);
        check = false;

    }




    // Update is called once per frame
    void Update()
    {
        //-----------------ВЕСЬ КОД ЧИСТО ДЛЯ ПРЕЗЕНТАЦИИ--------------------------------------
        if (!Stop)
        {
            if (check)
            {
                Timer += Time.deltaTime;
                f = 0;
                foreach (ObjStructure obj in objStructures)
                {
                    foreach (GameObject obj2 in obj.parts.parts)
                    {
                        f -= 0.05f * Time.deltaTime;
                        obj2.transform.position += new Vector3(f * 0.0035f * Timer, 0, 0);
                    }
                }
            }
            else
            {
                if (Timer >= 0)
                {
                    Timer -= Time.deltaTime;
                    f = 0;
                    foreach (ObjStructure obj in objStructures)
                    {
                        foreach (GameObject obj2 in obj.parts.parts)
                        {
                            f += 0.05f * Time.deltaTime;
                            obj2.transform.position += new Vector3(f * 0.0035f * Timer, 0, 0);
                        }
                    }
                }

            }
        }
        else
        {
            gameObject.GetComponent<Animator>().StopPlayback();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (check)
                check = false;
            else
                check = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Stop)
                Stop = false;
            else
                Stop = true;
        }
    }
}
