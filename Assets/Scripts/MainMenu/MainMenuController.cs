using System.Collections;
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
    [SerializeField] private SteamVR_LaserPointer leftLaserPointer;
    [SerializeField] private SteamVR_LaserPointer rightLaserPointer;

    [Header("Выбор комнат и сетевой код")]
    [Tooltip("Поставьте галочку в PlayMode, чтобы вызвался метод входа в случайную комнату")]
    [SerializeField] private bool executeJoinRandomRoom = false;
    [SerializeField] private Networking networking;

    private void Start()
    {
        executeJoinRandomRoom = false;
    }

    private void LateUpdate()
    {
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;

        if (executeJoinRandomRoom)
        {
            executeJoinRandomRoom = false;
            JoinRandomRoom();
        }
    }

    /// <summary>
    /// Вызывается при подключении к комнате из Networking. 
    /// Убирает меню и включает нужные объекты
    /// </summary>
    public void StartGame()
    {
        /*Destroy(leftLaserPointer.pointer);
        Destroy(rightLaserPointer.pointer);*/
        foreach (var gObject in enableOnGameStart)
        {
            gObject.SetActive(true);
        }

        foreach (var gObject in disableOnGameStart)
        {
            gObject.SetActive(false);
        }

       
    }

    public void JoinRoom()
    {

    }

    public void CreateRoom()
    {
        networking.CreateRoom();
    }

    public void JoinRandomRoom()
    {
        networking.JoinRandomRoom();
    }
    // TODO Реализовать остальные методы из меню и навесить их на кнопки
}