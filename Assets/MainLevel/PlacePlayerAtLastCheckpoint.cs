using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayerAtLastCheckpoint : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject playergo;
    private void Awake()
    {
        if(player.IsContinuing)
        {
            playergo.transform.position = player.LastCheckpoint;
            player.IsContinuing = false;
        }
    }
}
