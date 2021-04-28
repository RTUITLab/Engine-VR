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

        lastPlayerPosition = playerTransform.position;
    }

    private IEnumerator DisableVRIK()
    {
        Debug.Log("VRIK");
        vrik.enabled = false;
        yield return new WaitForSeconds(0.3f);
        vrik.enabled = true;
    }
}
