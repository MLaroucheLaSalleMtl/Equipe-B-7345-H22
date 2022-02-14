using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArBehavior : Attack
{
    protected float resetTimeShot = 0.8f; //time between each individual shot
    protected float bulletSpeed = 20;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    private int currentAmmo = 30;

    private void Awake()
    {
        base.maxBullet = 30;
        base.noAmmo = false;
    }

    public IEnumerator ShootRifle()
    {
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        currentAmmo--;
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone2 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone2.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(clone2.GetComponent<DamageDone>().BreakDistance());
        currentAmmo--;
        yield return new WaitForSeconds(0.1f);
        Rigidbody clone3 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone3.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(clone3.GetComponent<DamageDone>().BreakDistance());
        currentAmmo--;
        if (currentAmmo <= 0) base.noAmmo = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight();
        }
        if (Input.GetButtonDown("Fire1") && attackOnce && currentAmmo > 0)
        {
            StartCoroutine(ShootRifle());
            base.Attacking("Shoot", resetTimeShot);
            print(currentAmmo);
        }
        if(Input.GetButtonDown("Fire3"))
        {
            base.Reloading("ArIsAim");
            currentAmmo = maxBullet;
        }
    }
}
