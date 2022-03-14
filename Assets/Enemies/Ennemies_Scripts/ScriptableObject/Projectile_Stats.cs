using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Stats", menuName = "Projectile Stats")]
public class Projectile_Stats : ScriptableObject
{

    [SerializeField] private  float lazerMaxRange ;
    [SerializeField] private  float lazerMinRange ;
    [SerializeField] private  float lazerImpulseForce;
    [SerializeField] private  float lazerDamage ;
    [SerializeField] private  float lazerResetTime;

    public float LazerMaxRange { get => lazerMaxRange; set => lazerMaxRange = value; }
    public float LazerMinRange { get => lazerMinRange; set => lazerMinRange = value; }
    public float LazerImpulseForce { get => lazerImpulseForce; set => lazerImpulseForce = value; }
    public float LazerDamage { get => lazerDamage; set => lazerDamage = value; }
    public float LazerResetTime { get => lazerResetTime; set => lazerResetTime = value; }
}
