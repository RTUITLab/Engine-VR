using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class SyncUser : MonoBehaviourPunCallbacks //Этот вариант лучше, его можно будет потом не так сложно переделать под множество игроков
{
    [SerializeField] private GameObject CapsulePrefab;
    [SerializeField] private Transform Player;
    private List<Transform> Capsules = new List<Transform>();
    private bool isMaster;
    private PhotonView photonView;
    void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        photonView = gameObject.GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC("SyncCapsulePos", RpcTarget.Others, Player.position.x, Player.position.y, Player.position.z, PhotonNetwork.LocalPlayer.ActorNumber);
            photonView.RPC("SyncCapsuleRotation", RpcTarget.Others, Player.rotation.x, Player.rotation.y, Player.rotation.z, Player.rotation.w, PhotonNetwork.LocalPlayer.ActorNumber);
            //Debug.Log($"id человека {PhotonNetwork.LocalPlayer.ActorNumber}"); //При переподключении старые юзеры не стираються

            Debug.LogError($"Кол во трансформов в листе {Capsules.Count}");
        }
    }

    [PunRPC]
    private void SyncCapsulePos(float x, float y, float z, int id)
    {
        id = (PhotonNetwork.LocalPlayer.ActorNumber < id) ? id - 2 : id - 1;
        Capsules[id - 1].position = new Vector3(x, y, z);
    }

    [PunRPC]
    private void SyncCapsuleRotation(float x, float y, float z, float w, int id)
    {
        id = (PhotonNetwork.LocalPlayer.ActorNumber < id) ? id - 2 : id - 1;
        Capsules[id].rotation = new Quaternion(x, y, z, w);
    }

    public override void OnJoinedRoom() //Нужно добавить одну капсулу всем клиентам, кроме себя
    {
        photonView.RPC("CreateCapsule", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    private void CreateCapsule(int id)
    {
            Debug.LogError("Добавляем капсулу");
            GameObject capsule = Instantiate(CapsulePrefab);
            Capsules.Add(capsule.transform);
    }
}
