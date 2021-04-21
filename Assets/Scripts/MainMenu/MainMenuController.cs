using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject canvasCenter;
    private Vector3 position;

    private void LateUpdate()
    {
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;

        // test
    }

    public void JoinRandomRoom()
    {
        SceneManager.LoadScene(1);

        Destroy(FindObjectOfType<Player>());
    }

    // TODO Реализовать остальные методы из меню и навесить их на кнопки

    // Нужно сохранять выбор (случайная комната, определенная комната), и при переходе на новую сцену
    // в классе Networking обрабатывать желаемое действие (там частично есть методы)
}
