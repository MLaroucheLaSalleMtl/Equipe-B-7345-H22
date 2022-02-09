using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    protected bool attackOnce = true;
    protected float attackRange;
    //steven add this **
    protected int attackDamgage;

    [SerializeField] protected GameObject player;

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
    public IEnumerator ResetAttack(string animName, float resetShotTime)
    {
        yield return new WaitForSeconds(resetShotTime);
        anim.SetBool(animName, false);
        attackOnce = true;
    }
    //steven add this **

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemie>())
        {
            other.gameObject.GetComponent<Enemie>().ReceiveDamage(this.attackDamgage);
        }
    }

    //public void DoDamage(); //TODO

}
