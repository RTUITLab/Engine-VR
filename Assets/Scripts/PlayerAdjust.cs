using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class PlayerAdjust : MonoBehaviour
{
    [SerializeField] Transform VRCamera;
    [SerializeField] SteamVR_Action_Boolean action;
    [SerializeField] SteamVR_Input_Sources source;
    [SerializeField] List<Transform> ChangeSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (action.GetStateDown(source))
        {
            float multiplayer = VRCamera.localPosition.y / 1.7f;
            foreach(Transform transform in ChangeSize)
            {
                transform.localScale = new Vector3(multiplayer, multiplayer, multiplayer);

            }
        }

    }
}
