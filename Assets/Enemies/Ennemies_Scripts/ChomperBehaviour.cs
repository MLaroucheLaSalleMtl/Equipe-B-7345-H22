using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private bool isDestChange = false;
    [SerializeField] private CapsuleCollider meleeAttackColl;

    //run for special behaviour
    private Vector3 nextRunDest = new Vector3();
    private bool needtomove = false;
    
    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;

    private void Awake()
    {
        this.GetComponent();
        this.GetStats();
        //set collider 
        this.meleeAttackColl.enabled = false;
        this.meleeAttackColl.isTrigger = true;
        //Physics.IgnoreCollision(this.GetComponent<BoxCollider>(), myTarget.GetComponent<CapsuleCollider>());
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
            print("switch");
            this.SpecialMove();
        }
        //when chassing player
        if (playerFound && !canAttack && !needtomove)
        {
            print("chasse");
            base.EnemieChassing();
        }
        //when melee attack
        if (canAttack && playerFound && !needtomove)
        {
            print("attack");
            base.MeleeAttack("attack");
        }
        //when Patrolling
        if (!playerFound)
        {
            print("Patroll");
            base.EnemieWalk();
        }
    }
    #region Animation event
    protected void AttackBegin()
    {
        this.meleeAttackColl.enabled = true;
    }
    protected void AttackEnd()
    {
        this.meleeAttackColl.enabled = false;
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

    
    //animation event
    public override void AttackCompleted()
    {
        base.MovingBehaviour();
        this.needtomove = base.RandomValue(0, 15) < 3;
        anim.ResetTrigger("attack");
            Invoke(nameof(base.ResetAttack), 1f);
    }

    
    //to test ----
    private bool validpath(Vector3 path)
    {
        NavMeshPath navPath = new NavMeshPath();
        return base.agent.CalculatePath(path, navPath);

        
    }
    //** need to rework stat modifier on enemies 
    protected override void SpecialMove()
    {
       
        if (!playerFound)
        {
            this.needtomove = false;
            this.isDestChange = false;
        }
        //asign a single new destination
         if (!isDestChange)
         {
            MovingBehaviour();
            nextRunDest = (transform.position +( new Vector3 (base.myTarget.transform.position.x - transform.position.x  , 0,
                         base.myTarget.transform.position.z - transform.position.z).normalized * -4.5f)); // add a variable for the value ** add random to move varius behaviour from single behaviour
                //print(nextRunDest+ "  next dest switch");
            base.enemieRange = 20f;
            //base.agent.speed = enemieSpeed * 1.5f;
            isDestChange = true;
         }
       
         if (Vector3.Distance(nextRunDest, transform.position) < 1.5f /*|| agent.velocity.magnitude < 0.01f*/ )
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

    }

    
}