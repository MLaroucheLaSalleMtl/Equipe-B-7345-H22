using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int healthPoints;
    [SerializeField] private Vector3 lastCheckpoint;
    [SerializeField] private int playerLevel;

    public int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public Vector3 LastCheckpoint { get => lastCheckpoint; set => lastCheckpoint = value; }
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
}
