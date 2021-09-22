using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("Другое")] [SerializeField] private Transform target;
    [SerializeField] private GameObject canvasCenter;
    private Vector3 position;

    [SerializeField] private LocomotionConstant locomotion;
    [SerializeField] private GameObject[] enableOnGameStart;
    [SerializeField] private GameObject[] disableOnGameStart;
    [SerializeField] private SteamVR_LaserPointer leftLaserPointer;
    [SerializeField] private SteamVR_LaserPointer rightLaserPointer;

    [SerializeField] private GameObject engine;
    [SerializeField] private DBForOptimizedEngine engineScript;

    [Header("Выбор комнат и сетевой код")]
    [Tooltip("Поставьте галочку в PlayMode, чтобы вызвался метод входа в случайную комнату")]
    [SerializeField] private bool executeJoinRandomRoom = false;
    [SerializeField] private Networking networking;

    private void Start()
    {
        executeJoinRandomRoom = false;

        // yield return new WaitForSeconds(0.1f);

        // Генерируем коллайдеры и отключаем объект.
        // engine.SetActive(false);
    }
    
    private void LateUpdate()
    {
        // Центруем интерфейс относительно положения игрока
        position = target.transform.position;
        position.y = -0.83f;
        canvasCenter.transform.position = position;

        if (executeJoinRandomRoom || Input.GetKeyDown(KeyCode.X))
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
        if (leftLaserPointer.pointer) Destroy(leftLaserPointer.pointer);
        if (rightLaserPointer.pointer) Destroy(rightLaserPointer.pointer);

        foreach (var gObject in enableOnGameStart)
        {
            if (gObject) gObject.SetActive(true);

            if (gObject == engine) engineScript.enabled = true;
        }

        foreach (var gObject in disableOnGameStart)
        {
            if (gObject) gObject.SetActive(false);
        }

        //locomotion.enabled = true;
    }

    public void JoinRandomRoom()
    {
        networking.JoinRandomRoom();
    }
}