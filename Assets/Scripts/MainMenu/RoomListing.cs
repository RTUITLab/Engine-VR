using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private Text text;
    private Networking networking;
    public RoomInfo RoomInfo { get; private set; }

    private void Start()
    {
        networking = FindObjectOfType<Networking>();
    }

    public void SetRoomInfo(RoomInfo roominfo)
    {
        RoomInfo = roominfo;
        text.text = roominfo.Name;
    }
    public void JoinChosenRoom()
    {
        networking.JoinRoom(text.text);
    }
}
