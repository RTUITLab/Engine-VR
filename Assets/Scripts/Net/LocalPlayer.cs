using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class LocalPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject onlineBodyPref;     //Префаб онлайн тела.
    [SerializeField] private Transform[] bodyPoints;    //Точки тела, которые нужно синхронизировать с оналйн телом.
    private Transform transform;
    private OnlinePlayer onlinePlayer = null;

    private List<GameObject> nicknames;

    private void Awake()
    {
        transform = gameObject.transform;
    }

    public override void OnJoinedRoom() //При входе, создаёт своё тело и запоминает его, что бы менять ему положение.
    {
        GameObject onlineBody = PhotonNetwork.Instantiate(onlineBodyPref.name, transform.position, Quaternion.identity);
        onlinePlayer = onlineBody.GetComponent<OnlinePlayer>();
        onlinePlayer.hideBody();
    }

    // Добавление никнейма в список объектов, которые будут поворачиваться к камере
    public void AddNickname(GameObject nickname)
    {
        nicknames.Add(nickname);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < nicknames.Count; i++)
        {
            nicknames[i].transform.LookAt(transform.position + new Vector3(0, 1.8f));
        }
    }

    private void Update()   //Меняет положение своего тела, которое отрпавляеться всем остальным.
    {
        if (PhotonNetwork.IsConnected && onlinePlayer != null)
        {
            for (int i = 0; i < bodyPoints.Length; ++i)
            {
                onlinePlayer.SetTransform(bodyPoints[i], i);
            }
        }
    }
}
