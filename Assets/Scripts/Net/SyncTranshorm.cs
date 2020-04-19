using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(PhotonView))]

public class SyncTranshorm : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Interactable interaclable;
    [SerializeField] private Throwable throwable;
    private Rigidbody _rigidbody;
    private Transform _transform;

    [SerializeField] private bool position = true;
    private Vector3 lastPosition = Vector3.zero;
    [SerializeField] private bool rotation = true;
    private Quaternion lastRotation = Quaternion.identity;

    private bool cathed = false; //локальная переменная, если обьект схвачен, то true
    private bool syncCathed = false;

    private void Start()
    {
        _transform = gameObject.transform;
        lastPosition = _transform.position;
        lastRotation = _transform.rotation;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!syncCathed && (cathed || PhotonNetwork.IsMasterClient)) //Если он не взят другим игроком и ты мастер клиент или его держатель.
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

    public void CatchObj(bool active) //Вызываеться когда игрок схватил или отпустил предмет.
    {
        cathed = active;
        photonView.RPC("Interactable", RpcTarget.Others, active);
        if (!active) //При отпускании предмета передавать ускорение мастеру для просчета траектории.
        {
            photonView.RPC("SendVelocity", RpcTarget.MasterClient, _rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z);
        }
    }

    [PunRPC] private void Interactable(bool active) //Изменение активности VR скриптов.
    {
        interaclable.enabled = throwable.enabled = !active;
        syncCathed = active;
        if (syncCathed) 
        {
            cathed = false;
        }
    }

    [PunRPC] private void SendVelocity(float x, float y, float z)
    {
        _rigidbody.velocity = new Vector3(x, y, z);
    }
}
