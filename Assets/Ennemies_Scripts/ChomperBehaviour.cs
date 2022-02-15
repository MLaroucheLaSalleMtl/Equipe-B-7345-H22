using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private bool isDestChange = false;
    private bool attackDone = false;
    //patroll behaviour
    private bool walkDestinationSet = false;
    private Vector3 nextWalkDest;
    
    //run second behaviour
    private Vector3 nextRunDest;
    private bool needtomove = false;
    
    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;
   
    private void FixedUpdate()
    {
        //animation with rootMotion
        base.EnemieAnimation();
       
    }

    private void Update()
    {
         this.playerFound = base.PlayerDetected();
        this.canAttack = base.InAttackRange();

        //when update position before attack
        if (playerFound && needtomove)
        {
            print("switch");
            SwitchDestinationForAttack();
        }
        //when chassing player
        if (playerFound && !canAttack && !needtomove)
        {
            print("chasse");
            EnemieChassing();
        }
        //when melee attack
        if (canAttack && playerFound && !needtomove)
        {
            print("attack");
            MeleeAttack();
        }
        //when Patrolling
        if (!playerFound)
        {
            print("Patroll");
            EnemieWalk();
        }
    }
    private void AgentStatBehaviour(float speedValue, float EnemieRange)
    {
        base.agent.speed = speedValue;
        base.EnemieRange = EnemieRange;
    }

    private void LookAtPlayer(Vector3 target) // maybe remove it
    {
        if (base.PlayerDetected())
        {
            var rotation = Quaternion.LookRotation(target- transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 50f);
        }
    }

    //set agent destionation with vector3
    private void AgentDestination(Vector3 nextPath)
    {
        base.agent.SetDestination(nextPath);
    }
   
    private void EnemieChassing()
    {
        if (base.agent.speed != 6f && base.EnemieRange != 8f) //changing value for chassing
            AgentStatBehaviour(6, 8);


        MovingBehaviour();
        AgentDestination(base.myTarget.transform.position); //apply movement
    }

    protected void MovingBehaviour()
    {
        if (base.obstacle.enabled != false && base.agent.enabled != true)
        {

            base.obstacle.enabled = false;
            base.agent.enabled = true;
        }
    }
    
    private void MeleeAttack()
    {
        AgentDestination(this.transform.position); // stop player from moving

        if (!this.attackDone)
        {
            var lookAtTarget = new Vector3(base.myTarget.transform.position.x, this.transform.position.y, base.myTarget.transform.position.z);
            transform.LookAt(lookAtTarget); // be sure to look the player direction
            //anim.SetBool("isAttack", true);
            anim.SetTrigger("attack"); // set my attack
            base.agent.enabled = false;
            base.obstacle.enabled = true;
            this.attackDone = true;// wait Invoke for attack again
        }
    }
    //animation event
    public void AttackCompleted()
    {
        this.needtomove = base.RandomValue(0, 11) < 5;
        print(this.needtomove);
        anim.ResetTrigger("attack");
        if (!IsInvoking(nameof(ResetAttack)))
            Invoke(nameof(ResetAttack), 1f);
    }

    private void ResetAttack()
    {
        this.attackDone = false;
    }

    private void SwitchDestinationForAttack()
    {
        if (!playerFound)
            this.needtomove = false;
        //asign a single new destination
        if (!isDestChange)
        {
            MovingBehaviour();
            nextRunDest = (transform.position +( new Vector3 (base.myTarget.transform.position.x - transform.position.x  , 0,
                         base.myTarget.transform.position.z - transform.position.z).normalized * -4.5f)); // add a variable for the value
            print(nextRunDest+ "  next dest switch");
            base.EnemieRange = 20f;
            AgentDestination(nextRunDest);
            //base.agent.speed = enemieSpeed * 1.5f;
            isDestChange = true;
        }
        Vector3 distanceLeft = nextRunDest - transform.position  ;
        print(distanceLeft.magnitude + " magnitude");
        //Vector3 distanceLeft = new Vector3(transform.position.x - nextRunDest.x, transform.position.y, transform.position.z - nextRunDest.z);  
        print(distanceLeft);
        if (distanceLeft.magnitude < 1f /*|| agent.velocity.magnitude < 0.01f*/ )
        {
            print("dest done");
            needtomove = false;
            isDestChange = false;
        }
    }
   
   private Vector3 RandomEnemieDestionation(float minValue, float maxValue , Vector3 target)
    {
        
        return  new Vector3(Random.Range(target.x - minValue, target.x + maxValue),
               target.y, Random.Range(target.z - minValue, target.z + maxValue));
    }
    public override void EnemieWalk()
    {
        if (!walkDestinationSet)
        {
            if (base.agent.speed != 3f && base.EnemieRange != 10f)
                AgentStatBehaviour(3, 10);
            MovingBehaviour();
            print("changed");
            nextWalkDest = RandomEnemieDestionation(15f, 15f,transform.position);
            AgentDestination(nextWalkDest);
            walkDestinationSet = true;
        }
        Vector3 distanceLeft = nextWalkDest - transform.position ;// calculating the diff between my actual pos and next dest
        print(distanceLeft.magnitude + " mag");
        if (distanceLeft.magnitude < 1f || agent.velocity.magnitude < 0.01f) //check if my actualPos is far to my nextDest
            walkDestinationSet = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.PlayerDetected() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.EnemieRange);

    }
}
