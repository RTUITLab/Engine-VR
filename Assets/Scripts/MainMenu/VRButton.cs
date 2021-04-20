using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]

public class VRButton : MonoBehaviour
{
    public UnityEvent EventClick;
    public UnityEvent EventInside;
    public UnityEvent EventOutside;
}
