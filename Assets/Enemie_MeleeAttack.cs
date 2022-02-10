using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie_MeleeAttack : MonoBehaviour
{
    //[SerializeField] private Scriptable_Stats_Enemies enemie;
    private Enemie enemie;
    private void Awake()
    {
        enemie = GetComponentInParent<Enemie>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            this.enemie.AdaptiveForce(other);
            other.gameObject.GetComponentInParent<PlayerController>().Hp -= this.enemie.RealDamage;
        }
    }
   
}
