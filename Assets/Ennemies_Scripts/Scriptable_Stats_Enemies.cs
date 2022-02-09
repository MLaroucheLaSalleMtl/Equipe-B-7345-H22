using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Enemie stats" ,menuName = "Enemie Stats")]
public class Scriptable_Stats_Enemies : ScriptableObject
{
    

    [SerializeField] private new  string name;
    [SerializeField] private int healthPoints;
    [SerializeField] private int defensePoints;
    [SerializeField] private int attackPower;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectionRange;
    [SerializeField] private float impluseForce;
    public string Name       { get => name; }
    public int DefensePoints { get => defensePoints; }
    public int AttackPower   { get => attackPower;  }
    public int HealthPoints  { get => healthPoints;   }
    public float AttackRange { get => attackRange;     }
    public float DetectionRange { get => detectionRange; }
    public float ImpluseForce { get => impluseForce;  }
}   
