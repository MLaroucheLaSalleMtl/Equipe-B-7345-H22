using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStats : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    
    void Start()
    {
        player.HealthPoints = player.MaxHP;
        player.LastCheckpoint = new Vector3(0f, 0f, 0f);
        player.GotMarcPiece = false;
        player.GotSebPiece = false;
        player.GotStevenPiece = false;
    }
}
