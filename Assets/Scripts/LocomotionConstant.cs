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

    [SerializeField] float speed = 1;
    [SerializeField] private float walkingSmoothness = 0.2f;
    
    
    Transform MainCamera;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
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
        float M_speed = M_action.GetAxis(Locomition_source).magnitude * speed;

        Vector2 direction = M_action.GetAxis(Locomition_source);
        // direction.z *= -1;
        if (M_speed == 0)
        {
            direction = Vector2.zero;
        }
        
        
        // Сглаживание
        Vector3 oldDirection = new Vector2(animator.GetFloat("PosX"), animator.GetFloat("PosY"));
        Vector3 smoothed = Vector3.Lerp(oldDirection, direction, walkingSmoothness);
        animator.SetFloat("PosX", smoothed.x);
        animator.SetFloat("PosY", smoothed.y);

        Vector3 movement = orientation * Vector3.forward * (M_speed * Time.deltaTime);
        transform.position += movement;
    }

    private Quaternion CalculateOrientation()
    {
        float rotation = Mathf.Atan2(M_action.GetAxis(Locomition_source).x, M_action.GetAxis(Locomition_source).y);
        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, MainCamera.transform.eulerAngles.y + rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CalculateRotation();
    }
}
