using UnityEngine;
using System.Collections;

public class SmallEnemy : MonoBehaviour
{
    public GameObject mesh;
    public GameObject deathParticle;
    public Material redDmg;
    private Color initialColor;

    //particula
    public GameObject dmgParticle;

    //public GameObject dmgParticleFoot;
    private float timerParticle;
    public float durationParticle;
    private bool isHit;

    public AudioSource[] source;
    private RPGCharacterControllerFREE player;
    private Rigidbody rb;
    public float velocidade = 5f;
    public float health;
    public float waitTime = 0.2f;
    public float distanciaPlayer = 10f;
    bool flip;
    Vector3 distancia;
    bool isDead = false;
    public Transform weapon_prefab;
    float fireRate = 3f;
    public Transform weapon;
    float rotation_timer;
    private int deathCout = 0;
    //jump
    public GameObject[] areasSalto;
    int jump_count = 0;
    float jump_cd = 3;
    float jump_runAwayCD = 2;
    public float jump_speed = 20f;
    float jump_step;
    bool canJump;
    Vector3 jump_pos;

    //navmesh
    private UnityEngine.AI.NavMeshAgent agent;

    private Animator animator;

    public enum States
    {
        LookingForPlayer,
        Follow,
        RunAway,
        Attack,
        Death
    }

    private States state;

    void Start()
    {
        distancia = player.transform.position - transform.position;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        state = States.LookingForPlayer;
        StartCoroutine(verificaFSM(waitTime));
    }

    void Awake()
    {
        initialColor = redDmg.color;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            agent.isStopped = true;
            GetComponent<Collider>().enabled = false;
            state = States.Death;
        }
        else
        {
            particleOn();
            jump_step = jump_speed * Time.deltaTime;
            jump_runAwayCD -= Time.deltaTime;
            jump_cd -= Time.deltaTime;
            fireRate -= Time.deltaTime;
            rotation_timer -= Time.deltaTime;

            distancia = player.transform.position - transform.position;

            if (state != States.LookingForPlayer)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }

            if (state != States.Attack)
            {
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = true;

            }
        }
       
    }
    //Finite-state machine
    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if (state == States.LookingForPlayer && !isDead)
            {
                if (distancia.magnitude <= distanciaPlayer)
                {
                    state = States.Follow;
                }
                else
                {
                    animator.SetTrigger("Idle");
                    if(!source[1].isPlaying)
                    {
                        source[1].Play();

                    }
                }
            }
            else if (state == States.RunAway && !isDead)
            {
                if (PlayerClose())
                {
                    toRunAway(false);
                }
                else
                {
                    state = States.Follow;
                }
            }
            else if (state == States.Follow && !isDead)
            {
                if (distancia.magnitude >= distanciaPlayer)
                {
                    if (PlayerFar())
                    {
                        Follow();
                    }
                    else
                    {
                        state = States.RunAway;
                    }
                }
                else
                {
                    state = States.Attack;
                }
            }
            else if (state == States.Attack && !isDead)
            {
                if (PlayerFar())
                {
                    state = States.Follow;
                }
                else if (PlayerClose())
                {
                    state = States.RunAway;
                }
                else
                {
                    Attacking();
                }
            }
            else
            {
                if (!isDead)
                {
                    animator.SetTrigger("Death");
                    source[3].Play();
                    isDead = true;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
                    {
                        deathParticle.SetActive(true);
                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                    {
                        mesh.SetActive(false);
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        Destroy(gameObject);
                    }                              
                }
            
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
    private void Attacking()
    {
        if (fireRate < 0)
        {
            animator.SetBool("canMove", false);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                {
                    //adiciona força para frente para a nova instancia   
                }

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                {
                    // toRunAway(true);
                    GameObject weapon_shot = Instantiate(weapon_prefab.gameObject, new Vector3(weapon.position.x, weapon.position.y, weapon.position.z), weapon.rotation) as GameObject;
                    source[0].Play();

                    fireRate = 2f;
                }
            
            }
         
        
        }
  

    }


    void GetHit()
    {
        isHit = true;

        source[2].Play();

        player.GetComponent<RPGCharacterControllerFREE>().camera_shake = true;
        health -= 50f;
        animator.SetTrigger("Knock");

    }
        private bool PlayerClose()
    {
        if (distancia.magnitude <= distanciaPlayer / 2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool PlayerFar()
    {
        if (distancia.magnitude >= distanciaPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void toRunAway(bool follow)
    {
        animator.SetTrigger("Jump");

        //quando ta atacando
        if (follow)
        {
            // transform.LookAt(player.transform);

            if (jump_cd <= 0)
            {
                canJump = true;
            }

            if (canJump)
            {
                jump_count = Random.Range(0, 2);
                if (jump_count < 1)
                {
                    // transform.position = areasSalto[0].transform.position;
                    jump_pos = areasSalto[0].transform.position;
                }
                else
                {
                    //transform.position = areasSalto[1].transform.position;
                    jump_pos = areasSalto[1].transform.position;

                }
                jump_cd = 3f;
                canJump = false;
            }
            agent.SetDestination(jump_pos);

            //  transform.position = Vector3.Lerp(transform.position, jump_pos, jump_step);

        }
        //quando foge
        else
        {
            if (jump_runAwayCD <= 0)
            {
                canJump = true;
            }

            if (canJump)
            {
                if (jump_count <= 1)
                {
                    jump_count++;
                }
                else
                {
                    jump_count = 0;
                }
                jump_pos = areasSalto[jump_count].transform.position;
                jump_runAwayCD = 3f;
                canJump = false;
            }
            // transform.position = Vector3.Lerp(transform.position, jump_pos, jump_step);
            agent.SetDestination(jump_pos);

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
    private void Follow()
    {
        animator.SetBool("canMove", true);
        // transform.LookAt(player.transform);
        animator.SetTrigger("Walk");

        // rb.velocity = (transform.forward * velocidade * Time.deltaTime);
        agent.SetDestination(player.transform.position);

        //transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
    }

    void OnTriggerExit(Collider col)
    {
        if(health > 0)
        {
            if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Arrow")
            {

                GetHit();
                redDmg.color = initialColor;
            }

        }


    }
}

