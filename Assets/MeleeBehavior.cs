using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehavior : Attack
{

    protected float resetTimeSwing = 1.0f; //time between each individual swings

    void Update()
    {
        base.attackRange = 3.0f;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire1"))
        {
            base.Attacking("Swing", resetTimeSwing);
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
