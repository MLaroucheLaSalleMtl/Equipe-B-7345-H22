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
    private const int RANDOM_LAZER_RAY_RANGE = 1;
    private const float lazerResetTime = 3f;

    //initialise into awake overide parent fonction
    private bool canLazer;
    private bool isLazerCooldown;
    private const float LazerCooldown = 3f; //seconds
    private Vector3 nextRayPos = Vector3.zero;
    
    [SerializeField] private LineRenderer lazerRenderer; // ** WARNING linerenderer must be at Vector(0,0,0)
    [SerializeField] private Transform lazerStartPos;
    [SerializeField] private PlayerStats playerStats;
    //melee Behaviour
    private string[] meleeAnim;
    private int animValue;
    //when facing enemie righthand is at left and lefthand is at the right
    [SerializeField] private CapsuleCollider[] handColls; // left is [0] and righ is [1]
  
    // both are assing in update for check the attack range and player detection
    private bool playerFound;
    private bool canAttack;
    private void Awake()
    {
        base.GetComponent();
        base.GetStats();
        this.SetMeleeAnim();
        this.SetMeleeColl();
        this.lazerRenderer.enabled = false;
        this.lazerRenderer.gameObject.transform.position = Vector3.zero;
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

    //Lazer Beam Behaviour
    private void CanLazerBeam()
    {
        if (!IsInvoking(nameof(NextRayCastValue))) 
                Invoke(nameof(NextRayCastValue),1f);

        NavMeshHit hit;
        if (!isLazerCooldown)
        {
            if (nextRayPos != Vector3.zero)
            {
                if (!base.agent.Raycast(nextRayPos, out hit))
                {
                    Vector3Int target = IntTargetValuePosition();

                    if (hit.distance > lazerMinRange && hit.distance < lazerMaxRange && hit.position.x == target.x)
                    {
                        base.AgentDestination(this.transform.position);
                        canLazer = true;
                        base.LookAtTarget();
                    }
                }
            }
        }
    }
    #region LzerAnimEvent
    public void EnableBeam()
    {
       
        if (!this.lazerRenderer.enabled)
            this.lazerRenderer.enabled = true;

        VisualLazerBeam();
        this.playerStats.HealthPoints -= RealDamage;
    }
    public void DisableBeam()
    {
        if (this.lazerRenderer.enabled)
        {
            this.lazerRenderer.enabled = false;
        }
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
    private Vector3Int IntTargetValuePosition()
    {
        Vector3 targetPosition = base.myTarget.transform.position;
        return new Vector3Int((int)(targetPosition.x), (int)(targetPosition.y), (int)(targetPosition.z));
    }
    private void NextRayCastValue()
    {
        Vector3Int target = IntTargetValuePosition();
        this.nextRayPos =  new Vector3(Random.Range((int)target.x - RANDOM_LAZER_RAY_RANGE, (int)target.x + RANDOM_LAZER_RAY_RANGE), target.y, target.z );
    }
    private void VisualLazerBeam()
    {
        lazerRenderer.SetPosition(0, lazerStartPos.position);
        lazerRenderer.SetPosition(1, base.myTarget.transform.position);
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
            return true;

        }
        base.AgentDestination(base.myTarget.transform.position);
        return false;
    }

    private void Update()
    {
        this.playerFound = base.PlayerDetected();
        this.canAttack = base.InAttackRange();
        print(playerFound);
        if (this.playerFound && CanUseLazer() && !canAttack)
        CanLazerBeam();
        //when chassing player
        if (playerFound && !canAttack && !CanUseLazer())
        {
            //print("chasse");
            base.EnemieChassing();
        }
        ////when melee attack
        if (canAttack && playerFound && !CanUseLazer())
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
    
    //private void Meme(string attackName)
    //{
    //    AgentDestination(this.transform.position); // stop player from moving

    //    if (!this.attackDone)
    //    {
    //        var lookAtTarget = new Vector3(this.myTarget.transform.position.x, this.transform.position.y, this.myTarget.transform.position.z);
    //        transform.LookAt(lookAtTarget);
    //        anim.SetTrigger(attackName); // set my attack
    //        this.agent.enabled = false;
    //        this.obstacle.enabled = true;
    //        this.attackDone = true;// wait Invoke for attack again
    //    }
    //}
    private void RandomMeleeAttack()
    {
        if (!base.attackDone)
        {
            animValue = base.RandomValue(0, 1);
            print(animValue);
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
    #region Animation event
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
        this.MovingBehaviour();
        Invoke(nameof(base.ResetAttack), 1f);
    }
    #endregion
    protected override void SpecialMove()
    {
        throw new System.NotImplementedException();
    }


   
}
