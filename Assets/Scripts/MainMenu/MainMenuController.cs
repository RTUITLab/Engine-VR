using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("Другое")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject canvasCenter;
    private Vector3 position;

    [SerializeField] private GameObject[] startGameObjects;

    [Header("Выбор комнат и сетевой код")]
    [SerializeField] private Networking networking;

    private void Start()
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
        foreach (var startGameObject in startGameObjects)
        {
            startGameObject.SetActive(true);
        }
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
