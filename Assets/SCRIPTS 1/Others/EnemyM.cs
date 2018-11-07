using UnityEngine;
using System.Collections;

public class EnemyM : MonoBehaviour
{
    public Material redDmg;
    private Color initialColor;
    public GameObject mesh;

   // private AudioSource source;
    public AudioSource[] source;
    private bool playOne;

    //particula
    public GameObject dmgParticle;
    public GameObject deathParticle;


    //public GameObject dmgParticleFoot;
    private float timerParticle;
    public float durationParticle;
    private bool isHit;



    private RPGCharacterControllerFREE player;
    public float speed;
    private float waitTime = 0.2f;
    private States state;
    Vector3 distance;
    public float player_Distance;
    public int player_DistanceToAttack;
    bool isDead = false;
    public bool enemy_close = false;
    private Rigidbody rb;
    private GameObject closest;

    //ataque temporario
    protected Animator animator;
    float timer_attack;
    float attack_cd;
    public bool isAttacking;
    public GameObject weapon;
    public bool stopAttack = true;
    private int randAttack = 1;


    //gamecontroller
    private GameController gc;
    public int index;

    //pos outro inimigo
    Vector3 otherEnemy_pos;
    Vector3 otherEnemy_Distance;
    bool otherEnemy_close;
    float timer_waitingToAttack;
    float timer_waitDirection;

    //dodge wall
    private Vector3 direction;
    private RaycastHit hit;
    public int range = 5;
    public bool canMove = true;

    //variaveis de verificaçao objeto mais perto do player
    Vector3 distance_Right;
    Vector3 distance_Left;
    private int isRight;
    private float timer_dodge;

    //distancia do player
    private int dist_nRand;
    private bool farEnemy;

    //levando dano
    bool isKnockback;
    private float timer_knockback;
    public float knockbackMultiplier = 1f;
    private float health = 200f;

    //Navmesh
    private UnityEngine.AI.NavMeshAgent agent;
    public Transform[] destination;
    public float timer_destination;
    public float timer_destStay = 1f;
    public int count = 0;
    private int temp = 1;
    public float distance_destination;

    //public typeDamage
    private int damage_type;
    public enum States
    {
        LookingForPlayer,
        Follow,
        Attack,
        Waiting,
        Death
    }

