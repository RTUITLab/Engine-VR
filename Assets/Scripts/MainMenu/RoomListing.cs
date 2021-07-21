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

    [SerializeField] private GameObject roomOpened;
    [SerializeField] private GameObject roomClosed;

    private void Start()
    {
        networking = FindObjectOfType<Networking>();
    }

    public void SetRoomInfo(RoomInfo roominfo)
    {
        RoomInfo = roominfo;
        text.text = roominfo.Name;

        roomOpened.SetActive(roominfo.IsOpen);
        roomClosed.SetActive(!roominfo.IsOpen);
    }
    
    public void JoinChosenRoom()
    {
        if (RoomInfo.IsOpen)
            networking.JoinRoom(RoomInfo.Name);
    }
}
