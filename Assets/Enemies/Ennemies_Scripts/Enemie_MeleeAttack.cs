using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie_MeleeAttack : MonoBehaviour
{
    private Enemie enemie;
    private float meleeHitRange = 3f;
    public void Awake()
    {
        enemie = GetComponentInParent<Enemie>();
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            print("hit");
            this.enemie.AdaptiveForce(meleeHitRange,enemie.MeleeImpluseForce);
        }
    }

}
