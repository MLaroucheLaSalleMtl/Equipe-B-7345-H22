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
    //playerStat
    protected new string name ;
    protected int healthPoints;
    private int maxHealthPoints;
    protected int defensePoints;
    protected int attackPower;

    protected Animator anim;
    protected NavMeshAgent agent;
    //protected SphereCollider sColl;
    protected bool haveAttacked;
   
    [SerializeField]private CapsuleCollider meleeAttackColl; // for melee attack
    private BoxCollider bodycoll;// player cant pass through 

    //add force variable
    private float impluseForce;
    private bool powerIncresed = false;

    public int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public int RealDamage { get => InflictDamage(); }

    protected void GetComponent()
    {
       this.anim = GetComponent<Animator>();
       this.agent = GetComponent<NavMeshAgent>();
       this.bodycoll = GetComponent<BoxCollider>();
    }

    protected void SetMyAgent()
    {
        //default value when start game
    }

    //use in update to show stat of the ennemie
    protected void GetStats()
    {
        //set base statistique
        this.name = enemie_stats.name;
        this.attackPower = this.enemie_stats.AttackPower;
        this.healthPoints = this.enemie_stats.HealthPoints;
        this.defensePoints = this.enemie_stats.DefensePoints;
        this.maxHealthPoints = this.healthPoints;
        //set collider 
        this.meleeAttackColl.enabled = false;
        this.meleeAttackColl.isTrigger = true;
        //set default range 
        this.EnemieRange = this.enemie_stats.DetectionRange;
        this.attackRange = this.enemie_stats.AttackRange;
        this.impluseForce = this.enemie_stats.ImpluseForce;
        this.haveAttacked = false;
    }
    
   
    public abstract void EnemieWalk();
    protected void EnemieAnimation()
    {
        this.anim.SetFloat("axisX", this.agent.velocity.magnitude);
        this.anim.SetBool("isDead", this.healthPoints <= 0);
        this.anim.SetBool("isPlayer", this.PlayerDetected());

        //if (this.InAttackRange())
        //    anim.SetBool("isAttack", !this.haveAttacked);
    }
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
        if(this.healthPoints <= 25)
        {
            var regainChance = Random.Range(0, 100);
             if (regainChance < 10)
             {
                 this.healthPoints = enemie_stats.HealthPoints;
                 this.EnrageMode();
             }
        }
    }


    #region Animation event
    public void AttackBegin()
    {
        this.meleeAttackColl.enabled = true;
        this.bodycoll.enabled = false;

    }
    public void AttackEnd()
    {
        this.meleeAttackColl.enabled = false;
        this.bodycoll.enabled = true;

    }
    public void EnableCollision()
    {
        bodycoll.enabled = true;
    }
    public void DisableCollision()
    {
        bodycoll.enabled = false ;
    }
    #endregion
    protected int RandomValue(int min, int max)
    {
        return Random.Range(min, max);
    }

    private int InflictDamage()//will receive player variable to have acces to his hp
    {
        int damage;
        
        if (this.healthPoints == maxHealthPoints)
            damage = RandomValue((int)(this.attackPower * 0.25f), (int)(this.attackPower * 0.75f));
        else if (this.healthPoints < (int)(this.healthPoints * 0.50f))
            damage = RandomValue((int)(this.attackPower * 0.50f), (int)(this.attackPower + 1));
        else
            damage = (int)(this.attackPower * 0.5f) ;
        return damage;
    }

    protected void DeadBehaviour()
    {
        if (this.healthPoints <= 0)
        {
            this.agent.destination = transform.position ;
            Destroy(gameObject, 2f);
        }
    }
    private void EnrageMode()
    {
        if (!powerIncresed)
        {
            float scaleEmplifer = 1.5f;
            float attackPowerEmplifier = 1.1f;
            //bool chanceToEnrage = this.RandomValue(0, 100) > 90 ? true : false;
            if (this.RandomValue(0, 100) > 90 ? true : false)
            {
                this.transform.localScale *= scaleEmplifer;
                this.attackPower = (int)(this.attackPower * attackPowerEmplifier);
                this.powerIncresed = true;
            }
        }

    }
    //apply damage in fonction of the defensePoints
    private float DamageReducer()
    {
        float reducer = 0.00f;
        switch (this.defensePoints)
        {
            case 10:
                reducer = 0.9f;
                break;
            case 20:
                reducer = 0.8f;
                break;
            case 30:
                reducer = 0.7f;
                break;
            case 40:
                reducer = 0.6f;
                break;
        }
        if (reducer == 0.00f)
            print("Invalide defensePoint must be initialise with {10,20,30,40}");

        return reducer;
    }
    public void ReceiveDamage(int weaponDamage)
    {
        if (this.healthPoints > 0)
        {
            int realDamage = (int)(weaponDamage * DamageReducer());
            print(realDamage);
            this.healthPoints -= realDamage;
            ResetHealth();
        }
    }
    public void AdaptiveForce(Collider other)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            var contact = hit.point - transform.position;
            other.gameObject.GetComponentInParent<Rigidbody>().AddForce(contact.normalized * this.impluseForce, ForceMode.Impulse);
        }
        
    }
    

}
