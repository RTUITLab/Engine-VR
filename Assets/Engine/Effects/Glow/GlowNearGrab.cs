using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;
public class GlowNearGrab : MonoBehaviour
{
    public Material GlowMaterial;
    Material[] StartMaterials;
    public GameObject Listener;
    bool isGlowing;

    private void OnEnable()
    {
        isGlowing = false;
        StartMaterials = GetComponent<Renderer>().materials;
    }

    private void Start()
    {
        Listener.GetComponent<InteractableHoverEvents>().onHandHoverBegin.AddListener(GlowControll);
        Listener.GetComponent<InteractableHoverEvents>().onHandHoverEnd.AddListener(GlowControll);
    }

    public void GlowControll()
    {
        isGlowing = !isGlowing;
        if (isGlowing)
        {
            Material[] materials = GetComponent<MeshRenderer>().materials;
            for (int i = 0; i< StartMaterials.Length; i++)
            {
                materials[i] = new Material(GlowMaterial);
                materials[i].SetTexture("_Albedo", StartMaterials[i].GetTexture("_BaseMap"));
            }
            //material.SetTexture("_Metallic", StartMaterial.GetTexture("_Metallic"));
            //material.SetTexture("_Normal", StartMaterial.GetTexture("_NormalMap"));
            GetComponent<MeshRenderer>().materials = materials;

        }
        else
        {
            GetComponent<Renderer>().materials = StartMaterials;
        }
    }

}
