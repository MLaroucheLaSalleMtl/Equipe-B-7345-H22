using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStats : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject playergo;

    void Awake()
    {
        if (player.IsContinuing)
        {
            playergo.transform.position = player.LastCheckpoint;
            player.IsContinuing = false;
        }
        else
        {
            player.HealthPoints = player.MaxHP;
            player.LastCheckpoint = new Vector3(0f, 0f, 0f);
            player.GotMarcPiece = false;
            player.GotSebPiece = false;
            player.GotStevenPiece = false;
        }
    }
}
