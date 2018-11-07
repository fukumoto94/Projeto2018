using UnityEngine;
using System.Collections;

public class MediumEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float waitTime = 0.2f;
    private States state;
    Vector3 distance;
    public float player_Distance;
    public float player_DistanceToAttack;
    bool isDead = false;
    bool enemy_close = false;
    //ataque temporario
    protected Animator animator;
    float timer_attack;
    float attack_cd;
    //gamecontroller
    private GameController gc;
    public int index;

    //pos outro inimigo
    Vector3 otherEnemy_pos;
    Vector3 otherEnemy_Distance;
    bool otherEnemy_close;

    public enum States
    {
        LookingForPlayer,
        Follow,
        Attack
    }

    void Awake()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() as GameController;
    }
	// Use this for initialization
	void Start ()
    {
        animator = GetComponentInChildren<Animator>();

        state = States.LookingForPlayer;
        StartCoroutine(verificaFSM(waitTime));
	}
	
	// Update is called once per frame
	void Update ()
    {
        attack_cd -= Time.deltaTime;
        timer_attack -= Time.deltaTime;
        distance = player.transform.position - transform.position;
       //EnemyClose();

    }

    IEnumerator verificaFSM(float waitTime)
    {
        while(true)
        {
            if(state == States.LookingForPlayer && !isDead)
            {
                if (PlayerClose("find"))
                {
                    state = States.Follow;
                }
                else
                {

                }
            }
            else if (state == States.Follow && !isDead)
            {
                if (!PlayerClose("find"))
                {
                    state = States.LookingForPlayer;
                }
                else
                {
                    if(PlayerClose("attack"))
                    {
                        state = States.Attack;
                    }
                    else
                    {
                        Follow();
                    }
                }
            }
            else if(state == States.Attack && !isDead)
            {

                if (!PlayerClose("attack"))
                {
                    state = States.LookingForPlayer;
                }
                else
                {
                    Attack();
                    Follow();

                }

               
            }
            else
            {

                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(waitTime);

        }
    }

    private void EnemyClose()
    {
        GameObject[] gos;
        Vector3 distanceTemp;

        gos = GameObject.FindGameObjectsWithTag("EnemyM");
        GameObject closest = gameObject;
        foreach (GameObject go in gos)
        {
            distanceTemp = transform.position - go.transform.position;

            if (distanceTemp.magnitude <= 2f)
            {
                closest = go;
            }
        }
        if(closest != gameObject)
        {
            if (transform.position.z > closest.transform.position.z)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
      
            if (otherEnemy_pos.x > closest.transform.position.x)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        
        }     
    }
    private bool PlayerClose(string objective)
    {
        float distance_player;
        if(objective == "attack")
        {
            distance_player = player_DistanceToAttack;
            animator.SetBool("Moving", false);

        }
        else
        {
            distance_player = player_Distance;

        }

        
        if (distance.magnitude <= distance_player)
        {
            //gc.enemy[index] = 1;
            enemy_close = true;
            return true;
        }
        else 
        {
           // gc.enemy[index] = 0;
            enemy_close = false;
            return false;
        }
        
    }

    private void Follow()
    {
        /*
        if(!enemy_close && gc.enemies < 2 && timer_attack < 0)
        {
            animator.SetBool("Moving", true);
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (gc.enemies >= 2 || timer_attack > 0 && distance.magnitude <= player_Distance / 4 )
        {
            animator.SetBool("Moving", true);
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.Translate(Vector3.forward * speed * Time.deltaTime * -1);
        }
        */
    }

    private void Attack()
    {

        if (timer_attack < 0)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            animator.SetTrigger("AttackKick1Trigger");
            timer_attack = 7f;

        }
    }
    /*
    private GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        Vector3 distanceTemp;

        gos = GameObject.FindGameObjectsWithTag("EnemyM");
        GameObject closest = gameObject;
        foreach(GameObject go in gos)
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
     
     

        //Debug.Log(closest.nam);

        if (closest != gameObject && gc.enemies > 1)
        {
            enemy_close = true;
        }
        else
        {
            if(gc.enemies <= 0)
            {
                enemy_close = false;

            }
        }


        return closest;
    }
    */

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "EnemyM")
        {
         
        }
    }
}

