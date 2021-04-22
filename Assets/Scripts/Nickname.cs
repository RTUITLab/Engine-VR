using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    [SerializeField] private Transform nickname;

    private void LateUpdate()
    {
        //// Поворачиваем все никнеймы в сторону камеры
        //transform.LookAt(Camera.main.transform.position);
        //// nickname.eulerAngles += new Vector3(90, -90, 0);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        //// Никнейм всегда перпендикулярен поверхности
        var rotation = nickname.rotation;
        rotation.y = 0;
        rotation.z = 0;
        nickname.rotation = rotation;
    }
}
