using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private float enemieSpeed = 5f;
    private bool isAttacking = false;
    private bool isDestChange = false;
    private bool attackDone = false;
    //private GameObject targetPos;
    private bool walkDestinationSet = false;
    private Vector3 nextWalkDest;
    private Vector3 nextRunDest;
    private bool needtomove = false;

    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;

    //obstacle
    public NavMeshObstacle navObj;

    //void Start()
    //{
       
    //    SetEnemie();
        
    //    navObj.enabled = false;
    //}

    private void SetEnemie()
    {
        base.GetComponent();
        base.GetStats();
    }
    private void FixedUpdate()
    {
        //animation with rootMotion
        base.EnemieAnimation();
        this.playerFound = base.PlayerDetected();
        this.canAttack = base.InAttackRange();
    }

    private void Update()
    {


        //when update position before attack
        if (playerFound && needtomove) SwitchDestinationForAttack();
        //when chassing player
        if (playerFound && !canAttack && !needtomove) EnemieChassing();
        //when melee attack
        if (canAttack && playerFound && !needtomove) MeleeAttack();
        //when Patrolling
        if (!playerFound) EnemieWalk();

    }


    private void AgentStatBehaviour(float speedValue, float EnemieRange)
    {
        base.agent.speed = speedValue;
        base.EnemieRange = EnemieRange;
    }

    private void LookAtPlayer(Vector3 target) // maybe remove it
    {
        if (base.PlayerDetected() && !isAttacking)
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
        navObj.enabled = false;
        base.agent.enabled = true;
    }
    
    private void MeleeAttack()
    {
        AgentDestination(this.transform.position); // stop player from moving
        if (!this.attackDone)
        {
            var lookAtTarget = new Vector3(base.myTarget.transform.position.x, this.transform.position.y, base.myTarget.transform.position.z);
            transform.LookAt(lookAtTarget); // be sure to look the player direction
            anim.SetTrigger("attack"); // set my attack
            //anim.PlayInFixedTime("ChomperAttack1");
            base.agent.enabled = false;
            navObj.enabled = true;

            this.attackDone = true;// wait Invoke for attack again
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("ChomperAttack1") )// when anim finish
            {
                anim.ResetTrigger("attack"); // reset trigger anim

            }
            Invoke(nameof(ResetAttack), 0.75f);

        }
    }

    private void ResetAttack()
    {
        this.attackDone = false;
        this.needtomove = base.RandomValue(0, 11) < 5 ? true : false;
    }

    private void SwitchDestinationForAttack()
    {
        //asign a single new destination
        if (!isDestChange)
        {
            MovingBehaviour();
            nextRunDest = transform.position -(new Vector3((base.myTarget.transform.position.x - transform.position.x +0.5f) , 0, (base.myTarget.transform.position.z - transform.position.z -0.5f)) );
            AgentDestination(nextRunDest);
            base.agent.speed = enemieSpeed * 1.5f;
            isDestChange = true;
        }
        Vector3 distanceLeft = transform.position - nextRunDest;

        if (distanceLeft.magnitude < 1f || agent.velocity.magnitude < 0.01f)
        {
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

            nextWalkDest = RandomEnemieDestionation(15f, 15f,transform.position);
            AgentDestination(nextWalkDest);
            walkDestinationSet = true;
        }
        Vector3 distanceLeft = transform.position - nextWalkDest;// calculating the diff between my actual pos and next dest
        if (distanceLeft.magnitude < 1f || agent.velocity.magnitude < 0.01f) //check if my actualPos is far to my nextDest
            walkDestinationSet = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.PlayerDetected() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.EnemieRange);

    }
}
