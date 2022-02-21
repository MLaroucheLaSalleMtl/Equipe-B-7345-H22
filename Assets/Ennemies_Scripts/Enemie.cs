using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//main class to herite for enemie gameobject
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemie : MonoBehaviour
{
    // behaviour value
    [SerializeField] private Scriptable_Stats_Enemies enemie_stats;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsBullet;
    [SerializeField] protected float EnemieRange = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    // player gameobject position
    [SerializeField] protected GameObject myTarget;
    // for melee attack
    
    // patroll variable
    private bool walkDestinationSet;
    private Vector3 nextWalkDest;
    protected bool attackDone = false;


    //EnemieStats
    protected new string name ;
    protected int healthPoints;
    private int maxHealthPoints;
    protected int defensePoints;
    protected int attackPower;
   //Essential Components
    protected Animator anim;
    protected NavMeshAgent agent;
    protected NavMeshObstacle obstacle;
   
    //add force variable
    private float impluseForce;
    private bool powerIncresed = false;

    //Get -- Set  section 
    //------------------------------------------------//
    public int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public int RealDamage { get => InflictDamage(); }

    //abstract methode  section 
    //------------------------------------------------//
    protected abstract void AttackCompleted();
    protected abstract void SpecialMove();

    //Starting stat section 
    //------------------------------------------------//

   
    protected void GetComponent()
    {
       this.anim = GetComponent<Animator>();
       this.agent = GetComponent<NavMeshAgent>();
       this.obstacle = GetComponent<NavMeshObstacle>();
    }

    protected void NaveMeshSetting()
    {
        //agent
        this.agent.speed = 6f;
        this.agent.angularSpeed = 200f;
        this.agent.acceleration = 7f;
        this.agent.stoppingDistance = 0f;
        //obstacle
        this.obstacle.enabled = false;

    }

    //use in update to show stat of the ennemie
    protected void GetStats()
    {
        //set base statistique
        this.name = enemie_stats.Name;
        this.attackPower = this.enemie_stats.AttackPower;
        this.healthPoints = this.enemie_stats.HealthPoints;
        this.defensePoints = this.enemie_stats.DefensePoints;
        this.maxHealthPoints = this.healthPoints;
        
        //set default range 
        this.EnemieRange = this.enemie_stats.DetectionRange;
        this.attackRange = this.enemie_stats.AttackRange;
        //force 
        this.impluseForce = this.enemie_stats.ImpluseForce;
        //
        this.walkDestinationSet = false;
        //initialise player to be able to lacate him 
        if (this.myTarget == null)  //the name must fit with the the scene name
                this.myTarget = GameObject.Find("Player");
    }
    //Animation section 
    //------------------------------------------------//

    protected void EnemieAnimation()
    {
        this.anim.SetFloat("magnitude", this.agent.velocity.magnitude);
        this.anim.SetBool("isPlayer", this.PlayerDetected());
        DeadBehaviour();
    }
    private void DeadBehaviour()
    {
        if (this.healthPoints <= 0)
        {
            this.anim.SetBool("isDead", true);
            this.agent.isStopped = true;
            Destroy(gameObject, 1.5f);
        }
    }

   


    //Physic section 
    //------------------------------------------------//
    protected bool PlayerDetected()
    {
        return Physics.CheckSphere(transform.position, this.EnemieRange, this.whatIsPlayer);
    }
    protected bool InAttackRange()
    {
        return Physics.CheckSphere(transform.position, this.attackRange, this.whatIsPlayer);
    }
    protected bool AttackedByPLayer()
    {
        return Physics.CheckSphere(transform.position, this.attackRange, this.whatIsBullet);
    }
    protected bool BeenHitted()
    {
        return !this.PlayerDetected() && this.AttackedByPLayer();
    }

    protected void ResetHealth()
    {
        if(this.healthPoints <= (int)this.maxHealthPoints* 0.5f)
        {
            var regainChance = Random.Range(0, 100);
            if (regainChance < 10)
            {
                this.healthPoints = enemie_stats.HealthPoints;
                 this.EnrageMode();
            }
        }
    }


    
    protected int RandomValue(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    public int InflictDamage()//will receive player variable to have acces to his hp
    {
        int damage;
        
        if (this.healthPoints == maxHealthPoints)
            damage = RandomValue((int)(this.attackPower * 0.25f), (int)(this.attackPower * 0.75f));
        else if (this.healthPoints < (int)(this.healthPoints * 0.50f))
            damage = RandomValue((int)(this.attackPower * 0.50f), (int)(this.attackPower));
        else
            damage = (int)(this.attackPower * 0.5f) ;
        return damage;
    }

    private void EnrageMode()
    {
        if (!powerIncresed)
        {
            float scaleEmplifer = 1.5f;
            float attackPowerEmplifier = 1.1f;
            //bool chanceToEnrage = this.RandomValue(0, 100) > 90 ? true : false;
            if (this.RandomValue(0, 100) > 90)
            {
                this.transform.localScale *= scaleEmplifer;
                this.attackPower = (int)(this.attackPower * attackPowerEmplifier);
                this.powerIncresed = true;
            }
        }
    }
   
    private float DamageReducer(int damage)
    {
        float scaling = this.defensePoints * 0.01f;
        return scaling * damage;
    }
    public void ReceiveDamage(int Damage)
    {
        if (this.healthPoints > 0)
        {
            int realDamage = (int)(Damage - DamageReducer(Damage));
            print(realDamage);
            this.healthPoints -= Damage;
            ResetHealth(); //if lucky will receive reset health
        }
    }
    //Behaviour section 
    //------------------------------------------------//
    //** weapon must have a force value to be use on ennemi
    public void AdaptiveForce(Collider other)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            var contact = hit.point - transform.position;
            other.gameObject.GetComponent<Rigidbody>().AddForce(contact.normalized * this.impluseForce, ForceMode.Impulse);
        }
        
    }

    protected void ResetAttack()
    {
        this.attackDone = false;
    }


    protected void EnemieChassing()
    {
        if (this.agent.speed != 6f && this.EnemieRange != 8f) //changing value for chassing
            this.AgentStatBehaviour(6, 8);


        //this.MovingBehaviour();
        this.AgentDestination(this.myTarget.transform.position); //apply movement
    }

    protected void MovingBehaviour()
    {
        if (this.obstacle.enabled != false && this.agent.enabled != true)
        {

            this.obstacle.enabled = false;
            this.agent.enabled = true;
        }
    }
    protected void AgentDestination(Vector3 nextPath)
    {
        this.agent.SetDestination(nextPath);
    }

    protected void AgentStatBehaviour(float speedValue, float EnemieRange)
    {
        this.agent.speed = speedValue;
        this.EnemieRange = EnemieRange;
    }

    protected void EnemieWalk()
    {
        if (!walkDestinationSet)
        {
            if (this.agent.speed != 3f && this.EnemieRange != 10f)
                AgentStatBehaviour(3, 10);
            //MovingBehaviour();
            nextWalkDest = RandomEnemieDestionation(15f, 15f);
            AgentDestination(nextWalkDest);
            walkDestinationSet = true;
        }
        Vector3 distanceLeft = nextWalkDest - transform.position;// calculating the diff between my actual pos and next dest
        print(distanceLeft.magnitude + " mag");
        if (distanceLeft.magnitude < 1f || agent.velocity.magnitude < 0.01f) //check if my actualPos is far to my nextDest
            walkDestinationSet = false;
    }

    private Vector3 RandomEnemieDestionation(float minValue, float maxValue)
    {
        return new Vector3(Random.Range(transform.position.x - minValue, transform.position.x + maxValue),
               transform.position.y, Random.Range(transform.position.z - minValue, transform.position.z + maxValue));
    }

    protected void MeleeAttack(string attackName)
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

}
