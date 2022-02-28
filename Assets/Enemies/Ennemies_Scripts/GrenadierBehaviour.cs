using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadierBehaviour : Enemie
{
    //lazerBeam
    [Header("Lazer Beam")]
    private const float lazerMaxRange = 20f;
    private const float lazerMinRange = 5f;
    private const float lazerPowerForce = 20f;
    private const float lazerDamage = 30f;
    private const int random_Lazer_Ray_Range = 1;
    private const float lazerResetTime = 3f;
   

    //initialise into awake overide parent fonction
    private bool canLazer;
    private bool isLazerCooldown;
    private const float LazerCooldown = 3f; //seconds
    private Vector3 nextRayPos = Vector3.zero;
    private Vector3 tempPlayerPos;
    
    [SerializeField] private  LineRenderer lazerPrefab; // ** WARNING linerenderer must be at Vector(0,0,0)
    [SerializeField] private Transform lazerStartPos;
    [SerializeField] private PlayerStats playerStats;
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
        base.GetComponent();
        base.GetStats();
        this.SetMeleeAnim();
        this.SetMeleeColl();
        //this.lazerPrefab.enabled = false;
        //this.lazerPrefab.gameObject.transform.position = Vector3.zero;
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
    int i = 0;
    //Lazer Beam Behaviour
    private void CanLazerBeam()
    {
        NavMeshHit hit;
        if (!isLazerCooldown)
        {
                if (!base.agent.Raycast(tempPlayerPos, out hit))
                {
                    if (hit.distance > lazerMinRange && hit.distance <= lazerMaxRange /*&& hit.position == tempPlayerPos*/)
                    {
                    i++;
                    print("DAMMMMM" + i );
                        this.playerStats.HealthPoints -= RealDamage;
                    }
                }
        }
    }
    #region LazerAnimEvent

    public void LockTarget()
    {
        this.tempPlayerPos = base.myTarget.transform.position;
    }
    public void EnableBeam()
    {
        base.LookAtTarget();
        CanLazerBeam();
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
    //private Vector3Int IntTargetValuePosition()
    //{
    //    Vector3 targetPosition = base.myTarget.transform.position;
    //    return new Vector3Int((int)(targetPosition.x), (int)(targetPosition.y), (int)(targetPosition.z));
    //}
    //private void NextRayCastValue()
    //{
    //    Vector3Int target = IntTargetValuePosition();
    //    this.nextRayPos =  new Vector3(Random.Range((int)target.x - random_Lazer_Ray_Range, (int)target.x + random_Lazer_Ray_Range), target.y, target.z );
    //}

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
            base.LookAtTarget();
            return true;
        }
         base.EnemieChassing();
        return false;
    }

    private void Update()
    {
        //physic
        this.playerFound = base.PlayerDetected();
        this.canMeleeAttack = base.InMeleeAttackRange();


        if(CanUseLazer())
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
            //print(animValue);
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
