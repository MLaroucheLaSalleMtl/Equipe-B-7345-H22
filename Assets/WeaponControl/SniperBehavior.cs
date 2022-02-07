using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBehavior : Attack
{
    protected float resetTimeShot = 1.1f; //time between each individual shot
    protected float range = 30.0f;
    protected float bulletSpeed = 50;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject cameraScope;
    

    public void ShootSniper()
    {
        if(isAiming)
        {
            Rigidbody clone1 = Instantiate(bullets, cameraScope.transform.position, cameraScope.transform.rotation);
            clone1.AddForce(cameraScope.transform.forward * bulletSpeed, ForceMode.Impulse);
        }
        else if(!isAiming)
        {
            Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
            clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        }
        
    }

    void Update()
    {
        base.attackRange = range;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight(true);
        }
        if (Input.GetButtonDown("Fire1") && attackOnce)
        {
            base.Attacking("Shoot", resetTimeShot);
            ShootSniper();
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
