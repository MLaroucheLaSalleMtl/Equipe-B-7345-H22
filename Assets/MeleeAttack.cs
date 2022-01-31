using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float meleeAttackRange = 3.0f;
    private bool attackOnceMelee = true;
    
    public void MeleeAttackSwing()
    {
        
        if (attackOnceMelee)
        {
            anim.SetBool("Swing", true);
            attackOnceMelee = false;
            Invoke("ResetSwing", 1.0f);
            DoDamage();
        }
        

    }
    public void ResetSwing()
    {
        anim.SetBool("Swing", false);
        attackOnceMelee = true;
    }

    public void DoDamage()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, meleeAttackRange))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("Done");
            }
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAttackSwing();
        }
    }
}
