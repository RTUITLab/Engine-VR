using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] private string Version = "1.0";
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 2;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject MasterPlayer;
    [SerializeField] private GameObject Player;
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
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayers });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player.GetComponent<PlayerOnline>().Disable();
            MasterPlayer.GetComponent<PlayerOnline>().isLocal();
        }
        else
        {
            Player.GetComponent<PlayerOnline>().isLocal();
            MasterPlayer.GetComponent<PlayerOnline>().Disable();
        }
    }
}