using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    public abstract void DoDamage(Vector3 position, float attackRange, float damage);
}
