using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//main class to herite for enemie gameobject
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemie : MonoBehaviour
{
    
    [SerializeField] protected Scriptable_Stats_Enemies enemie_stats;
    [SerializeField] protected LayerMask mask;
    protected new string name ;
    protected int healthPoints;
    protected int defensePoints;
    protected int attackPower;
    protected Vector3 target;
    [SerializeField][Range(1f,75f)] protected float detectionRange = 10f;

    


    //use in update to show stat of the ennemie
    protected void GetStats()
    {
        this.name = enemie_stats.name;
        this.attackPower = enemie_stats.AttackPower;
        this.healthPoints = enemie_stats.HealthPoints;
        this.defensePoints = enemie_stats.DefensePoints;
        //this.target = GetComponent<Player>(); ** follow is position
    }
    public abstract void SaveStats();
    public abstract void EnemieMovement();
    public abstract void EnemieWalk();
    public abstract void EnemieAnimation();
   
    
    

        
    
    private void ResetHealth()
    {
        this.healthPoints = enemie_stats.HealthPoints;
    }

}
