using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool attackOnce = true;
    protected float attackRange;
    [SerializeField] protected GameObject player;

    public void Attacking(string animName,float resetShotTime)
    {
        if (attackOnce)
        {
            anim.SetBool(animName, true);
            attackOnce = false;
            //DoDamage();
            StartCoroutine(ResetAttack(animName, resetShotTime));
        }
    }
    public IEnumerator ResetAttack(string animName, float resetShotTime)
    {
        yield return new WaitForSeconds(resetShotTime);
        anim.SetBool(animName, false);
        attackOnce = true;
    }

    //public void DoDamage(); //TODO

    public void Update()
    {
        
    }
}
