using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehavior : Attack
{
    [SerializeField] private WeaponDamage damage;
    protected float resetTimeShot = 0.5f; //time between each individual shot
    protected float range = 15.0f;
    protected float bulletSpeed = 75;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    

    private void Awake()
    {
        base.maxBullet = 10;
        base.noAmmo = false;
        damage.AmmoCount = maxBullet;
        base.currAmmo = damage.AmmoCount;
    }

    public void ShootPistol()
    {
        //if(!isAiming)
        //{
        //    shotOffset = new Vector3(Random.Range(muzzle.transform.forward.x - 0.005f, muzzle.transform.forward.x + 0.005f), Random.Range(muzzle.transform.forward.y - 0.005f, muzzle.transform.forward.y + 0.005f), 0f);
        //}
        //else if(isAiming)
        //{
        //    shotOffset = new Vector3(0f, 0f, 0f);
        //}
        damage.AmmoCount--;
        Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
        clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        if (damage.AmmoCount <= 0) base.noAmmo = true;
    }


    void Update()
    {
        DisplayUI();
        if (control.AimDownSightsInput)
        {
            control.AimDownSightsInput = false;
            base.AimDownSight();
        }
        if (control.FireInput && attackOnce && damage.AmmoCount > 0)
        {
            control.FireInput = false;
            base.Attacking("Shoot", resetTimeShot);
            ShootPistol();
            base.currAmmo = damage.AmmoCount;
        }
        if (control.ReloadInput)
        {
            control.ReloadInput = false;
            base.Reloading("PistolIsAim");
            if(!isAiming)
            {
                damage.AmmoCount = maxBullet;
                base.currAmmo = damage.AmmoCount;
            }
        }
    }
}
