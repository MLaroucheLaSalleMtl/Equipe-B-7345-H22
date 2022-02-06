using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//main class to herite for enemie gameobject
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemie : MonoBehaviour
{
    
    [SerializeField] private Scriptable_Stats_Enemies enemie_stats;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected new string name ;
    protected int healthPoints;
    protected int defensePoints;
    protected int attackPower;
    protected Animator anim;
    protected NavMeshAgent agent;
    //protected SphereCollider sColl;
    protected bool haveAttacked;
    private int maxHealthPoints;
    public  float attackRange =1.5f;

    [SerializeField][Range(1f,75f)] protected float EnemieRange = 10f;

    
    protected void GetComponent()
    {
       this.anim = GetComponent<Animator>();
       this.agent = GetComponent<NavMeshAgent>();
       //this.sColl = GetComponent<SphereCollider>();
    }

    //use in update to show stat of the ennemie
    protected void GetStats()
    {
        this.name = enemie_stats.name;
        this.attackPower = this.enemie_stats.AttackPower;
        this.healthPoints = this.enemie_stats.HealthPoints;
        this.defensePoints = this.enemie_stats.DefensePoints;
        this.maxHealthPoints = this.healthPoints;
        //this.sColl.radius = this.EnemieRange;
        this.haveAttacked = false;
    }
    public abstract void SaveStats();
    public abstract void EnemieMovement();
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

    protected void ResetHealth()
    {
        if(this.healthPoints <= 15)
        {
            var regainChance = Random.Range(0, 100);
            if(regainChance > 90)
            this.healthPoints = enemie_stats.HealthPoints;
        }
    }

    private int RandomValue(int min, int max)
    {
        return Random.Range(min, max);
    }

    protected void InflictDamage()//will receive player variable to have acces to his hp
    {
        int damage;
        //maybe switch for a switch?
        if (this.healthPoints == maxHealthPoints)
            damage = RandomValue((int)(this.attackPower * 0.25f), (int)(this.attackPower * 0.75f));

        else if (this.healthPoints < (int)(this.healthPoints * 0.50f))
            damage = RandomValue((int)(this.attackPower * 0.50f), (int)(this.attackPower + 1));

        else
            damage = (int)(this.attackPower * 0.5f) ;
       
        //player.hp -= damage

    }

    protected void DeadBehaviour()
    {
        if (this.healthPoints <= 0)
        {
            Destroy(gameObject, 2f);
        }
    }

    protected void ReceiveDamage(Collision collision)
    {
        //apply dmg
        var coll = collision.gameObject;
        if (coll.CompareTag("Weapon"))
        {
            //this.healthPoints -= 
        }
        else if (coll.CompareTag("AssaultBullet"))
        {

        }

        else if (coll.CompareTag("SniperBullet"))
        {

        }

    }

}
