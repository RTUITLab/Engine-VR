using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private Networking networking;

    private void Start()
    {
<<<<<<< Updated upstream
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;

        // test
=======
        
>>>>>>> Stashed changes
    }

    public void JoinRandomRoom()
    {
        SceneManager.LoadScene(1);
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
