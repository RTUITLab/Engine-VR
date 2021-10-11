using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBForOptimizedEngine : MonoBehaviour
{
    public int depthCount;

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
                    AllParts(child, list);
                }
            }
            Parts parts = new Parts(list);
            return parts;
        }

        public static GameObject AdditionalPartRoot(GameObject obj) //Находит дополнительный объект в дочерних у объекта
        {
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("дополнительно"))
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        public static void AllParts(Transform obj, List<GameObject> objects) //Метод находит все детали объекта obj и запихивает их в массив objects
        {
            foreach (Transform child in obj.transform)
            {
                if (child.name.Contains("Деталь"))
                    objects.Add(child.gameObject);
                if (child.childCount != 0 && child.name.Contains("Деталь"))
                    AllParts(child, objects);
            }
        }
        public static Parts PartCollection(GameObject obj) // Находит все дочерние детали объекта
        {
            List<GameObject> list = new List<GameObject>();
            AllParts(obj.transform, list); //тут заполняем массив list всеми "Деталями" объекта obj
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
        NameContainer status;
        Parts parts;
        Parts additionalParts;
        public GameObject mainRoot;
        public GameObject additionalRoot;
        public GameObject moving; // ВОТ ТУТ СКРИПТ МАКСА ТИПА

        public static void DataCollection(List<ObjStructure> objects, Transform transform)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (!child.name.Contains("дополнительно"))
                {
                    objects.Add(new ObjStructure(child.gameObject, child.name, new Depth(Depth.FindDepth(child, 0)), new NameContainer("Visible"), Parts.PartCollection(child.gameObject), Parts.AdditionalPartCollection(child.gameObject), Parts.AdditionalPartRoot(child.gameObject)));
                }
            }
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

        public ObjStructure(GameObject MainRoot, string Name, Depth Depth, NameContainer Status, Parts Parts, Parts AdditionalParts, GameObject AdditionalRoot)
        {
            mainRoot = MainRoot;
            name = Name;
            depth = Depth;
            status = Status;
            parts = Parts;
            additionalParts = AdditionalParts;
            additionalRoot = AdditionalRoot;
        }

        public ObjStructure(string Name)
        {
            name = Name;

        }

        public ObjStructure()
        { }
    }
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
        for (int i = 0; i < depthCount; i++)
        {
            depths.Add(new Depth(i));
        }
        statuses.Add(new NameContainer("Visable")); 
        statuses.Add(new NameContainer("Invisible")); 
        statuses.Add(new NameContainer("Highlighted"));

        for (int i = 0; i <= materials.Count - 1; i++)
            colors.Add(new Colors(materials[i].color.r, materials[i].color.g, materials[i].color.b));

        ObjStructure.DataCollection(objStructures, transform);

    }
}
