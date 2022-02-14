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
    private int currentAmmo = 10;

    private void Awake()
    {
        base.maxBullet = 10;
        base.noAmmo = false;
    }

    public void ShootPistol()
    {
        Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        currentAmmo--;
        if (currentAmmo <= 0) base.noAmmo = true;
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight();
        }
        if (Input.GetButtonDown("Fire1") && attackOnce && currentAmmo > 0)
        {
            base.Attacking("Shoot", resetTimeShot);
            ShootPistol();
            print(currentAmmo);
        }
        if (Input.GetButtonDown("Fire3"))
        {
            base.Reloading("PistolIsAim");
            currentAmmo = maxBullet;
        }
    }
}
