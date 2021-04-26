﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("Другое")] [SerializeField] private Transform target;
    [SerializeField] private GameObject canvasCenter;
    private Vector3 position;

    [SerializeField] private GameObject[] enableOnGameStart;
    [SerializeField] private GameObject[] disableOnGameStart;

    [Header("Выбор комнат и сетевой код")] [SerializeField]
    private Networking networking;

    private void Start()
    {
        //JoinRandomRoom(); // TODO its a test!
    }

    private void LateUpdate()
    {
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;
    }

    /// <summary>
    /// Вызывается при подключении к комнате из Networking. 
    /// Убирает меню и включает нужные объекты
    /// </summary>
    public void StartGame()
    {
        foreach (var gObject in enableOnGameStart)
        {
            gObject.SetActive(true);
        }

        foreach (var gObject in disableOnGameStart)
        {
            gObject.SetActive(false);
        }

        Destroy(GameObject.Find("PointerLine"));
        Destroy(GameObject.Find("PointerLine"));
    }

    public void OpenList()
    {
    }

    public void CreateRoom()
    {
        networking.CreateRoom();
    }

    public void JoinRandomRoom()
    {
        networking.JoinRandomRoom();
        //networking.JoinRoom("Room_1");
    }
    // TODO Реализовать остальные методы из меню и навесить их на кнопки
}