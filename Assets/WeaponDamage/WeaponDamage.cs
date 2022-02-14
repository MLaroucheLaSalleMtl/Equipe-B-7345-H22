using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDamage", menuName = "WeaponDamage")]
public class WeaponDamage : ScriptableObject
{
    [SerializeField] private int damage;

    public int Damage { get => damage; set => damage = value; }
}
