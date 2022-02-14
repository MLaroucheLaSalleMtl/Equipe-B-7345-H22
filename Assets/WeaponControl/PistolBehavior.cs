using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehavior : Attack
{
    protected float resetTimeShot = 0.5f; //time between each individual shot
    protected float range = 15.0f;
    protected float bulletSpeed = 20;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;



    public void ShootPistol()
    {
        Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
    }


    void Update()
    {
        base.attackRange = range;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if(Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight(false);
        }
        if (Input.GetButtonDown("Fire1") && attackOnce)
        {
            
            base.Attacking("Shoot", resetTimeShot);
            ShootPistol();
            RaycastHit hit;
            if (Physics.Raycast(base.player.transform.position, base.player.transform.forward, out hit, attackRange))
            {
                if (hit.collider.tag == "Enemy")
                {
                    print("Done");
                }
            }
        }
    }
}
