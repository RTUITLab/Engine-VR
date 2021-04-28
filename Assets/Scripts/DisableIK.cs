using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(VRIK))]
public class DisableIK : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private Vector3 lastPlayerPosition;
    private VRIK vrik;

    private void Start()
    {
        vrik = GetComponent<VRIK>();
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
        vrik.enabled = false;
        yield return new WaitForEndOfFrame();
        vrik.enabled = true;
    }
}
