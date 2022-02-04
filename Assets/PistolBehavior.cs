using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehavior : Attack
{
    protected float resetTimeShot = 0.1f; //time between each individual shot
    protected float range = 15.0f;

    void Update()
    {
        base.attackRange = 15.0f;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire1") && attackOnce)
        {
            base.Attacking("Shoot", resetTimeShot);
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
