using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBehavior : Attack
{
    protected float resetTimeShot = 1.1f; //time between each individual shot
    protected float bulletSpeed = 50;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private Camera AimCamera;
    private int currentAmmo = 5;

    private void Awake()
    {
        base.maxBullet = 5;
        base.noAmmo = false;
    }


    public void ShootSniper()
    {
        if(isAiming)
        {
            Rigidbody clone1 = Instantiate(bullets, AimCamera.gameObject.transform.position, AimCamera.gameObject.transform.rotation);
            clone1.AddForce(AimCamera.gameObject.transform.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
            currentAmmo--;
        }
        else if(!isAiming)
        {
            Rigidbody clone1 = Instantiate(bullets, muzzle.transform.position, muzzle.transform.rotation);
            clone1.AddForce(muzzle.transform.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
            currentAmmo--;
        }
        if (currentAmmo <= 0) base.noAmmo = true;
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
        if (Input.GetButtonDown("Fire2"))
        {
            base.AimDownSight();
        }
        if (Input.GetButtonDown("Fire1") && attackOnce && currentAmmo > 0)
        {
            base.Attacking("Shoot", resetTimeShot);
            ShootSniper();
            print(currentAmmo);
        }
        if (Input.GetButtonDown("Fire3"))
        {
            base.Reloading("SniperIsAim");
            currentAmmo = maxBullet;
        }
    }
}
