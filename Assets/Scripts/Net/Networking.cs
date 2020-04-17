using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] private string Version = "1.1";
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 2;
    void Start()
    {
        PhotonNetwork.GameVersion = Version;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayers, PublishUserId = true });
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    private void Update()
    {

        try
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
        catch
        {

        }
    }
}