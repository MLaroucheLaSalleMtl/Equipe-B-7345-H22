using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStats : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    
    void Awake()
    {
        player.HealthPoints = player.MaxHP;
        player.LastCheckpoint = this.gameObject.transform.position;
        player.GotMarcPiece = false;
        player.GotSebPiece = false;
        player.GotStevenPiece = false;
    }
}
