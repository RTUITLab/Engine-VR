using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        HandUIControllers hand = other.gameObject.GetComponentInParent<HandUIControllers>();
        if (hand)
        {
            Application.Quit();
        }
    }
}
