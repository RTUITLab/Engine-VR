using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SyncUser : MonoBehaviour //Этот вариант лучше, его можно будет потом не так сложно переделать под множество игроков
{
    [SerializeField] private Transform Capsule;
    [SerializeField] private Transform Player;
    private bool isMaster;
    private PhotonView photonView;
    void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        photonView = gameObject.GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(PhotonNetwork.InRoom)
        {
            photonView.RPC("SyncCapsulePos", RpcTarget.Others, Player.position.x, Player.position.y, Player.position.z);
            photonView.RPC("SyncCapsuleRotation", RpcTarget.Others, Player.rotation.x, Player.rotation.y, Player.rotation.z, Player.rotation.w);
        }
    }

    [PunRPC]
    private void SyncCapsulePos(float x, float y, float z)
    {
        Capsule.position = new Vector3(x, y, z);
    }

    [PunRPC]
    private void SyncCapsuleRotation(float x, float y, float z, float w)
    {
        Capsule.rotation = new Quaternion(x, y, z, w);
    }
}
