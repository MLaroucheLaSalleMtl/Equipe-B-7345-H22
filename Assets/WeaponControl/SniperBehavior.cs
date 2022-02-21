using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBehavior : Attack
{
    [SerializeField] private WeaponDamage damage;
    protected float resetTimeShot = 1.1f; //time between each individual shot
    protected float bulletSpeed = 50;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private Camera AimCamera;

    private void Awake()
    {
        base.maxBullet = 5;
        base.noAmmo = false;
        base.currAmmo = 5;
        base.currAmmo = damage.AmmoCount;
    }

    public void ShootSniper()
    {
        if(isAiming)
        {
            damage.AmmoCount--;
            shotOffset = new Vector3(0f, 0f, 0f);
            Rigidbody clone1 = Instantiate(bullets, AimCamera.gameObject.transform.position, AimCamera.gameObject.transform.rotation);
            clone1.AddForce((AimCamera.gameObject.transform.forward + shotOffset) * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        }
        else if(!isAiming)
        {
            damage.AmmoCount--;
            //shotOffset = new Vector3(Random.Range(AimCamera.gameObject.transform.forward.x - 0.005f, AimCamera.gameObject.transform.forward.x + 0.005f), Random.Range(AimCamera.gameObject.transform.forward.y - 0.05f, AimCamera.gameObject.transform.forward.y + 0.05f), 0f);
            Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
            clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        }
        if (damage.AmmoCount <= 0) base.noAmmo = true;
    }

    public void CameraZoom()
    {
        AimCamera.gameObject.SetActive(true);
    }
    public void CameraDezoom()
    {
        AimCamera.gameObject.SetActive(false);
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
            ShootSniper();
            base.currAmmo = damage.AmmoCount;
        }
        if (control.ReloadInput)
        {
            control.ReloadInput = false;
            base.Reloading("SniperIsAim");
            if (!isAiming)
            {
                damage.AmmoCount = maxBullet;
                base.currAmmo = damage.AmmoCount;
            }
        }
    }
}
