using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyP : MonoBehaviour
{

    private RPGCharacterControllerFREE player;

    //distance settings
    private float distance;
    public float distFind;
    public float distAttack;
    public float distRun;

    public Transform frogModel;
  
    public float health;
    public bool playerOn;

    //place to jump
    private int placeRand;
    public Transform[] placeToJump;
    private Vector3 placePos;

    //attack settings
    private float cdAttack;
    public float coldown;
    private bool canAttack = true;
    public Transform weaponLocal;
    public Transform weaponPrefab;


    //jump
    private bool isJumped;

    //Inspector itens
    private Rigidbody rb;
    private Animator animator;


    //Navmesh
    private UnityEngine.AI.NavMeshAgent agent;

    //state machine
    private States state;
    private float waitTime = 0.2f;

    public enum States
    {
        LookingForPlayer,
        Follow,
        Attack,
        Run,
        Death
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
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

        if (health <= 0)
        {
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            state = States.Death;
        }
        else
        {
            if(playerOn)
            {
                if (state != States.Run || cdAttack < 0)
                {
                    agent.destination = player.transform.position;
                }
                else
                {
                    agent.destination = placeToJump[2].position;

                }
                distance = agent.remainingDistance;
                cdAttack -= Time.deltaTime;
            }
        
        }


    }

    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if (state == States.LookingForPlayer)
            {
                if (PlayerDistance() == 0)
                {
                    state = States.Follow;
                }
                else
                {
                    animator.SetTrigger("Idle");
                }
            }
            else if (state == States.Follow)
            {
                switch (PlayerDistance())
                {
                    case 1:
                        state = States.Attack;
                        break;
                    case 2:
                        state = States.Run;
                        break;
                    default:
                        Follow();
                        break;
                }

            }
            else if (state == States.Attack)
            {
                Attack();
            }

            else if (state == States.Run)
            {
                if(PlayerDistance() == 2)
                {
                    RunAway();
                }
                else
                {
                    if(cdAttack < 0)
                    {
                        canAttack = true;
                        transform.LookAt(player.transform);
                        state = States.Follow;
                    }
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                {
                    animator.SetTrigger("Death");
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        Destroy(gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(waitTime);

        }
    }



 

    void GetHit()
    {
        player.GetComponent<RPGCharacterControllerFREE>().camera_shake = true;
        health -= 50f;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
        {
            animator.SetTrigger("Knock");
        }
    }
    private int PlayerDistance()
    {
        if(canAttack)
        {
            if (distance < distAttack)
            {
                return 1;
            }
            else if(distance < distFind)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (distance < distRun)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
      
    }

    void RunAway()
    {
        animator.SetBool("canMove", false);

        if (agent.remainingDistance > 0)
        {
            agent.isStopped = false;
            if(!isJumped)
            {
                animator.SetTrigger("Jump");
                isJumped = true;
            }
            agent.SetDestination(placeToJump[2].position);
        }
        else
        {
            isJumped = false;

        }


    }

    private void Follow()
    {
        animator.SetBool("canMove", true);
        transform.LookAt(player.transform);
        agent.isStopped = false;
        animator.SetTrigger("Walk");
        agent.SetDestination(player.transform.position);
    }

    private void Attack()
    {
        animator.SetBool("canMove", false);

        isJumped = false;
        transform.LookAt(player.transform);
        agent.isStopped = true;

        if (canAttack)
        {
            animator.SetTrigger("Attack");
            canAttack = false;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && cdAttack < 0)
            {
                Instantiate(weaponPrefab.gameObject, new Vector3(weaponLocal.position.x, weaponLocal.position.y, weaponLocal.position.z), weaponLocal.rotation);
                cdAttack = coldown;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f)
            {
                placeRand = Random.Range(0, 2);
                placePos = placeToJump[placeRand].position;
                state = States.Run;
            }
        }


    }

    void OnTriggerExit(Collider col)
    {
        if (health > 0)
        {
            if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Arrow")
            {
                GetHit();
            }
        }

    }
}
