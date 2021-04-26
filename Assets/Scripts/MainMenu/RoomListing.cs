using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private Text text;
    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roominfo)
    {
        RoomInfo = roominfo;
        text.text = roominfo.Name;
    }
}
