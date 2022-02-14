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
    [SerializeField] private Camera AimCamera;

    public void Attacking(string animName,float resetShotTime)
    {
        if (attackOnce)
        {
            anim.SetBool(animName, true);
            //DoDamage();
            attackOnce = false;
            StartCoroutine(ResetAttack(animName, resetShotTime));
        }
    }

    public void AimDownSight(bool isSniper)
    {
        isAiming = !isAiming;
        if(isAiming)
        {
            anim.SetBool("Aiming", true);
            if(!IsInvoking("AimDownSight") && isSniper) AimCamera.gameObject.SetActive(true);
        }
        else if(!isAiming)
        {
            anim.SetBool("Aiming", false);
            if(isSniper) AimCamera.gameObject.SetActive(false);

        }
    }
    public IEnumerator ResetAttack(string animName, float resetShotTime)
    {
        yield return new WaitForSeconds(resetShotTime);
        anim.SetBool(animName, false);
        attackOnce = true;
    }

    //public void DoDamage(); //TODO

}
