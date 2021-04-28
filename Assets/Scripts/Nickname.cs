using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    [SerializeField] private Transform nickname;
    private Transform localPlayerHead;

    private void Start()
    {
        localPlayerHead = GameObject.Find("FollowHead").transform;
    }

    private void LateUpdate()
    {
        // Поворачиваем все никнеймы в сторону камеры
        nickname.LookAt(localPlayerHead);
    }
}