    void Awake()
    {
        initialColor = redDmg.color;
        //source = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;

        closest = this.gameObject;
       // gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() as GameController;
        rb = GetComponent<Rigidbody>();

    }
    // Use this for initialization
    void Start()
    {
        timer_destination = timer_destStay;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        state = States.LookingForPlayer;
        StartCoroutine(verificaFSM(waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDead)
        {
            agent.isStopped = true;
            weapon.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            state = States.Death;
        }
        else
        {
            if (health <= 0)
            {
                isDead = true;
            }
            else
            {
                if (!PlayerClose("attack"))
                {
                    isAttacking = false;

                }
                // Debug.Log(enemy_close);

                // Debug.Log(state);
                distance_destination = agent.remainingDistance;
                attack_cd -= Time.deltaTime;
                timer_attack -= Time.deltaTime;
                timer_waitingToAttack -= Time.deltaTime;
                timer_waitDirection -= Time.deltaTime;
                timer_dodge -= Time.deltaTime;
                distance = player.transform.position - transform.position;
                timer_knockback -= Time.deltaTime;

                animator.SetBool("isKnock", isKnockback);
   



                if (PlayerOnVision() || isKnockback)
                {
                    if(!playOne)
                    {
                        source[1].Play();
                        playOne = true;
                    }
                    transform.transform.LookAt(new Vector3(player.transform.position.x, transform.transform.position.y, player.transform.position.z));
                }

                particleOn();
            }
           
        }
      

    }
 
    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if (state == States.LookingForPlayer)
            {
  
                animator.SetBool("Side", false);

                if (PlayerClose("find") && PlayerOnVision())
                {

                    state = States.Follow;
                }
                else
                {
                    Watching();

                }
            }
            else if (state == States.Follow)
            {
                if (!PlayerClose("find"))
                {
                    state = States.LookingForPlayer;
                }
                else
                {
                    if (PlayerClose("attack"))
                    {
                        state = States.Attack;
                    }
                    else
                    {
                        if (this.gameObject == FindClosestEnemy())
                        {
                            Follow(FindClosestEnemy(), true);
                        }
                        else
                        {
                            Follow(this.gameObject, false);

                        }
                    }

                }
            }
            else if (state == States.Attack)
            {
                if (!PlayerClose("attack"))
                {
                    state = States.LookingForPlayer;
                }
                else
                {
                    randAttack = Random.Range(1, 3);
                    Attack();
                    state = States.Waiting;

                }
            }
            else if (state == States.Waiting)
            {
                if (timer_waitingToAttack < 0)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    state = States.LookingForPlayer;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.None;
                    WaitingToAttack();
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                {
                    redDmg.color = initialColor;
                    source[2].Play();
                    animator.SetTrigger("Death");
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                    {
                        deathParticle.SetActive(true);
                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                    {
                        mesh.SetActive(false);
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(waitTime);

        }
    }

    private void particleOn()
    {
        dmgParticle.SetActive(isHit);

        if (isHit)
        {
            redDmg.color = Color.red;
            if (timerParticle < durationParticle)
            {
                timerParticle += Time.deltaTime;
            }
            else
            {
                isHit = false;
            }
        }
        else
        {
            redDmg.color = initialColor;
            timerParticle = 0f;
        }
        //dmgParticleFoot.SetActive(isHit);
    }

    void GetHit()
    {
        isHit = true;
        if(damage_type == 1)
        {
            player.GetComponent<RPGCharacterControllerFREE>().camera_shake = true;
            health -= 50f;
            int hits = 5;
            int hitNumber = Random.Range(0, hits);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
            {
                animator.SetTrigger("Knock");
            }
           // StartCoroutine(_LockMovementAndAttack(0, .2f));
            //apply directional knockback force
            if (hitNumber <= 1)
            {
                StartCoroutine(_Knockback(-transform.forward, 8, 4));
            }
            else if (hitNumber == 2)
            {
                StartCoroutine(_Knockback(transform.forward, 8, 4));
            }
            else if (hitNumber == 3)
            {
                StartCoroutine(_Knockback(transform.right, 8, 4));
            }
            else if (hitNumber == 4)
            {
                StartCoroutine(_Knockback(-transform.right, 8, 4));
            }
            damage_type = 0;
        }
        else if(damage_type == 2)
        {
            health -= 25f;
            int hits = 5;
            int hitNumber = Random.Range(0, hits);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
            {
                animator.SetTrigger("Knock");
            }
           // StartCoroutine(_LockMovementAndAttack(0, .2f));
            //apply directional knockback force
            if (hitNumber <= 1)
            {
                StartCoroutine(_Knockback(-transform.forward, 8, 4));
            }
            else if (hitNumber == 2)
            {
                StartCoroutine(_Knockback(transform.forward, 8, 4));
            }
            else if (hitNumber == 3)
            {
                StartCoroutine(_Knockback(transform.right, 8, 4));
            }
            else if (hitNumber == 4)
            {
                StartCoroutine(_Knockback(-transform.right, 8, 4));
            }
            damage_type = 0;

        }


    }

    IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        isKnockback = true;
        StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
     
        yield return new WaitForSeconds(.5f);
        timer_knockback = .5f;
        isKnockback = false;
    }

    IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        while (isKnockback)
        {
            rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
            yield return null;
        }
    }

    //method to keep character from moveing while attacking, etc
    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        canMove = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        animator.applyRootMotion = true;
        yield return new WaitForSeconds(lockTime);
        canMove = true;
        animator.applyRootMotion = false;
    }
    private bool PlayerOnVision()
    {
        if (distance.magnitude <= player_Distance)
        {
            if(!stopAttack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else 
        {
            return false;
        }
    }
    private bool PlayerClose(string objective)
    {
        float distance_player;
        if (objective == "attack")
        {
            distance_player = player_DistanceToAttack;
           // animator.SetBool("Moving", false);

        }
        else
        {
            distance_player = player_Distance;

        }

        if (distance.magnitude <= distance_player)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void WaitingToAttack()
    {
        float auxTimer = 0;
        dist_nRand = Random.Range(player_DistanceToAttack, 10);
        if (timer_waitDirection < -2f)
        {
            timer_waitDirection = 2f;
        }

        agent.stoppingDistance = player_DistanceToAttack+5f ;

        if (distance.magnitude <= player_DistanceToAttack+5f) //&& CanMove("backward")
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
            {
                animator.SetTrigger("Moving");
            }
            rb.velocity = (transform.forward * speed * Time.deltaTime * -4);
            // agent.SetDestination(player.transform.position);

            auxTimer = 0;
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        else
        {
            isAttacking = false;

            // animator.SetBool("Moving", true);
            // transform.transform.LookAt(new Vector3(player.transform.position.x, transform.transform.position.y, player.transform.position.z));
            auxTimer += Time.deltaTime;
            if (timer_waitDirection < 0)
            {
                animator.SetBool("Side", true);
                //   if(CanMove("right"))
                //   {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MoveRight"))
                {
                    animator.SetTrigger("MoveRight");
                }
                rb.velocity = (transform.right * speed * Time.deltaTime * 2);

                //transform.transform.Translate(Vector3.right * speed * Time.deltaTime);
                //  }
            }
            else
            {
                animator.SetBool("Side", true);

                //if(CanMove("left"))
                // 
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MoveLeft"))
                {
                    animator.SetTrigger("MoveLeft");
                }

                rb.velocity = (transform.right * speed * Time.deltaTime * -2);

                //  transform.transform.Translate(Vector3.left * speed * Time.deltaTime);
                //   }
            }

            if (auxTimer > 5f)
            {
              //  state = States.Follow;
            }
        }

    }
   
    private void Watching()
    {
        if(!source[0].isPlaying)
        {
            source[0].Play();
        }
        
        agent.stoppingDistance = 0;

        if (timer_destination < 0)
        {
            if (count >= (destination.Length - 1))
            {
                temp = -1;
            }
            if (count <= 0)
            {
                temp = 1;
            }
            count += temp;
            timer_destination = timer_destStay;
        }
        agent.SetDestination(destination[count].position);

        if (distance_destination >= 0.5f)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
            {
                animator.SetTrigger("Moving");
            }
        }
        else
        {
        
            timer_destination -= Time.deltaTime;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.SetTrigger("Idle");
            }

        }
        // Debug.Log(distance_destination);

        // transform.LookAt(destination[count]);
    }

    private void Follow(GameObject go, bool closest)
    {
        // if ( timer_knockback < 0)

        if (closest)
        {
            go.GetComponent<EnemyM>().agent.stoppingDistance = 1.5f;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
            {
                animator.SetTrigger("Moving");
            }
            go.GetComponent<EnemyM>().agent.SetDestination(player.transform.position);
        }
        else
        {
            go.GetComponent<EnemyM>().agent.stoppingDistance = 4;
            Debug.Log(go.GetComponent<EnemyM>().agent.remainingDistance);
            if (go.GetComponent<EnemyM>().agent.remainingDistance < 4)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.SetTrigger("Idle");
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
                {
                    animator.SetTrigger("Moving");
                }
            }
         
            go.GetComponent<EnemyM>().agent.SetDestination(player.transform.position);
        }


        //  agent.SetDestination(player.transform.position);
        //  go.transform.LookAt(new Vector3(player.transform.position.x, go.transform.position.y, player.transform.position.z));
        // go.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // go.GetComponent<Rigidbody>().velocity = (go.transform.forward * Time.deltaTime * speed);

    }

