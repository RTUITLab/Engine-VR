using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [HideInInspector] public FixedPart connecntedFixed;

    [SerializeField] Material outlineMaterial;

    [SerializeField] GameObject description;
    [SerializeField] Transform camera;

    public enum State { HighLighted, Visible }

    State _state;

    public State state
    {
        get
        {
            return _state;
        }
        set
        {
            if(value == State.HighLighted)
            {
                SetOutline(true);
            }
            else if (value == State.Visible)
            {
                SetOutline(false);
            }
        }
    }

    void SetOutline(bool Adding)
    {
        List<Material> materials = new List<Material>();
        GetComponent<MeshRenderer>().GetMaterials(materials);
        if (Adding)
        {
            materials.Add(outlineMaterial);
            description.SetActive(true);
        }
        else
        {
            if (materials.Contains(outlineMaterial))
            {
                materials.Remove(outlineMaterial);
            }
            description.SetActive(false);
        }
        GetComponent<MeshRenderer>().materials = materials.ToArray();
    }


    // Start is called before the first frame update
    void Start()
    {
        state = State.Visible;
        Invoke("AfterStartFunc", 1f);
    }

    void AfterStartFunc()
    {
        camera = GameObject.FindGameObjectWithTag("VrCamera").transform;

    }

    public void IsReadyChanger(bool IsReady)
    {
        connecntedFixed.IsReadyToMove = IsReady;
        if (IsReady)
        {
            state = State.HighLighted;
        }
        else
        {
            state = State.Visible;
        }
    }

    // Update is called once per frame
    void Update()
    {
        description.transform.LookAt(camera);
    }
}
