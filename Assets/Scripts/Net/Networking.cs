using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] private string Version = "1.1";
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 2;
    [SerializeField] private int PhotonLimit = 20; //Лимит максимального кол-ва подключений.
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
        byte FreeSlots = (byte)(PhotonLimit - PhotonNetwork.CountOfPlayers); //Колчичество свободных мест в данном приложении.
        if (FreeSlots > 0)
        {
            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = (FreeSlots > maxPlayers ? maxPlayers : FreeSlots), PublishUserId = true });
        }
        else
        {
            Debug.LogError("Свободных мест нет. Сервер приложения переполнен.");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"Количесто игроков в комнате: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    private void Update()
    {
        if(PhotonNetwork.InRoom)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}