    private void Attack()
    {
        if(timer_knockback < 0)
        {
            //transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            if (!isAttacking)
            {
                animator.SetTrigger("Attack" + randAttack);
                weapon.GetComponent<Collider>().enabled = true;
                isAttacking = true;
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    weapon.GetComponent<Collider>().enabled = false;
                    timer_waitingToAttack = 5f;
                    timer_attack = 2f;
                    isRight = 0;
                }
            }
          
        }
    }
    
    private GameObject FindClosestEnemy()
    {
        GameObject[] gos;
       // Vector3 distanceTemp;
        gos = GameObject.FindGameObjectsWithTag("EnemyM");

        foreach (GameObject go in gos)
        {
           // distanceTemp = player.transform.position - go.transform.position;

            //if (distanceTemp.magnitude <= player_DistanceToAttack+5f && !enemy_close || closestList.Count == 0)
            if(closest == null)
            {
                closest = this.gameObject;
            }
            if ((closest.transform.position - player.transform.position).magnitude >= (go.transform.position - player.transform.position).magnitude)
            {
                closest = go;
            }
    


        }
        return closest;

    }

    void OnTriggerExit(Collider col)
    {
        if(!isDead)
        {
            if (col.gameObject.tag == "Weapon")
            {
                source[3].Play();
                damage_type = 1;
                GetHit();
            }
            if (col.gameObject.tag == "Arrow")
            {
                source[3].Play();
                damage_type = 2;
                GetHit();
                Destroy(col.gameObject);

            }
        }
     
    }
}

