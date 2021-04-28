using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal.Internal;

[RequireComponent(typeof(VRIK))]
public class DisableIK : MonoBehaviour
{
    public float comanglemlp;
    public float stepspeed;
    public float rootspeed;
    [SerializeField] private Transform playerTransform;

    private Vector3 lastPlayerPosition;
    private VRIK vrik;

    private void Start()
    {
        vrik = GetComponent<VRIK>();

        //vrik.solver.plantFeet = false;
    }

    private void Update()
    {
        if (Vector3.Distance(playerTransform.position, lastPlayerPosition) > 2f)
        {
            StartCoroutine(DisableVRIK());
        }

        
    }

    private IEnumerator DisableVRIK()
    {
        // vrik.solver.plantFeet = false;
        // vrik.solver.leftLeg.positionWeight = 1f;
        // vrik.solver.leftLeg.rotationWeight = 1f;
        // vrik.solver.rightLeg.positionWeight = 1f;
        // vrik.solver.rightLeg.rotationWeight = 1f;
        vrik.solver.plantFeet = false;
        vrik.solver.locomotion.comAngleMlp = comanglemlp;//50;
        vrik.solver.locomotion.stepSpeed = stepspeed;//30;
        vrik.solver.locomotion.rootSpeed = rootspeed;//200;
        
        
        yield return new WaitForSeconds(0.2f);

        vrik.solver.plantFeet = true;
        vrik.solver.locomotion.comAngleMlp = 1;
        vrik.solver.locomotion.stepSpeed = 3;
        vrik.solver.locomotion.rootSpeed = 20;

        // vrik.solver.plantFeet = true;
        // vrik.solver.leftLeg.positionWeight = 0f;
        // vrik.solver.leftLeg.rotationWeight = 0f;
        // vrik.solver.rightLeg.positionWeight = 0f;
        // vrik.solver.rightLeg.rotationWeight = 0f;
    }

    private void LateUpdate()
    {
        lastPlayerPosition = playerTransform.position;
    }
}
