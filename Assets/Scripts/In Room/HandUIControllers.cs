using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIControllers : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        RoomUI roomUI = other.gameObject.GetComponentInParent<RoomUI>();
        if (roomUI)
        {
            switch (other.gameObject.name)
            {
                case "LeaveRoom Button":
                    roomUI.ClickLeaveRoom();
                    break;
            }
        }
    }
}
