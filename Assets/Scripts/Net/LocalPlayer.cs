using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

// Eloren1: Этот скрипт вообще работает? На сцене его не найти при запуске игры. Как и OnlinePlayer. (TODO:)
public class LocalPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject onlineBodyPref;     //Префаб онлайн тела.
    [SerializeField] private Transform[] bodyPoints;    //Точки тела, которые нужно синхронизировать с оналйн телом.
    private Transform transform;
    private OnlinePlayer onlinePlayer = null;

    private void Awake()
    {
        transform = gameObject.transform;
    }

    public override void OnJoinedRoom() //При входе, создаёт своё тело и запоминает его, что бы менять ему положение.
    {
        Debug.Log("LocalPlayer");
        FindObjectOfType<MainMenuController>().StartGame();
        GameObject onlineBody = PhotonNetwork.Instantiate(onlineBodyPref.name, transform.position, Quaternion.identity);
        onlinePlayer = onlineBody.GetComponent<OnlinePlayer>();
        onlinePlayer.hideBody();

        SendNickname();
    }

    public void SendNickname()
    {
        string nickname = PlayerPrefs.GetString("Nickname");
        onlinePlayer.SendNickname(nickname);
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
