using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] private string Version = "1.1";
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 2;

    [SerializeField] private int PhotonLimit = 20; //Лимит максимального кол-ва подключений.

    //private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private MainMenuController mainMenu;

    public Text text;

    void Start()
    {
        PhotonNetwork.GameVersion = Version;
        PhotonNetwork.ConnectUsingSettings();

        mainMenu = FindObjectOfType<MainMenuController>();
    }

    public override void OnConnectedToMaster()
    {
        JoinLobby();

    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void CreateRoom()
    {
        
        string roomName = "Комната " + PlayerPrefs.GetString("Nickname");

        byte FreeSlots =
            (byte)(PhotonLimit - PhotonNetwork.CountOfPlayers); //Колчичество свободных мест в данном приложении.
        if (FreeSlots > 0)
        {
            PhotonNetwork.CreateRoom(roomName,
                new Photon.Realtime.RoomOptions
                { MaxPlayers = (FreeSlots > maxPlayers ? maxPlayers : FreeSlots), PublishUserId = true });

        }
        else
        {
            Debug.LogError("Свободных мест нет. Сервер приложения переполнен.");
        }
    }


    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}