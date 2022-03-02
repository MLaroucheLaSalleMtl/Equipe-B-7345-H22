using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private bool isDestChange;
    [SerializeField] private CapsuleCollider meleeAttackColl;

    //run for special behaviour
    private Vector3 nextRunDest;
    private bool needtomove;
    
    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;

    private void Awake()
    {
        this.GetComponent();
        base.GetStats();
        //set collider 
        this.meleeAttackColl.enabled = false;
        this.meleeAttackColl.isTrigger = true;
        //set bool
        this.isDestChange = false;
        this.needtomove = false;
        //position
        this.nextRunDest = new Vector3();

    }

    private void FixedUpdate()
    {
        //animation with rootMotion
        base.EnemieAnimation();
       
    }
    
    
    private void Update()
    {
        this.playerFound = base.PlayerDetected();
        this.canAttack = base.InMeleeAttackRange();

        //when update position before attack
        if (playerFound && needtomove)
        {
            //print("switch");
            this.SpecialMove();
        }
        //when chassing player
        if (playerFound && !canAttack && !needtomove)
        {
            //print("chasse");
            base.EnemieChassing();
        }
        //when melee attack
        if (canAttack && playerFound && !needtomove)
        {
            //print("attack");
            base.MeleeAttack("attack");
        }
        //when Patrolling
        if (!playerFound)
        {
            //print("Patroll");
            base.EnemieWalk();
        }
    }
    #region Animation event
    public void AttackBegin()
    {
        this.meleeAttackColl.enabled = true;
    }
    public void AttackEnd()
    {
        this.meleeAttackColl.enabled = false;
    }
    public override void AttackCompleted()
    {
        StartCoroutine(base.ChangeBehaviour());
        //base.MovingBehaviour();
        this.needtomove = base.RandomValue(0, 15) < 3;
        anim.ResetTrigger("attack");
        Invoke(nameof(base.ResetAttack), 1f);
    }

    #endregion


    private void LookAtPlayer(Vector3 target) // maybe remove it
    {
        if (base.PlayerDetected())
        {
            var rotation = Quaternion.LookRotation(target- transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 50f);
        }
    }

    
    //** need to rework stat modifier on enemies 
    protected override void SpecialMove()
    {
       
        if (!playerFound  )
        {
            this.needtomove = false;
            this.isDestChange = false;
        }
        //asign a single new destination
         if (!isDestChange)
         {
            StartCoroutine(base.ChangeBehaviour());
            //MovingBehaviour();

            nextRunDest = (transform.position + (new Vector3(base.myTarget.transform.position.x - transform.position.x, 0,
                        base.myTarget.transform.position.z - transform.position.z).normalized * -4.5f));

            //if (!base.IsValidPath(nextRunDest))
            //{
            //    this.needtomove = false;
            //    this.isDestChange = false;
            //}
            //base.enemieRange = 20f;
            //base.agent.speed = enemieSpeed * 1.5f;
            isDestChange = true;
         }
       
         if (Vector3.Distance(nextRunDest, transform.position) < 1.5f )
         {
            needtomove = false;
            isDestChange = false;
         }
        AgentDestination(nextRunDest);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.PlayerDetected() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.enemieRange);
        Gizmos.DrawRay(new Vector3 (transform.position.x, 0.9f, transform.position.z), Vector3.forward * 10);
    }

    
}
