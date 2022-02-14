using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    protected bool attackOnce = true;
    protected bool isAiming = false;
    protected float attackRange;
    [SerializeField] protected GameObject player;
    protected int maxBullet;
    protected bool noAmmo;

    public void Attacking(string animName,float resetShotTime)
    {
        if (attackOnce)
        {
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

    }

    public void ResetReload()
    {
        anim.SetBool("Reload", false);
        noAmmo = false;
    }

    public void AimDownSight()
    {
        isAiming = !isAiming;
        if(isAiming)
        {
            anim.SetBool("Aiming", true);
        }
        else if(!isAiming)
        {
            anim.SetBool("Aiming", false);
        }
    }
    public IEnumerator ResetAttack(string animName, float resetShotTime)
    {
        yield return new WaitForSeconds(resetShotTime);
        anim.SetBool(animName, false);
        attackOnce = true;
    }

    public void CheckAmmo()
    {
        if (noAmmo && anim.GetBool("Aiming") == true)
        {
            AimDownSight();
        }
    }
}
