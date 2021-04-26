using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class OnlinePlayer : MonoBehaviour
{
    [SerializeField] private Transform[] bodyPoints;    //Точки тела которые нужно синхронизировать с локальным телом.
    [SerializeField] private GameObject[] want2Hide;    //Элементы тела, которые не должны быть видны у локального плеера.

    [SerializeField] private PhotonView photonView;
    [SerializeField] private TMP_Text nicknameOutput;

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
}
