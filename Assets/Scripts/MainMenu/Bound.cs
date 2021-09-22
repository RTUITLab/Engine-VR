using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float teleportModifier = 0.9f;
    private float startPositionY;

    private void Start()
    {
        startPositionY = player.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        player.position = new Vector3(player.position.x, 0, player.position.z) * teleportModifier +
            new Vector3(0, startPositionY, 0);
    }
}