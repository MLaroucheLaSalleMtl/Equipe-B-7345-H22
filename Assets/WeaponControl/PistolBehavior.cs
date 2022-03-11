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

    private void OnEnable()
    {
        attackOnce = true;
        isAiming = false;
        isShooting = false;
        isReloading = false;
    }
    public void ShootPistol()
    {
        //if (!isAiming)
        //{
        //    shotOffset = new Vector3(Random.Range(muzzle.transform.localPosition.x - 1, muzzle.transform.localPosition.x + 1), Random.Range(muzzle.transform.localPosition.y - 1, muzzle.transform.localPosition.y + 1), 0f);
        //}
        //else if (isAiming)
        //{
        //    shotOffset = new Vector3(0f, 0f, 0f);
        //}
        damage.AmmoCount--;
        Rigidbody clone1 = Instantiate(bullets, hipShot.transform.position, muzzle.transform.rotation);
        //clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
        clone1.AddForce(hipShot.transform.forward * bulletSpeed, ForceMode.Impulse);
        if(isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0f));
        if(!isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0.04f));
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
        //Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * range, Color.red);
        //Debug.DrawRay(base.player.transform.position, base.player.transform.forward * range, Color.yellow);
    }
}
