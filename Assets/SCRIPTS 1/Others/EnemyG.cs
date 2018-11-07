using UnityEngine;
using System.Collections;

public class EnemyG : MonoBehaviour
{
    public Material redDmg;
    private Color initialColor;

    //audio
    public AudioSource[] source;
    private int clipRand;

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
    public float player_DistanceToAttack;
    bool isDead = false;
    bool enemy_close = false;
    private Rigidbody rb;
    
    //ataque temporario
    protected Animator animator;
    float attack_cd;
    public bool isAttacking;
   // public GameObject weapon;
    public Collider weapon;
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


    //distancia do player
    private int dist_nRand;

    //levando dano
    bool isKnockback;
    public float knockbackMultiplier = 1f;
    private float health = 100f;

    //weakness
    public GameObject weakness;
    public bool weakOut = false;

    //damage type
    private int damage_type;

    //animation aux
    private bool atkAux;
    private bool moveAux;
    private bool isknock;

    private UnityEngine.AI.NavMeshAgent agent;

    public enum States
    {
        LookingForPlayer,
        Follow,
        Attack,
        Waiting,
        GemOut
    }

    void Awake()
    {
        initialColor = redDmg.color;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
        //gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() as GameController;
        rb = GetComponent<Rigidbody>();

    }
    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        state = States.LookingForPlayer;
        StartCoroutine(verificaFSM(waitTime));
        
    }

    // Update is called once per frame
    void Update()
    {
  
        agent.destination = player.transform.position;

        attack_cd -= Time.deltaTime;
        timer_waitingToAttack -= Time.deltaTime;
        timer_waitDirection -= Time.deltaTime;



        if(weakness == null && !weakOut)
        {
            if(state != States.GemOut)
            {
                animator.Play("Attack", 0, 0f);
            }
            state = States.GemOut;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
            {
                weapon.enabled = true;
            }
        }
 
        if (health <= 0)
        {
            isDead = true;
        }

    }

    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if (!isDead)
            {
                if (state == States.LookingForPlayer)
                {
                    agent.isStopped = true;

                    if (PlayerClose("find") && agent.remainingDistance > 0)
                    {
                 
                        state = States.Follow;

                    }
                    else
                    {
                        clipRand = Random.Range(0, 2);

                        if(!source[clipRand].isPlaying)
                        {
                            source[clipRand].Play();
                        }
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                    
                            animator.SetTrigger("Idle");
                        }
           
                    }
                }
                else if (state == States.Follow)
                {
                    if (PlayerClose("attack"))
                    {
                        state = States.Attack;
                    }
                    else
                    {
                        Follow();
                    }
                }
                else if (state == States.Attack)
                {
                    Attack();
                   
                }
                else if (state == States.Waiting)
                {
                    if (timer_waitingToAttack < 1)
                    {
                        animator.Play("Attack", 0, 0f);
                        animator.SetBool("isAttack", false);
                        atkAux = false;
                        animator.speed = 1;
                        state = States.Follow;
                    }
                    else
                    {
                        WaitingToAttack();
                    }
                }
                else if(state == States.GemOut)
                {
                    animator.speed = 1;
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knock1") && !isknock)
                    {
                        source[3].Play();
                        animator.SetTrigger("Knock1");
                        isknock = true;
                    }
                    else
                    {
              
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                        {
                            source[4].Play();
                            weakOut = true;
                            state = States.Waiting;

                        }
                    
                    }
                }

            }

            else
            {
                animator.speed = 1;

                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                {
                    source[5].Play();
                    animator.SetTrigger("Death");
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        Destroy(this.gameObject);
                    }
                }
                //GetHit();
            }
            yield return new WaitForSeconds(waitTime);

        }
    }

    void GetHit()
    {
        isHit = true;
        if (damage_type == 1)
        {
            if (player.GetComponent<RPGCharacterControllerFREE>().attack_animatTime > 0)
            {
                player.GetComponent<RPGCharacterControllerFREE>().camera_shake = true;
                health -= 25f;
                int hits = 5;
                int hitNumber = Random.Range(0, hits);
                animator.SetTrigger("Knock0");

                StartCoroutine(_LockMovementAndAttack(.1f, .4f));
              
                damage_type = 0;
            }
        }
        else if(damage_type == 2)
        {
            player.GetComponent<RPGCharacterControllerFREE>().camera_shake = true;
            health -= 25f;
            int hits = 5;
            int hitNumber = Random.Range(0, hits);
            animator.SetTrigger("Knock0");

            StartCoroutine(_LockMovementAndAttack(.1f, .4f));
            //apply directional knockback force
         
            damage_type = 0;

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
    }


    //method to keep character from moveing while attacking, etc
    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        canMove = false;
        animator.SetBool("Moving", false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        animator.applyRootMotion = true;
        yield return new WaitForSeconds(lockTime);
        canMove = true;
        animator.applyRootMotion = false;
    }
    private bool PlayerClose(string objective)
    {
        float distance_player;
        if (objective == "attack")
        {
            distance_player = player_DistanceToAttack;
        }
        else
        {
            distance_player = player_Distance;
        
        }
        if (agent.remainingDistance <= distance_player)
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

        animator.SetBool("Moving", false);

        rb.isKinematic = true;
    }


    private void Follow()
    {
        /*
        bool aux = false;
        bool startWalk = false;

        if (!aux)
        {
            animator.SetTrigger("StartWalk");
            aux = true;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                startWalk = true;
            }
        }
        Debug.Log(aux);

        if (startWalk)
        {
         
        }
        */
        agent.isStopped = false;

        rb.isKinematic = false;
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        animator.SetBool("Moving", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !moveAux)
        {
            animator.SetTrigger("Walk");
            moveAux = true;
        }
        else
        {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                moveAux = false;

            }

        }

        agent.SetDestination(player.transform.position);

        /*
        // go.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        go.GetComponent<Rigidbody>().velocity = (go.transform.forward * Time.deltaTime * speed);
        */
    }

        private void Attack()
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("Moving", false);

            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));


        bool attack = false;
        agent.isStopped = true;

  
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !atkAux)
        {
            source[2].Play();
            animator.Play("Walk", 0, 0f);
            animator.SetTrigger("Attack");
           // weapon.enabled = true;

            atkAux = true;
        }
        else
        {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f && !attack)
            {
                weapon.enabled = false;
                animator.speed = 0;
                timer_waitingToAttack = 5f;
                state = States.Waiting;
                attack = true;
            }
   
        }


    }
    /*
    private GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        Vector3 distanceTemp;

        gos = GameObject.FindGameObjectsWithTag("EnemyM");
        GameObject closest = gameObject;
        foreach (GameObject go in gos)
        {
            distanceTemp = player.transform.position - go.transform.position;

            if (distanceTemp.magnitude <= player_DistanceToAttack)
            {

                closest = go;
            }
            else
            {
                animator.SetBool("Moving", false);
                enemy_close = true;
            }
        }


        if (closest != gameObject)
        {
            enemy_close = true;
        }
        else
        {
            enemy_close = false;
        }


        return closest;
    }
    */

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Weapon" && weakOut)
        {
            source[6].Play();
            damage_type = 1;
            GetHit();

        }
        if (col.gameObject.tag == "Arrow" && weakOut)
        {
            source[6].Play();
            damage_type = 2;
            GetHit();
            Destroy(col.gameObject);

        }
    }
}

