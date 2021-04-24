using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("Другое")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject canvasCenter;
    private Vector3 position;

    [SerializeField] private GameObject[] enableOnGameStart;
    [SerializeField] private GameObject[] disableOnGameStart;

    [Header("Выбор комнат и сетевой код")]
    [SerializeField] private Networking networking;

    private void Start()
    {
        JoinRandomRoom(); // TODO its a test!
    }

    private void LateUpdate()
    {
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;
    }

    public void JoinRandomRoom()
    {
        // Вызов рандомной комнаты в Networking

        StartGame();
    }

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
    }
    // TODO Реализовать остальные методы из меню и навесить их на кнопки

    // Нужно сохранять выбор (случайная комната, определенная комната), и при переходе на новую сцену
    // в классе Networking обрабатывать желаемое действие (там частично есть методы)
}
