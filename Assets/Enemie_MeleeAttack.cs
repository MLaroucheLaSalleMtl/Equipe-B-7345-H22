using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie_MeleeAttack : MonoBehaviour
{
    private Enemie enemie;
    [SerializeField]private PlayerStats playerStats;
    public void Awake()
    {
        enemie = GetComponentInParent<Enemie>();
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            print("hit");
            this.enemie.AdaptiveForce(other);
            this.playerStats.HealthPoints -= this.enemie.RealDamage;
        }
    }

}
