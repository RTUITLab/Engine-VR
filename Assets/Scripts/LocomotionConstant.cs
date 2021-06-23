using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LocomotionConstant : MonoBehaviour
{

    [SerializeField] SteamVR_Action_Vector2 M_action;
    [SerializeField] private Animator animator;
    SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");

    [SerializeField] SteamVR_Input_Sources Locomition_source;

    [SerializeField] float Rotating_angle = 45;

    [SerializeField] private bool keyboardInput;

    [SerializeField] float speed = 1;
    [SerializeField] private float walkingSmoothness = 0.2f;

    public OnlinePlayer onlinePlayer;
    private Transform MainCamera;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        if (keyboardInput)
        {
            Debug.LogWarning("<b>Установлен метод перемещения через кнопки клавиатуры!</b>");
            FindObjectOfType<Valve.VR.InteractionSystem.FallbackCameraController>().enabled = false;
            Debug.LogWarning("Конфликтное управление камерой в 2D режиме отключено");
        }
    }

    void CalculateRotation()
    {
        if (snapRightAction.GetLastStateDown(SteamVR_Input_Sources.RightHand))
        {
            transform.Rotate(transform.up, Rotating_angle);
        }
        if (snapLeftAction.GetLastStateDown(SteamVR_Input_Sources.RightHand))
        {
            transform.Rotate(transform.up, -Rotating_angle);

        }
    }


    void CalculateMovement()
    {
        Quaternion orientation = CalculateOrientation();

        Vector2 direction;
        if (keyboardInput)
            direction = Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up;
        else
            direction = M_action.GetAxis(Locomition_source).x * Vector2.right + M_action.GetAxis(Locomition_source).y * Vector2.up;
        Debug.Log(M_action.GetAxis(Locomition_source));


        float M_speed = direction.magnitude * speed;

        if (M_speed == 0)
        {
            direction = Vector2.zero;
        }

        // Сглаживание
        Vector2 oldDirection = new Vector2(animator.GetFloat("PosX"), animator.GetFloat("PosY"));
        Vector2 smoothed = Vector2.Lerp(oldDirection, direction, walkingSmoothness);

        animator.SetFloat("PosX", smoothed.x);
        animator.SetFloat("PosY", smoothed.y);

        if (onlinePlayer != null)
        {
            onlinePlayer.SendMovementDirection(smoothed);
        }

        Vector3 movement = orientation * Vector3.forward * (M_speed * Time.deltaTime);
        transform.position += movement;
    }

    private Quaternion CalculateOrientation()
    {
        float rotation;

        if (keyboardInput)
            rotation = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            rotation = Mathf.Atan2(M_action.GetAxis(Locomition_source).x, M_action.GetAxis(Locomition_source).y);

        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, MainCamera.transform.eulerAngles.y + rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    private void Update()
    {
        CalculateRotation();
        CalculateMovement();
    }
}
