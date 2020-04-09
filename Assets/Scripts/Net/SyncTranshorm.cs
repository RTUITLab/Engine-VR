using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class SyncTranshorm : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    [SerializeField] private bool position = true;
    private Vector3 lastPosition = Vector3.zero;
    [SerializeField] private bool rotation = true;
    private Quaternion lastRotation = Quaternion.identity;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private bool lastKinematic;

    private void Start()
    {
        _transform = gameObject.transform;
        lastPosition = _transform.position;
        lastRotation = _transform.rotation;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        lastKinematic = _rigidbody.isKinematic;
    }

    private void Update()
    {
        if(position && lastPosition != _transform.position && _rigidbody.isKinematic)
        {
            photonView.RPC("SyncPosition", RpcTarget.All, _transform.position.x, _transform.position.y, _transform.position.z);
        }
        if(rotation && lastRotation != _transform.rotation && _rigidbody.isKinematic)
        {
            photonView.RPC("SyncRotation", RpcTarget.All, _transform.rotation.x, _transform.rotation.y, _transform.rotation.z, _transform.rotation.w);
        }
        if(lastKinematic != _rigidbody.isKinematic)
        {
            photonView.RPC("SyncKinrmatic", RpcTarget.All, _rigidbody.isKinematic);
        }
    }

    [PunRPC]
    private void SyncPosition(float x, float y, float z)
    {
        Vector3 newVector = new Vector3(x, y, z);
        lastPosition = newVector;
        _transform.position = newVector;
    }

    [PunRPC]
    private void SyncRotation(float x, float y, float z, float w)
    {
        Quaternion newQuaternion = new Quaternion(x, y, z, w);
        lastRotation = newQuaternion;
        _transform.rotation = newQuaternion;
    }

    [PunRPC]
    private void SyncKinrmatic(bool kin)
    {
        _rigidbody.isKinematic = lastKinematic = kin;
    }
}
