using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private float rangeAttack = 2.50f;
    private float enemieSpeed = 5f;
    private Vector3 newDestination = Vector3.zero;
    //private bool attackDone = false;
    private float rotSpeed = 10.0f;
    private bool isAttacking = false;
    //private bool behaviourFinish = false;
    private int hp;
    private bool isDestChange = false;
    private bool isAgentStoped = false;
    private GameObject playerPos;

    private bool playerFound;
    private bool canAttack;
    void Start()
    {
        base.GetComponent();
        SetEnemie();
        playerPos = GameObject.Find("player__test");
    }
    private void FixedUpdate()
    {
        base.EnemieAnimation();
    }
    private void Update()
    {
        playerFound = base.PlayerDetected();
        canAttack = base.InAttackRange();
        LookAtPlayer(playerPos.transform.position);
        // print("have attack = " + base.haveAttacked);

        //if (base.PlayerDetected() && base.haveAttacked)
        //{
        //    print("Switch destination");
        //    SwitchDestinationForAttack();
        //}

        if (this.healthPoints <= 0)
        {
            base.DeadBehaviour();
        }

        else if (playerFound && !canAttack)
        {
            //print("I follow you");
            EnemieFollow(playerPos.transform.position);

        }

        else if (canAttack && playerFound)
        {

            print("melee attack");
            MeleeAttack();
        }


        else if (!playerFound)
            EnemieWalk();

    }

    private void OnCollisionEnter(Collision collision)
    {
        var coll = collision.gameObject;
        if (coll.CompareTag("Player"))
        {
            //coll.GetComponent<Rigidbody>().AddForce(new Vector3(1, 1, 1), ForceMode.Impulse);
        }
        //else if (coll.CompareTag("PlayerTester"))
        //{
        //    agent.isStopped = true;
        //    anim.SetTrigger("isHit");
        //    StartCoroutine(AutorizePath());
        //}
    }
    

    private IEnumerator AutorizePath()
    {
        yield return new WaitForSeconds(1f);
        base.agent.isStopped = false;
    }
    private void SetEnemie()
    {
        base.GetStats();
    }


    private void LookAtPlayer(Vector3 target)
    {
        if (base.PlayerDetected() && !isAttacking)
        {
         var rotation =   Quaternion.LookRotation(target- transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
        }
    }

    //set agent destionation with vector3
    private void AgentDestination(Vector3 nextPath)
    {
        base.agent.SetDestination(nextPath);
    }

    private void RunEnemieBehaviour()
    {
        if (base.agent.speed != enemieSpeed)
            base.agent.speed = enemieSpeed;
        if (base.EnemieRange != 10f)
            base.EnemieRange = 10f;
        //if(base.sColl.radius != base.EnemieRange)
        //base.sColl.radius = base.EnemieRange;
    }
    private void EnemieFollow(Vector3 coll)
    {
        RunEnemieBehaviour();
        AgentDestination(coll);
    }
    private void MeleeAttack()
    {
        

        if (!base.haveAttacked)
        {
            this.isAttacking = true; // this is for apply root motion on animation
            AgentDestination(transform.position); //stop the agent from moving
            base.agent.isStopped = true; //stop his path
            //anim.SetBool("isAttack", !this.haveAttacked);
            anim.SetTrigger("attack");
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("ChomperAttack1")) // when anim finish
            {
                base.haveAttacked = true;

                anim.ResetTrigger("attack");
                if (!IsInvoking(nameof(ResetMovement)))
                    Invoke(nameof(ResetMovement), 0.5f);

                if (!IsInvoking(nameof(AutorizeToAttack)))
                    Invoke(nameof(AutorizeToAttack), 1.25f);
            }
        }
    }

    private void ResetMovement()
    {
       
        this.isAttacking = false;
        base.agent.isStopped = false;
    }

    private void AutorizeToAttack()
    {
        base.haveAttacked = false;
    }

    private void SwitchDestinationForAttack()
    {
        //asign a single new destination
        if (!isDestChange)
        {
            newDestination = RandomEnemieDestionation(10f, 10f);
            AgentDestination(newDestination);
            //base.agent.speed = enemieSpeed * 1.5f;
            isDestChange = true;
        }

        //this mean that the enemie find his new destination and his ready to jump on enemie again
        if (transform.position == newDestination || agent.velocity.magnitude < 1f )
        {
            isDestChange = false;

            if (!IsInvoking(nameof(AutorizeToAttack)))
                Invoke(nameof(AutorizeToAttack), 1f);
            
        }
    }
   
    //use to see if the enemie is in range for attacking
    private bool PlayerDistanceDiff(Vector3 playerPos)
    {
        var playerPosX = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(playerPos.x));
        var playerPosZ = Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(playerPos.z));

       // print(Mathf.Abs(playerPosX)); print(Mathf.Abs(playerPosZ));
        return (playerPosX <= rangeAttack && playerPosZ <= rangeAttack);

    }
   private Vector3 RandomEnemieDestionation(float minValue, float maxValue)
    {
        var enemiePos = transform.position;
        return  new Vector3(Random.Range(enemiePos.x - minValue, enemiePos.x + maxValue),
               enemiePos.y, Random.Range(enemiePos.z - minValue, enemiePos.z + maxValue));


    }
    public override void EnemieWalk()
    {
            if(base.agent.velocity.magnitude < 0.1f)
            {
                base.agent.speed = enemieSpeed * 0.5f;
                base.EnemieRange = 12f;
                AgentDestination(RandomEnemieDestionation(15f,15f));
            }
        
    }

    public override void EnemieMovement()
    {
        throw new System.NotImplementedException();
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.PlayerDetected() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.EnemieRange);

    }
}
