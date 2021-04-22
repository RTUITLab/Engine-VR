using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField] private string Version = "1.1";
    [SerializeField] [Range(2, 20)] private byte maxPlayers = 2;
    [SerializeField] private int PhotonLimit = 20; //Лимит максимального кол-ва подключений.
    Dictionary<string, RoomInfo> myRoomList = new Dictionary<string, RoomInfo>();

    void Start()
    {
        PhotonNetwork.GameVersion = Version;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // PhotonNetwork.JoinRandomRoom();
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

        var transforms = FindObjectsOfType<SyncTranshorm>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("roomListUpdated");

        //pr is used to temporarily save the room button on the ui, and destroy the originally displayed room ui after obtaining it
        GameObject[] pr = GameObject.FindGameObjectsWithTag("CustomRoom");
        foreach (var r in pr)
        {
            Destroy(r);
        }

        //Traverse the rooms where the room information has changed. Note that this roomList does not include all rooms, but rooms with changed attributes, such as newly added ones.
        foreach (var r in roomList)
        {
            //Clear closed or undisplayable or removed rooms
            if (!r.IsOpen || !r.IsVisible || r.RemovedFromList)
            {
                if (myRoomList.ContainsKey(r.Name))//If the room existed before, remove it now
                {
                    myRoomList.Remove(r.Name);
                }
                continue;
            }
            //Update room information
            if (myRoomList.ContainsKey(r.Name))
            {
                myRoomList[r.Name] = r;
            }
            //Add new room
            else
            {
                myRoomList.Add(r.Name, r);//If the room did not exist before, add it to myRoomList
            }
        }

       /* foreach (var r in myRoomList.Values)
        {
            //This function is used to add room ui on canvas, define it yourself
            UpdateRoomList(r.Name);
        }

        Debug.Log("===roomList count:" + roomList.Count + "===myRoomList count:" + myRoomList.Count);
        messageText.text = "===roomList count:" + roomList.Count + "===myRoomList count:" + myRoomList.Count;*/
    }

   /* public void UpdateRoomList(string name)
    {
        //Here I have made the room ui into a prefab and saved it in the Resources folder
        GameObject go = Instantiate(Resources.Load("CustomRoom") as GameObject);
        go.transform.parent = roomInLobbyPanel.transform;//roomInLobbyPanel is the panel I use to display the room list
        go.transform.localScale = Vector3.one;
        go.name = name;
        go.transform.Find("RoomName").GetComponent<Text>().text = name;//Set the text as the room name
                                                                       //Binding select room event (custom)
        go.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            this.ChooseRoom(go.name);
        });
    }*/






    private void Update()
    {
        if(PhotonNetwork.InRoom)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}