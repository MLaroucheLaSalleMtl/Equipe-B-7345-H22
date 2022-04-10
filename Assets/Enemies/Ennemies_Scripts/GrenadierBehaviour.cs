using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadierBehaviour : Enemie
{
    //lazerBeam const value
    [SerializeField] private Projectile_Stats projetiles;
    private float lazerMaxRange;
    private float lazerMinRange;
    private float lazerImpulseForce;
    private float lazerDamage;
    private float lazerResetTime;

    private bool canLazer;
    private bool isLazerCooldown;

    private Vector3 nextRayPos = Vector3.zero;
    private Vector3 tempPlayerPos;
    [Header("Lazer")]
    [SerializeField] private LineRenderer lazerPrefab; // ** WARNING linerenderer must be at Vector(0,0,0)
    [SerializeField] private Transform lazerStartPos;
    [SerializeField] private AudioClip lazerSound;

    //melee Behaviour
    private string[] meleeAnim;
    private int animValue;
    //when facing enemie righthand is at left and lefthand is at the right
    [SerializeField] private CapsuleCollider[] handColls; // left is [0] and righ is [1]

    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canMeleeAttack;
    private void Awake()
    {
        this.GetComponent();
        base.GetStats();
        this.SetMeleeAnim();
        this.SetMeleeColl();
        this.LazerStats();
        //save start pos for respawn
        base.startpos = transform.position;
    }
    private  void LazerStats()
    {
        this.lazerMaxRange = this.projetiles.LazerMaxRange;
        this.lazerMinRange = this.projetiles.LazerMinRange;
        this.lazerImpulseForce = this.projetiles.LazerImpulseForce;
        this.lazerDamage = this.projetiles.LazerDamage;
        this.lazerResetTime = this.projetiles.LazerResetTime;
        this.canLazer = false;
    }

    protected override void EnemieAnimation()
    {
        base.EnemieAnimation();
        base.anim.SetBool("rAttack", canLazer);
    }
    private void FixedUpdate()
    {
        //animation with rootMotion
        this.EnemieAnimation();
    }
    

    
    #region LazerAnimEvent

    public void LockTarget()
    {
        this.tempPlayerPos = base.myTarget.transform.position;
        base.LookAtTarget();
    }
    public void EnableBeam()
    {
        base.SetSound(this.lazerSound);
        base.AdaptiveForce(lazerMaxRange,lazerImpulseForce,true); // lazer impulsion
        VisualLazerBeam();
    }
    public void DisableBeam()
    {
        this.canLazer = false;
        this.isLazerCooldown = true;
        if (!IsInvoking(nameof(LazerOnCooldown)))
            Invoke(nameof(LazerOnCooldown), lazerResetTime); 
    }
    private void LazerOnCooldown()
    {
        this.isLazerCooldown = false;
    }
    #endregion

    private void VisualLazerBeam()
    {
        var myLazer = Instantiate(lazerPrefab);
        myLazer.SetPosition(0, lazerStartPos.position);
        myLazer.SetPosition(1, this.tempPlayerPos);
        Destroy(myLazer.gameObject , 0.50f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = base.PlayerDetected() ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, base.enemieRange);
    }
    
    private bool CanUseLazer()
    {
        float actualDistance =  Vector3.Distance(transform.position, base.myTarget.transform.position);
        if (actualDistance > lazerMinRange && actualDistance <= lazerMaxRange && !isLazerCooldown)
        {
            base.AgentDestination(this.transform.position);
            if (!base.isPlayed && base.healthPoints > 0)
            {
                base.SetSound(this.breathingSound);
                base.isPlayed = true;
            }
            return true;
        }
         
        return false;
    }

    private void Update()
    {
        //physic
        this.playerFound = base.PlayerDetected();
        this.canMeleeAttack = base.InMeleeAttackRange();


        if(CanUseLazer() && base.PlayerDetected())
            this.canLazer = true;
        //when chassing player
        if (playerFound && !canMeleeAttack && !CanUseLazer())
        {
            //print("chasse");
            base.EnemieChassing();
        }
        ////when melee attack
        if (canMeleeAttack && playerFound && !CanUseLazer())
        {
            //print("attack");
            RandomMeleeAttack();
            base.MeleeAttack(this.meleeAnim[animValue]);
        }
        ////when Patrolling
        if (!playerFound)
        {
            //print("Patroll");
            base.EnemieWalk();
        }
    }
    
    private void RandomMeleeAttack()
    {
        if (!base.attackDone)
        {
            animValue = base.RandomValue(0, 1);
        }
    }
    private void SetMeleeAnim()
    {
        this.meleeAnim = new string[2];
        this.meleeAnim[0] = "mAttack1";
        this.meleeAnim[1] = "mAttack2";
    }
    private void SetMeleeColl()
    {
        this.handColls[0].isTrigger = true; 
        this.handColls[0].enabled = false;
        this.handColls[1].isTrigger = true;
        this.handColls[1].enabled = false;
    }
    #region Animation melee event
    public void StartAttack()
    {
        this.handColls[this.animValue].enabled = true;
        base.SetSound(base.meleeAttackSound);
    }
    public void EndAttack()
    {
        this.handColls[this.animValue].enabled = false;
    }
    public override void AttackCompleted()
    {
        base.anim.ResetTrigger(this.meleeAnim[this.animValue]);
        //this.MovingBehaviour();
        StartCoroutine(base.ChangeBehaviour());
        Invoke(nameof(base.ResetAttack), 1f);
    }
    #endregion
    protected override void SpecialMove()
    {
        throw new System.NotImplementedException();
    }


   
}
