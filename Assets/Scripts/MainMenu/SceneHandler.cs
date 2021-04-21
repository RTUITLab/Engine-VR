using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {

        VRButton button = e.target.GetComponent<VRButton>();

        if (button != null)
        {
            if (button.EventClick != null)
            {
                button.EventClick.Invoke();
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        VRButton button = e.target.GetComponent<VRButton>();

        if (button != null)
        {
            if (button.EventInside != null)
            {
                button.EventInside.Invoke();
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        VRButton button = e.target.GetComponent<VRButton>();

        if (button != null)
        {
            if (button.EventOutside != null)
            {
                button.EventOutside.Invoke();
            }
        }
    }
}