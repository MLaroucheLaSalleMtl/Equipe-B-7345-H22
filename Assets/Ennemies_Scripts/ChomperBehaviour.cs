using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChomperBehaviour : Enemie
{
    private NavMeshAgent agent;
    
    private SphereCollider sColl;
    private Animator anim;
    private float rangeAttack = 2f;
    private bool playerDetection = false;
    private float enemieSpeed = 5f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sColl = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
       
        SetEnemie();
    }
    private void Update()
    {
        EnemieAnimation();
        EnemieWalk();
    }




    //trigger enter with the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            EnemieRun(other.gameObject.transform.position);


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            EnemieRun(other.gameObject.transform.position);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var coll = collision.gameObject;
        if (coll.CompareTag("Player"))
        {
            coll.GetComponent<Rigidbody>().AddForce(new Vector3(2, 10, 3), ForceMode.Impulse);
        }
        else if (coll.CompareTag("PlayerTester"))
        {
            agent.isStopped = true;
            anim.SetTrigger("isHit");
            StartCoroutine(AutorizePath());
            
        }
    }

    IEnumerator AutorizePath()
    {
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
    }
    private void SetEnemie()
    {
        base.GetStats();
        sColl.radius = base.detectionRange;
    }


    //set agent destionation with vector3
    private void AgentDestination(Vector3 nextPath)
    {
        agent.SetDestination(nextPath);

    }


    private void EnemieRun(Vector3 playerPosition)
    {
        agent.speed = enemieSpeed;
        sColl.radius = base.detectionRange;
        AgentDestination(playerPosition);
        playerDetection = true;
        if (PlayerDistanceDiff(playerPosition))
        {
            anim.SetTrigger("attack");
        }
       
    }

    

    private bool PlayerDistanceDiff(Vector3 playerPos)
    {
        var playerPosX = Mathf.Abs(transform.position.x) - Mathf.Abs(playerPos.x);
        var playerPosY = Mathf.Abs(transform.position.z) - Mathf.Abs(playerPos.z);
        //print(playerPosX); print(playerPosY);
        return playerPosX <= rangeAttack && playerPosY <= rangeAttack;

    }


    public override void EnemieAnimation()
    {
        anim.SetFloat("axisX", agent.velocity.magnitude);
        anim.SetBool("isDead",base.healthPoints <= 0);
        anim.SetBool("isPlayer",playerDetection);
    }

    

    //is ennemy attack or follow his taget(player)
    public override void EnemieWalk()
    {
        if (!playerDetection)
        {
            if(agent.velocity.magnitude < 0.1f)
            {
                agent.speed = enemieSpeed * 0.5f;
                sColl.radius = 15f;
                Vector3 enemiePos = transform.position;
                var randomPath = new Vector3(Random.Range(enemiePos.x - 15f, enemiePos.x + 15f), enemiePos.y, Random.Range(enemiePos.z - 15f, enemiePos.z + 15f));
                AgentDestination(randomPath);
            }
        } 
    }

    public override void EnemieMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void SaveStats()
    {
        throw new System.NotImplementedException();
    }

   
}
