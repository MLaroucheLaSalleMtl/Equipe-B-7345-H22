using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehavior : Attack
{
    protected float resetTimeSwing = 0.4f; //time between each individual shot

    void Update()
    {
        base.attackRange = 15.0f;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire1"))
        {
            base.Attacking("Shoot", resetTimeSwing);
            RaycastHit hit;
            if (Physics.Raycast(base.player.transform.position, base.player.transform.forward, out hit, attackRange))
            {
                print("In");
                if (hit.collider.tag == "Enemy")
                {
                    print("Done");

                }
            }
        }
    }
}
