using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OnlinePlayer : MonoBehaviour
{
    [SerializeField] private Transform[] bodyPoints;    //Точки тела которые нужно синхронизировать с локальным телом.
    [SerializeField] private GameObject[] want2Hide;    //Элементы тела, которые не должны быть видны у локального плеера.

    [SerializeField] private PhotonView photonView;
    [SerializeField] private Text nicknameOutput;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform robot;
    [SerializeField] private float walkingSmoothness = 0.2f;

    private Vector3 previousPosition;
    private Vector3 positionOffset;
    private Vector3 oldDirection;
    private Vector3 smoothed;

    public void SetTransform(Transform transform, int id)  
    {
        try
        {
            bodyPoints[id].SetPositionAndRotation(transform.position, transform.rotation);
        }
        catch(IndexOutOfRangeException e)
        {
            Debug.LogError($"В массиве точкет несуществует id:{id}. {e.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void hideBody()  
    {
        for (int i = 0; i < want2Hide.Length; ++i)
        {
            want2Hide[i].SetActive(false);
        }
    }

    public void HideNickname()
    { // Скрывает никнейм самого игрока, оставляя только никнеймы напарников
        nicknameOutput.gameObject.SetActive(false);
    }

    public void SendNickname(string nickname)
    {
        photonView.RPC("sendNickname", RpcTarget.All, nickname);
    }

    [PunRPC]
    private void sendNickname(string nickname)
    {
        nicknameOutput.text = nickname;
        Debug.Log("Send Nickname executed: nickname - " + nickname);
    }

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        positionOffset = (robot.position - previousPosition);
        Vector3 localPosition = Vector3.Normalize(robot.InverseTransformDirection(positionOffset));
        //oldDirection = new Vector3(animator.GetFloat("PosX"), 0f, animator.GetFloat("PosY"));
        //smoothed = Vector3.Lerp(oldDirection, localPosition, walkingSmoothness);
        animator.SetFloat("PosX", localPosition.x);
        animator.SetFloat("PosY", -localPosition.z);
        previousPosition = robot.position;
    }
}
