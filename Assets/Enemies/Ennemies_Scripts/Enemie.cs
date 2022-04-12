using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
//Important Area Need To be in Trigger, Y position of the transform = 0, collider size Y = 0, Layer = Area, Tag = Area(1,2,3,etc...)


//main class to herite for enemie gameobject
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemie : MonoBehaviour
{
    // EnemieManager is use for respawn enemie after an amount of time
    protected EnemieManager enemieManager;
    private bool isRemoved = false;
    // behaviour value
    [Header("Enemie stats")]
    [SerializeField] private Scriptable_Stats_Enemies enemie_stats;

    //melee range  ,  player range detection
    protected float enemieRange ;
    protected float MeleeAttackRange;

    //enemie area variable
    [Header("Enemie Area")]
    [SerializeField] private string enemieArea;
    private bool isAreaSet = false;

    // player variable
    [Header("Player Informations")]
    [SerializeField] protected GameObject myTarget;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] protected LayerMask whatIsPlayer;


    // patroll variable
    [Header("Walk Range Value")]
    [Range(3f, 25f)] [SerializeField] private float randWalkValue = 5f;
    protected bool walkDestinationSet;
    private Vector3 nextWalkDest;
    //melee attack variable
    protected bool attackDone = false;
    private float meleeImpluseForce;
    private bool powerIncresed = false;
    [SerializeField] private Collider bodyColl;
    //revive variable
    [Header("Reviving status")]
    [Range(5f,120f)] [SerializeField] private  float reviveTimer = 5f;
    [SerializeField] private bool isRevivable = true;
    private bool isDead = false;
    //Sounds
    [Header("Enemie Audio Sounds")]
    [SerializeField] protected AudioClip breathingSound;
    [SerializeField] protected AudioClip deadSound;
    [SerializeField] protected AudioClip meleeAttackSound;
    [SerializeField] protected AudioClip  playerHitted;
    protected AudioSource audioSource;
    protected bool isPlayed = false; // is player found sound play
   
    //those value are set in awake method;
    protected EnemieType enemieType; // can add this to scriptable initialisator
    protected Vector3 startpos;


    public bool haveToken = false;

    [SerializeField] private bool countAdded = false;


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


    protected void SetSound(AudioClip audio)
    {
        this.audioSource.PlayOneShot(audio);
    }
    public void TokenManagement()
    {
        if (this.CanHaveToken() && !this.haveToken && this.healthPoints > 0 && this.enemieManager.CanHaveToken())
        {
            this.enemieManager.GiveEnemieToken();
            this.haveToken = true;
        }

        else if (this.healthPoints <= 0 && this.haveToken)
        {
            this.enemieManager.RemoveEnemieToken();
            this.haveToken = false;
        }

        else if(!PlayerDetected() && this.haveToken)
        {
            this.enemieManager.RemoveEnemieToken();
            this.haveToken = false;
        }
    }

    //Get -- Set  section 
    //------------------------------------------------//
    public int HealthPoints { get => healthPoints; set => healthPoints = value; }
    public int RealDamage { get => InflictDamage(); }
    public float MeleeImpluseForce { get => meleeImpluseForce; }
    public int AttackPower { get => attackPower; set => attackPower = value; }



    //abstract methode  section 
    //------------------------------------------------//
    public abstract void AttackCompleted();
    protected abstract void SpecialMove();

    //Starting stat section 
    //------------------------------------------------//

    private void Start()
    {
           this.enemieManager = EnemieManager.instance;
            if (this.enemieType == EnemieType.CHOMPER)
                this.enemieManager.ListOfChomper.Add(this);
    }
    protected void GetComponent()
    {
       this.anim = GetComponent<Animator>();
       this.agent = GetComponent<NavMeshAgent>();
       this.obstacle = GetComponent<NavMeshObstacle>();
       this.audioSource = GetComponent<AudioSource>(); 
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
    protected  void GetStats()
    {
        //set base statistique
        this.name = enemie_stats.Name;
        this.attackPower = this.enemie_stats.AttackPower;
        this.healthPoints = this.enemie_stats.HealthPoints;
        this.defensePoints = this.enemie_stats.DefensePoints;
        this.maxHealthPoints = this.healthPoints;
        this.enemieType = this.enemie_stats.Type;
        //set default range 
        this.enemieRange = this.enemie_stats.DetectionPlayerRange;
        this.MeleeAttackRange = this.enemie_stats.MeleeAttackRange;
        //force 
        this.meleeImpluseForce = this.enemie_stats.MeleeImpluseForce;
        //
        this.walkDestinationSet = false;
        //initialise player to be able to lacate him 
        if (this.myTarget == null)  //the name must fit with the the scene name
                this.myTarget = GameObject.Find("Player");

       this.countAdded = true;
    }
    //Animation section 
    //------------------------------------------------//

    protected virtual void EnemieAnimation()
    {
        this.anim.SetFloat("magnitude", this.agent.velocity.magnitude);
        this.anim.SetBool("isPlayer", this.PlayerDetected());
        this.DeadBehaviour();
        this.TokenManagement();
    }
    private void DeadBehaviour()
    {
        if (this.healthPoints <= 0)
        {
            if (this.bodyColl.enabled)
                this.bodyColl.enabled = false;

            if (!isDead)
            {
                this.AgentDestination(transform.position);
                this.anim.SetBool("isDead", true);
                this.agent.isStopped = true;
                this.SetSound(this.deadSound);
                isDead = true;
            }

            if (this.countAdded)
            {
                this.playerStats.EnemiesCount += 1;
                this.enemieManager.DisplayEnemieCounter();
                this.countAdded = false;
            }
            if (this.isRevivable)
            {
                this.isRevivable = false;
                EnemieData enemieData = new EnemieData(this.enemieType, this.startpos, this.reviveTimer);
                this.enemieManager.StartCoroutine(this.enemieManager.EnemieReviver(enemieData));
            }
            
            if (!this.isRemoved && this.enemieType == EnemieType.CHOMPER)
            {
                this.enemieManager.ListOfChomper.Remove(this);
                this.isRemoved = true;
            }


            Destroy( gameObject, 1.5f);
        }
    }
    

    //Physic section 
    //------------------------------------------------//
    protected bool PlayerDetected()
    {
        return Physics.CheckSphere(transform.position, this.enemieRange, this.whatIsPlayer)  && this.enemieArea == this.playerStats.PlayerArea && this.haveToken && this.healthPoints > 0;
    } 
    protected bool CanHaveToken()
    {
        return Physics.CheckSphere(transform.position, this.enemieRange, this.whatIsPlayer)  && this.enemieArea == this.playerStats.PlayerArea && this.healthPoints > 0;
    }
    protected bool InMeleeAttackRange()
    {
        return Physics.CheckSphere(transform.position, this.MeleeAttackRange, this.whatIsPlayer) && this.healthPoints > 0;
    }

    protected void ResetHealth()
    {
        if(this.healthPoints <= (int)this.maxHealthPoints* 0.5f && !powerIncresed) 
        {
            var regainChance = Random.Range(0, 100) < 10;
            if (regainChance )
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

        if (this.healthPoints == this.maxHealthPoints)
            damage = RandomValue((int)(this.attackPower * 0.25f), (int)(this.attackPower * 0.75f));
        else if (this.healthPoints < (int)(this.healthPoints * 0.50f))
            damage = RandomValue((int)(this.attackPower * 0.50f), (int)(this.attackPower));
        else
            damage = (int)(this.attackPower * 0.5f);
        return damage;
    }

    private void EnrageMode()
    {
            float scaleEmplifer = 1.5f;
            float attackPowerEmplifier = 1.2f;
            bool chanceToEnrage = this.RandomValue(0, 100) > 30 ;
            if (chanceToEnrage)
            {
                this.transform.localScale *= scaleEmplifer;
                this.attackPower = (int)(this.attackPower * attackPowerEmplifier);
            }
                this.powerIncresed = true;

    }
    
    private bool IsNextPosInArea(Vector3 nextPos)
    {
        //float yOffset = 1f; // add an offset to be able to check the areazone
        var maxRange = 1.2f; // add a range for the ray
        int layerMask = 1 << 17;
        if (Physics.Raycast(nextPos, Vector3.down, out RaycastHit hit, maxRange, layerMask))
        {
            if(hit.collider.isTrigger && hit.collider.CompareTag(this.enemieArea))
            {
                if(this.IsValidPath(nextPos))
                    return true;
            }
        }
        return false;
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
            this.healthPoints -= realDamage;
            ResetHealth(); //if lucky will receive reset health
        }
    }
    //Behaviour section 
    //------------------------------------------------//
    //** weapon must have a force value to be use on ennemie to add ::: force parameter to me more versatile for all behaviour
    public void AdaptiveForce(float hitRange,float impluseForce, bool isLazer = false)
    {
        if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), this.transform.forward, out RaycastHit hit, hitRange) && hit.transform.CompareTag("Player"))
        {
            var contact = hit.point - transform.position;
            contact.y = 0; // remove force on  y 
            this.myTarget.GetComponent<Rigidbody>().AddForce(contact.normalized * impluseForce, ForceMode.Impulse);
            this.playerStats.HealthPoints -= RealDamage;
            if(!isLazer)
            this.SetSound(this.playerHitted);
        }
    }

    protected void ResetAttack()
    {
        this.attackDone = false;
    }


    protected void EnemieChassing()
    {
       
            if (!isPlayed)
            {
                this.SetSound(this.breathingSound);
                isPlayed = true;
            }
            if (walkDestinationSet)
                walkDestinationSet = false;

            this.AgentDestination(this.myTarget.transform.position); //apply movement
        
    }
    protected IEnumerator ChangeBehaviour()
    {
        if (this.obstacle.enabled != false && this.agent.enabled != true)
        {

            this.obstacle.enabled = false;
            yield return new WaitForSeconds(0.1f);
            this.agent.enabled = true;
        }
    }
    private void ResetAgent()
    {
        this.agent.enabled = true;
    }
    protected void AgentDestination(Vector3 nextPath)
    {
       //if(IsValidPath(nextPath))
        this.agent.destination = (nextPath);
    }

    protected void AgentStatBehaviour(float speedValue, float enemieRange)
    {
        if(this.agent.speed != speedValue && this.enemieRange != enemieRange)
        {
            this.agent.speed = speedValue;
            this.enemieRange = enemieRange;
        }
    }
    protected bool IsValidPath(Vector3 path)
    {
        NavMeshPath navPath = new NavMeshPath();
        this.agent.CalculatePath(path, navPath);
        if (navPath.status == NavMeshPathStatus.PathInvalid)
            return false;

        return true;
    }

    protected void EnemieWalk()
    {

            if (isPlayed)
                isPlayed = false;

            if (!walkDestinationSet && this.isAreaSet)
            {
                StartCoroutine(this.ChangeBehaviour());
                //check path 
                nextWalkDest = RandomEnemieDestionation();
                while (!IsNextPosInArea(nextWalkDest))
                {
                    nextWalkDest = RandomEnemieDestionation();
                    if (this.attackDone) return;
                }

                AgentDestination(nextWalkDest);
                walkDestinationSet = true;
            }
            if (Vector3.Distance(nextWalkDest, transform.position) < 1f)
                walkDestinationSet = false;
    }

    private Vector3 RandomEnemieDestionation()
    {
        return new Vector3(Random.Range(transform.position.x - this.randWalkValue, transform.position.x + this.randWalkValue),
               transform.localPosition.y , Random.Range(transform.position.z - this.randWalkValue, transform.position.z + this.randWalkValue));
    }

    protected void MeleeAttack(string attackName)
    {
        var attackPos = this.transform.position.z > 0 ? 2.5f : -2.5f;
        if (!this.attackDone /*&& this.IsValidPath(new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z + attackPos))*/)
        {
            AgentDestination(this.transform.position); // stop player from moving
            this.LookAtTarget();
            this.anim.SetTrigger(attackName); // set my attack
            this.agent.enabled = false;
            this.obstacle.enabled = true;
            this.attackDone = true;// wait Invoke for attack again
           
        }
    }
    protected void LookAtTarget()
    {
        var lookAtTarget = new Vector3(this.myTarget.transform.position.x, this.transform.position.y, this.myTarget.transform.position.z);
        this.transform.LookAt(lookAtTarget);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Area") && !this.isAreaSet)
        {
            this.enemieArea = other.tag;
            this.isAreaSet = true;
        }
    }

}
