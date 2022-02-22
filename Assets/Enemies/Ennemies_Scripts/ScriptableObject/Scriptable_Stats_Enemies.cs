using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemie stats" ,menuName = "Enemie Stats")]
public class Scriptable_Stats_Enemies : ScriptableObject
{

    [SerializeField] private new  string name; // enemie name
    [SerializeField] private int healthPoints; // hp
    [SerializeField] private int defensePoints; // reduce the receive damage from player
    [SerializeField] private int attackPower; // damage
    [SerializeField] private float attackRange; // melle attack range
    [SerializeField] private float detectionRange; // range player detection
    [SerializeField] private float impluseForce; // push player
    public string Name       { get => name; }
    public int DefensePoints { get => defensePoints; }
    public int AttackPower   { get => attackPower;  }
    public int HealthPoints  { get => healthPoints;   }
    public float AttackRange { get => attackRange;     }
    public float DetectionRange { get => detectionRange; }
    public float ImpluseForce { get => impluseForce;  }
}   
