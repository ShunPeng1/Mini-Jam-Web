using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeatMapFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }

}
