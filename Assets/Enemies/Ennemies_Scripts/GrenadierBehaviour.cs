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
    [SerializeField] private CapsuleCollider[] handColls; // left is [0] and righ is [1]
  
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
    }

    private void Update()
    {
        this.playerFound = base.PlayerDetected();
        this.canAttack = base.InAttackRange();

        
        //when chassing player
        if (playerFound && !canAttack )
        {
            //print("chasse");
            base.EnemieChassing();
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
        AgentDestination(this.transform.position); // stop player from moving

        if (!this.attackDone)
        {
            var lookAtTarget = new Vector3(this.myTarget.transform.position.x, this.transform.position.y, this.myTarget.transform.position.z);
            transform.LookAt(lookAtTarget);
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
        this.handColls[0].isTrigger = true; 
        this.handColls[0].enabled = false;
        this.handColls[1].isTrigger = true;
        this.handColls[1].enabled = false;
    }
    #region Animation event
    public void StartAttack()
    {
        this.handColls[this.animValue].enabled = true;
    }
    public void EndAttack()
    {
        this.handColls[this.animValue].enabled = false;
    }
    public override void AttackCompleted()
    {
        base.anim.ResetTrigger(this.meleeAnim[this.animValue]);
        this.MovingBehaviour();
        Invoke(nameof(base.ResetAttack), 1f);
    }
    #endregion
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
