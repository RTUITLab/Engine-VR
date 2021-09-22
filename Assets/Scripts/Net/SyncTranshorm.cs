using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(PhotonView))]

public class SyncTranshorm : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Interactable interaclable;
    [SerializeField] private Throwable throwable;

    [SerializeField] private Collider colider;
    private Rigidbody _rigidbody;
    private Transform _transform;

    [SerializeField] private bool position = true;
    private Vector3 lastPosition = Vector3.zero;
    [SerializeField] private bool rotation = true;
    private Quaternion lastRotation = Quaternion.identity;

    private bool cathed = false; //Локальная переменная, если обьект схвачен, то true.
    private bool syncCathed = false; //Если кто то другой хватает, то у всех это истино.
    private bool lastOwner = false; //КТо будет просчитывать физику данного предмета (Последний держатель предмета).

    private void Start()
    {
        _transform = gameObject.transform;
        lastPosition = _transform.position;
        lastRotation = _transform.rotation;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom) {
            if (!syncCathed && (cathed || lastOwner)) //Если он не взят другим игроком и ты мастер клиент или его держатель.
            {
                if (position && lastPosition != _transform.position)
                {
                    photonView.RPC("SyncPosition", RpcTarget.All, _transform.position.x, _transform.position.y, _transform.position.z);
                }
                if (rotation && lastRotation != _transform.rotation)
                {
                    photonView.RPC("SyncRotation", RpcTarget.All, _transform.rotation.x, _transform.rotation.y, _transform.rotation.z, _transform.rotation.w);
                }
            }
            else if (syncCathed && cathed) //Нужно проверить, но это не должно быть достигнуто.
            {
                Debug.LogError("Обьект схвачен двумя игроками. Не достижимый вариант событий.");
            }
            else //Если ничего не происходит, то последняя позиция - наша позиция.
            {
                _transform.position = lastPosition;
                _transform.rotation = lastRotation;
            }
        }
    }

    [PunRPC] private void SyncPosition(float x, float y, float z) //Можно было не делить, но теоретически может передавать меньшее кол-во информации при раздельной передачи позиции и поворота.
    {
        Vector3 newVector = new Vector3(x, y, z);
        lastPosition = newVector;
        _transform.position = newVector;
    }

    [PunRPC] private void SyncRotation(float x, float y, float z, float w)
    {
        Quaternion newQuaternion = new Quaternion(x, y, z, w);
        lastRotation = newQuaternion;
        _transform.rotation = newQuaternion;
    }

    public void CatchObj(bool active) //Вызывается когда игрок схватил или отпустил предмет.
    {
        if (transform.GetChild(0).name == "Двигатель(ось)")
        {
            // Комната закрывается при взятии в руку первой детали.
            Networking.ConstructionStarted = true;
        }

        Part part = GetComponent<Part>();
        if (part)
        {
            if (active)
            {
                part.DisplayHint();
            } else
            {
                part.StopDisplaying();
            }
        }

        cathed = active;
        photonView.RPC("Interactable", RpcTarget.OthersBuffered, active);
        if (active)
        {
            lastOwner = true;
        }
    }

    [PunRPC] private void Interactable(bool active) //Изменение активности VR скриптов.
    {
        interaclable.enabled = throwable.enabled  = colider.enabled = !active; //Выключаем или включаем у всех кто не держал предмет.
        syncCathed = active;
        if (active) 
        {
            cathed = lastOwner = false;
        }
    }

    [PunRPC] private void SendVelocity(float x, float y, float z)
    {
        _rigidbody.velocity = new Vector3(x, y, z);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) //При вроде передача информации о последнем положении предмета. Нужно что бы человек мог с ним взаимодействовать.
    {
        photonView.RPC("SyncPosition", RpcTarget.All, _transform.position.x, _transform.position.y, _transform.position.z);
        photonView.RPC("SyncRotation", RpcTarget.All, _transform.rotation.x, _transform.rotation.y, _transform.rotation.z, _transform.rotation.w);

        FindObjectOfType<LocalPlayer>().SendNickname();

        Debug.Log(newPlayer.UserId);
    }

    public override void OnJoinedRoom() //Запускаеться только при локальномКоннекте, что бы можно было передвигать предметы.
    {
        lastOwner = PhotonNetwork.IsMasterClient;
    }

    public bool isLastOwner()
    {
        return lastOwner;
    }

    public void SendGrav(bool grav) //Изменение гравитайии в Rigidbody
    {
        photonView.RPC("sendGrav", RpcTarget.All, grav);
    }

    [PunRPC] private void sendGrav(bool grav)
    {
        _rigidbody.useGravity = grav;
    }
}
