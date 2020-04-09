using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerSync : MonoBehaviour
{
    //[SerializeField] private GameObject LeftHend;
    //[SerializeField] private GameObject RightHend;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject[] usless; //не нужные другому пользователю
    [SerializeField] private PhotonView photonView;
    //[SerializeField] private GameObject[] hands;
    private void Start()
    {
        for(int i = 0; i < usless.Length; ++i)
        {
            usless[i].SetActive(photonView.IsMine);
        }
        body.SetActive(!photonView.IsMine);

        //if (!photonView.IsMine)
        //{
        //    PhotonNetwork.Instantiate(hands[0].name, Vector3.zero, Quaternion.identity);
        //    PhotonNetwork.Instantiate(hands[1].name, Vector3.zero, Quaternion.identity);
        //}
    }
}
