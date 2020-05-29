using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveScript : MonoBehaviour
{

    public Material DisolveMaterial;
    Material StartMaterial;
    public bool Disolving;
    bool StartedDisolving;

    private void OnEnable()
    {
        StartMaterial = GetComponent<Renderer>().material;
        StartedDisolving = false;
    }

    public void DisolveProcess(bool OnDisovle)
    {
        
        Disolving = OnDisovle;
        Material material = new Material(DisolveMaterial);
        material.SetTexture("_Albedo", StartMaterial.GetTexture("_BaseMap"));
        //material.SetTexture("_Normal", StartMaterial.GetTexture("_NormalMap"));
        GetComponent<MeshRenderer>().material = material;
        if (OnDisovle)
        {
            GetComponent<MeshRenderer>().material.SetFloat("_DisolveControll", 0);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetFloat("_DisolveControll", 1);
        }
        StartedDisolving = true;
    }

    void Update()
    {

        if (StartedDisolving)
        {
            if (Disolving)
            {
                float i = GetComponent<MeshRenderer>().material.GetFloat("_DisolveControll");
                if (i < 1)
                {
                    i += Time.deltaTime * 0.5f;
                    GetComponent<MeshRenderer>().material.SetFloat("_DisolveControll", i);
                }
                if (i >= 1)
                {
                    StartedDisolving = false;
                }
            }
            else
            {
                float i = GetComponent<MeshRenderer>().material.GetFloat("_DisolveControll");
                if (i > 0)
                {
                    i -= Time.deltaTime * 0.5f;
                    GetComponent<MeshRenderer>().material.SetFloat("_DisolveControll", i);
                }
                if (i <= 0)
                {
                    if (GetComponent<Renderer>().material != StartMaterial)
                    {
                        GetComponent<Renderer>().material = StartMaterial;
                        StartedDisolving = false;
                    }
                }
            }

        }
    }        
}
