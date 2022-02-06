using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehavior : Attack
{

    protected float resetTimeSwing = 0.9f; //time between each individual swings
    protected float range = 3.0f;

    void Update()
    {
        base.attackRange = range;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire1") && attackOnce)
        {
            base.Attacking("Swing", resetTimeSwing);
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
