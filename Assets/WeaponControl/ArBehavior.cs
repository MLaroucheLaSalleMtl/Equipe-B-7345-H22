using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArBehavior : Attack
{
    protected float resetTimeShot = 0.8f; //time between each individual shot
    protected float range = 20.0f;

    void Update()
    {
        base.attackRange = range;
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
