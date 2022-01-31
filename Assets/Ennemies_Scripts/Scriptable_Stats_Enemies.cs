using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Enemie stats" ,menuName = "Enemie Stats")]
public class Scriptable_Stats_Enemies : ScriptableObject
{
    

   [SerializeField]private new  string name;
   [SerializeField] private int healthPoints;
   [SerializeField] private int defensePoints;
   [SerializeField] private int attackPower;
   [SerializeField] private GameObject target;

    public string Name { get => name;                }
    public int DefensePoints { get => defensePoints; }
    public int AttackPower { get => attackPower;     }
    public GameObject Target { get => target;        }
    public int HealthPoints { get => healthPoints;   }
}   
