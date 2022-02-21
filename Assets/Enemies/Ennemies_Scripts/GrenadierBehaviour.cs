using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBehaviour : Enemie
{

    //private bool isDestChange = false;
    //patroll behaviour

    //melee animation
    private string[] meleeAnim;
    private int animValue;
    //when facing enemie righthand is at left and lefthand is at the right
    [SerializeField] private CapsuleCollider rightHand;
    [SerializeField] private CapsuleCollider leftHand;
    //run for special behaviour
    
    

    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;
    private void Awake()
    {
        base.GetComponent();
        base.GetStats();
        this.SetMeleeAnim();
        this.SetMeleeColl();
    }
    
    private void FixedUpdate()
    {
        //animation with rootMotion
        base.EnemieAnimation();
        print(base.agent.destination);
    }

    private void Update()
    {
        this.playerFound = base.PlayerDetected();
        this.canAttack = base.InAttackRange();

        
        //when chassing player
        if (playerFound && !canAttack )
        {
            //print("chasse");
           // base.EnemieChassing();
        }
        //when melee attack
        if (canAttack && playerFound )
        {
            //print("attack");
            //** need a funtion to change value if attacked isnt done
            if (!base.attackDone)
            {
                animValue  = base.RandomValue(0, 1);
                print(animValue);

            }
            this.Meme(this.meleeAnim[animValue]);
        }
        //when Patrolling
        if (!playerFound)
        {
            //print("Patroll");
            base.EnemieWalk();
        }
    }

    private void Meme(string attackName)
    {
       // AgentDestination(this.transform.position); // stop player from moving

        if (!this.attackDone)
        {
            //var lookAtTarget = new Vector3(this.myTarget.transform.position.x, this.transform.position.y, this.myTarget.transform.position.z);
            //transform.LookAt(lookAtTarget);
            anim.SetTrigger(attackName); // set my attack
            this.agent.enabled = false;
            this.obstacle.enabled = true;
            this.attackDone = true;// wait Invoke for attack again
        }
    }

    private void SetMeleeAnim()
    {
        this.meleeAnim = new string[2];
        this.meleeAnim[0] = "mAttack1";
        this.meleeAnim[1] = "mAttack2";
    }
    private void SetMeleeColl()
    {
        this.rightHand.isTrigger = true; 
        this.rightHand.enabled = false;
        this.leftHand.isTrigger = true;
        this.leftHand.enabled = false;
    }
    #region Animation event
    public void StartAttack()
    {
        this.rightHand.enabled = true;
    }
    public void EndAttack()
    {
        this.AttackCompleted();
        MovingBehaviour();
    }
    #endregion


    protected override void AttackCompleted()
    {
        base.anim.ResetTrigger("mAttack1");
       
        Invoke(nameof(base.ResetAttack), 1f);
    }

    protected override void SpecialMove()
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.InAttackRange() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.attackRange);

    }
}
