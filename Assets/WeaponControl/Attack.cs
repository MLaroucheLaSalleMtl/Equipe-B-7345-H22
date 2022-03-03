using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Attack : MonoBehaviour
{
    //Weapon Control
    [SerializeField] protected Animator anim;
    public bool attackOnce = true;
    public bool isAiming = false;
    public bool isReloading = false;
    public bool isShooting = false;
    protected Vector3 shotOffset;
    protected Camera player;
    protected int maxBullet;
    protected bool noAmmo;
    protected int currAmmo;
    [SerializeField] protected GameObject fullScope;
    [SerializeField] private GameObject generalCrosshair;
    [SerializeField] protected PlayerController control;

    //UI COntrol
    [SerializeField] private TMP_Text ammoText;

    private void Start()
    {
        player = GetComponentInParent<Camera>();
        attackOnce = true;
    }
    public void Attacking(string animName,float resetShotTime)
    {
        if (attackOnce && !isShooting)
        {
            isShooting = true;
            anim.SetBool(animName, true);
            attackOnce = false;
            StartCoroutine(ResetAttack(animName, resetShotTime));
        }
    }

    public void Reloading(string animAimName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animAimName))
        {
            anim.SetBool("Aiming", false);
            AimDownSight();
        }
        anim.SetBool("Reload", true);
        isReloading = true;
    }

    public void ResetReload()
    {
        anim.SetBool("Reload", false);
        noAmmo = false;
        isReloading = false;
    }

    public void AimDownSight()
    {
        isAiming = !isAiming;
        if(isAiming) anim.SetBool("Aiming", true);
        else if(!isAiming) anim.SetBool("Aiming", false);
    }

    public void ScopeChange() //scope change AR and Pistol
    {
        if (isAiming)
        {
            fullScope.gameObject.SetActive(false);
            generalCrosshair.gameObject.SetActive(false);
        }
        else if (!isAiming)
        {
            fullScope.gameObject.SetActive(true);
            generalCrosshair.gameObject.SetActive(true);
        }
    }
    public IEnumerator ResetAttack(string animName, float resetShotTime)
    {
        yield return new WaitForSeconds(resetShotTime);
        anim.SetBool(animName, false);
        attackOnce = true;
        isShooting = false;
        isReloading = false;
    }

    public void CheckAmmo()
    {
        if (noAmmo && anim.GetBool("Aiming") == true) AimDownSight();
    }

    public void DisplayUI()
    {
        ammoText.text = "Ammo : " + currAmmo + "/" + maxBullet;
    }

    public void IsNotShootingNoMo()
    {
        //isShooting = false;
        //isReloading = false;
    }
}
