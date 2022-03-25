using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBehavior : Attack
{
    [SerializeField] private WeaponDamage damage;
    protected float resetTimeShot = 1.1f; //time between each individual shot
    protected float bulletSpeed = 100;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private Camera AimCamera;
    private bool canAim = true;

    private void Awake()
    {
        base.maxBullet = 5;
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
        canAim = true;
    }

    public void ShootSniper()
    {
        if(isAiming)
        {
            damage.AmmoCount--;
            shotOffset = new Vector3(0f, 0f, 0f);
            Rigidbody clone1 = Instantiate(bullets, AimCamera.gameObject.transform.position, AimCamera.gameObject.transform.rotation);
            clone1.AddForce((AimCamera.gameObject.transform.forward + shotOffset) * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(ActivateRenderBullet(clone1, 0f));
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        }
        else if(!isAiming)
        {
            damage.AmmoCount--;
            //shotOffset = new Vector3(Random.Range(AimCamera.gameObject.transform.forward.x - 0.005f, AimCamera.gameObject.transform.forward.x + 0.005f), Random.Range(AimCamera.gameObject.transform.forward.y - 0.05f, AimCamera.gameObject.transform.forward.y + 0.05f), 0f);
            Rigidbody clone1 = Instantiate(bullets, hipShot.transform.position, muzzle.transform.rotation);
            clone1.AddForce(hipShot.transform.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(ActivateRenderBullet(clone1, 0.12f));
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

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 9 || other.gameObject.layer == 6)
        {
            if(base.isAiming)
            {
                AimDownSight();
            }
            canAim = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 6)
        {
            canAim = true;
        }
    }

    void Update()
    {
        DisplayUI();
        if (control.AimDownSightsInput && canAim)
        {
            control.AimDownSightsInput = false;
            base.AimDownSight();
        }
        if (control.FireInput && attackOnce && damage.AmmoCount > 0 && !isReloading)
        {
            control.FireInput = false;
            base.Attacking("Shoot", resetTimeShot);
            ShootSniper();
            base.currAmmo = damage.AmmoCount;
        }
        if ((control.ReloadInput || damage.AmmoCount <= 0) && damage.AmmoCount != maxBullet)
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
