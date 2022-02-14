using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArBehavior : Attack
{
    protected float resetTimeShot = 0.8f; //time between each individual shot
    protected float range = 20.0f;
    protected float bulletSpeed = 20;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;

    public IEnumerator ShootRifle()
    {
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone2 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone2.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone3 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone3.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
    }
    void Update()
    {

        base.attackRange = range;
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * attackRange, Color.red);
        if (Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight(false);
        }
        if (Input.GetButtonDown("Fire1") && attackOnce)
        {
            StartCoroutine(ShootRifle());
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
