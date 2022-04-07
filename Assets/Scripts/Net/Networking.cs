using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 10;

    [SerializeField] private int PhotonLimit = 20; //Лимит максимального кол-ва подключений.

    //private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private MainMenuController mainMenu;
    private RoomListingsMenu roomListingMenu;

    public static bool ConstructionStarted = false;
    private bool roomClosed = false;

    public Text text;

    void Start()
    {
        PhotonNetwork.GameVersion = "1.3";
        PhotonNetwork.ConnectUsingSettings();
        roomListingMenu = FindObjectOfType<RoomListingsMenu>();

        mainMenu = FindObjectOfType<MainMenuController>();
    }

    private void Update() {
        if (ConstructionStarted && !roomClosed)
        {
            Debug.Log("Room closed!");

            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.IsOpen = false;

            roomClosed = true;
        }
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

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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

        roomListingMenu.UpdateRoomListing();
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

    /*private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }*/
}
