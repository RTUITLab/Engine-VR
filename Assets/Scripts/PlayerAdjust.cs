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

    [SerializeField] float WristsDistanceLeft, WristsDistanceRight;

    [SerializeField] Transform ModelWristLeft, BoneWristLeft;
    [SerializeField] Transform ModelWristRight, BoneWristRight;

    public Transform upperArmBoneLeft, lowerArmBoneLeft;
    public Transform upperArmBoneRight, lowerArmBoneRight;
    public float HandMultiplayer = 1;
    float scaleHight, scaleArms;
    //IEnumerable enumerable;

    IEnumerator AnjustHands()
    {
        yield return new WaitForSeconds(2);
        ModelWristLeft = GameObject.FindGameObjectWithTag("LeftWrist").transform;
        ModelWristRight = GameObject.FindGameObjectWithTag("RightWrist").transform;

    }

    private void Start()
    {
        StartCoroutine(AnjustHands());
    }

    void ChangeHandScale(float Distance, Transform UpperArm, Transform LowerArm)
    {
        float Size = 1 + Distance * HandMultiplayer / 2;
        Vector3 NewScale = new Vector3(Size, Size, Size);
        UpperArm.localScale = NewScale;
        LowerArm.localScale = NewScale;
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
        if (ModelWristLeft && ModelWristRight)
        {
            WristsDistanceLeft = Vector3.Distance(ModelWristLeft.position, BoneWristLeft.position);
            WristsDistanceRight = Vector3.Distance(ModelWristRight.position, BoneWristRight.position);

            if (WristsDistanceLeft > 0.02f) ChangeHandScale(WristsDistanceLeft , upperArmBoneLeft, lowerArmBoneLeft);
            if (WristsDistanceRight > 0.02f) ChangeHandScale(WristsDistanceRight, upperArmBoneRight, lowerArmBoneRight);

        }


    }
}
