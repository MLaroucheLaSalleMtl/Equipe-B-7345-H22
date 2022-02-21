using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie_MeleeAttack : MonoBehaviour
{
    private Enemie enemie;
    [SerializeField]private PlayerStats playerStats;
    private void Awake()
    {
        enemie = GetComponentInParent<Enemie>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            this.enemie.AdaptiveForce(other);
            //other.gameObject.GetComponent<PlayerController>().Hp -= this.enemie.RealDamage;
            this.playerStats.HealthPoints -= this.enemie.RealDamage;
            //this.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
   
}
