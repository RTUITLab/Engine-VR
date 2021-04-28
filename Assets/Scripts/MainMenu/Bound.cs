﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction = player.position - new Vector3(3f, -0.5f, -7f);
        player.position = player.position -  direction.normalized * 2f;
    }
}
