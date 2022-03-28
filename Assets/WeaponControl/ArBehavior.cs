using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArBehavior : Attack
{
    [SerializeField] private WeaponDamage damage;
    protected float resetTimeShot = 0.8f; //time between each individual shot
    protected float bulletSpeed = 75;
    [SerializeField] private Rigidbody bullets;
    [SerializeField] private GameObject muzzle;

    private AudioSource audio;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    private void Awake()
    {
        base.maxBullet = 30;
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
    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public IEnumerator ShootRifle()
    {
        //if (!isAiming)
        //{
        //    shotOffset = new Vector3(Random.Range(muzzle.transform.forward.x - 0.005f, muzzle.transform.forward.x + 0.005f), Random.Range(muzzle.transform.forward.y - 0.005f, muzzle.transform.forward.y + 0.005f), 0f);
        //}
        //else if (isAiming)
        //{
        //    shotOffset = new Vector3(0f, 0f, 0f);
        //}
        audio.clip = shootSound;
        audio.PlayOneShot(audio.clip);
        damage.AmmoCount = damage.AmmoCount - 3;
        for (int i = 0; i<3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Rigidbody clone1 = Instantiate(bullets, hipShot.transform.position, muzzle.transform.rotation);
            clone1.AddForce(hipShot.transform.forward * bulletSpeed, ForceMode.Impulse);
            if (isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0f));
            if (!isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0.04f));
            StartCoroutine(clone1.GetComponent<DamageDone>().BreakDistance());
        }
        if (damage.AmmoCount <= 0) base.noAmmo = true;

        //yield return new WaitForSeconds(0.1f);
        //Rigidbody clone2 = Instantiate(bullets, hipShot.transform.position, muzzle.transform.rotation);
        //clone2.AddForce(hipShot.transform.forward * bulletSpeed, ForceMode.Impulse);
        //if (isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0f));
        //if (!isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0.04f));
        //StartCoroutine(clone2.GetComponent<DamageDone>().BreakDistance());

        //yield return new WaitForSeconds(0.1f);
        //Rigidbody clone3 = Instantiate(bullets, hipShot.transform.position, muzzle.transform.rotation);
        //clone3.AddForce(hipShot.transform.forward * bulletSpeed, ForceMode.Impulse);
        //if (isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0f));
        //if (!isAiming) StartCoroutine(ActivateRenderBullet(clone1, 0.04f));
        //StartCoroutine(clone3.GetComponent<DamageDone>().BreakDistance());
    }
    void Update()
    {
        DisplayUI();
        if (control.AimDownSightsInput)
        {
            control.AimDownSightsInput = false;
            base.AimDownSight();
        }
        if (control.FireInput && attackOnce && damage.AmmoCount > 0 && !isReloading)
        {
            control.FireInput = false;
            StartCoroutine(ShootRifle());
            base.Attacking("Shoot", resetTimeShot);
            base.currAmmo = damage.AmmoCount;
        }
        if((control.ReloadInput || damage.AmmoCount <= 0) && damage.AmmoCount != maxBullet)
        {
            control.ReloadInput = false;
            base.Reloading("ArIsAim");
            audio.clip = reloadSound;
            audio.PlayOneShot(audio.clip);
            if (!isAiming)
            {
                damage.AmmoCount = maxBullet;
                base.currAmmo = damage.AmmoCount;
            }
        }
    }
}
