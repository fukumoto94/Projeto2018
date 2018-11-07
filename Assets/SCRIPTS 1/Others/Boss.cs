using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    //GameObjects
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject Head;
    public GameObject Eye;
    public GameObject EyeL;
    public Transform middleEyeL;
    public Transform middleEye;
    public GameObject prefab;
    //Disabled
    private bool isDown;
    private bool isDead;
    private float waitingTimer;

    //Player
    //private RPGCharacterControllerFREE player;
    public GameObject player;

    //Start Pos and Rot 
    private Vector3 startPos_LeftH;
    private Vector3 startPos_RightH;
    private Vector3 startPos_Head;
    private Quaternion startRot_LeftH;
    private Quaternion startRot_RightH;
    private Quaternion startRot_Head;

    //Attack
    private float attackCd;
    private int attackRand;
    private bool isAttack;
    private float step;
    public float attackSpeed;
    private float attackTimer0;
    private float attackTimer1;
    private float attackTimer2;

    private float atkAnimTimer;
    private int attackRangeCount;
    private int qtdAttRange;

    //Weakness
    public GameObject[] weakness;
    private int weak_nRand;
    private float timer_weakness;
    public int time_changeWeak;
    private int lastRand;
    public int leftH_hit, rightH_hit, head_hit;
    private bool canHitHead;
    public int player_hit;
    public bool disableL = false;
    public bool disableR = false;
    public bool disableH = false;
    public bool disabled = false;
    public bool weakIsActive = false;
    private float timer_returnBoss = 5;


    //moving
    private bool moving = false;
    public Rigidbody[] chaoDestrutivel;

    //temp

    //boss on
    private bool bossOn  = true;

    private int attackCount  = 0;
    //State Machine
    private States state;
    private float waitTime = 0.01f;


    public enum States
    {
        Waiting,
        Attack,
        Disabled
    }

    // Use this for initialization
    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    void Start()
    {
        qtdAttRange = 5;
        startPos_LeftH = leftHand.transform.position;
        startRot_LeftH = leftHand.transform.rotation;

        startPos_RightH = rightHand.transform.position;
        startRot_RightH = rightHand.transform.rotation;

        startPos_Head = Head.transform.position;
        startRot_Head = Head.transform.rotation;

        state = States.Waiting;
        StartCoroutine(verificaFSM(waitTime));

    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            if (transform.position.x >= -8)
            {
                moving = false;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(-8, transform.position.y, transform.position.z), 0.01f);
            }
        }
        else
        {
            if ((transform.position - player.transform.position).magnitude < 45)
            {
                chaoDestrutivel[0].isKinematic = false;
                chaoDestrutivel[1].isKinematic = false;

                bossOn = true;
            }
            Debug.Log((transform.position - player.transform.position).magnitude);
            if (bossOn)
            {
                if (!isAttack)
                {
                    attackRand = Random.Range(0, 3);
                    waitingTimer -= Time.deltaTime;
                    //leftHand.transform.position = startPos_LeftH;
                    //rightHand.transform.position = startPos_RightH;
                }
                else
                {
                    step = .5f;
                    attackTimer0 -= Time.deltaTime;
                    attackTimer1 -= Time.deltaTime;
                    attackTimer2 -= Time.deltaTime;

                    if (attackRangeCount == 0)
                    {
                        atkAnimTimer -= Time.deltaTime;
                    }

                }
                if (attackCount > 2)
                {
                    attackCount = 0;
                }
                Eye.transform.LookAt(Vector3.Lerp(Eye.transform.position, player.transform.position, 0.00001f));
                EyeL.transform.LookAt(Vector3.Lerp(EyeL.transform.position, player.transform.position, 0.00001f));


                //weakness
                if (!canHitHead)
                {
                    weakness[2].SetActive(false);

                    if (weakIsActive)
                    {
                        if (!disableL && !disableR)
                        {
                            weak_nRand = Random.Range(0, 2);
                        }
                        else if (!disableL)
                        {
                            weak_nRand = 0;
                        }
                        else
                        {
                            weak_nRand = 1;
                        }

                        if (timer_weakness < time_changeWeak)
                        {
                            timer_weakness += Time.deltaTime;
                        }
                        else
                        {
                            weakIsActive = false;
                        }
                    }
                    else
                    {
                        if (timer_weakness > 0)
                        {
                            weakness[weak_nRand].SetActive(true);
                            timer_weakness -= Time.deltaTime;

                        }
                        else
                        {
                            weakness[weak_nRand].SetActive(false);
                            weakIsActive = true;
                        }
                    }
                }
                else
                {
                    weakness[2].SetActive(true);
                }

                if (disableL && disableR)
                {
                    canHitHead = true;
                }
                else
                {
                    canHitHead = false;
                }
                ///disable parts
                if (leftH_hit >= player_hit)
                {
                    weakness[0].SetActive(false);
                    disableL = true;
                }
                if (rightH_hit >= player_hit)
                {
                    weakness[1].SetActive(false);
                    disableR = true;
                }
                if (head_hit >= player_hit)
                {
                    weakness[2].SetActive(false);
                    disableH = true;
                }

                if (disableH)
                {
                    timer_returnBoss -= Time.deltaTime;
                    if (timer_returnBoss < 0)
                    {
                        weakness[0].SetActive(false);
                        weakness[1].SetActive(false);
                        weakness[2].SetActive(false);
                        leftH_hit = 0;
                        rightH_hit = 0;
                        head_hit = 0;
                        disableL = false;
                        disableR = false;
                        disableH = false;
                    }

                }
                else
                {
                    timer_returnBoss = 5;
                }

            }
            else
            {

            }

            //Debug.Log(leftH_hit +"   <left     right>  "+rightH_hit+"  head>"+head_hit);
        }

    }

    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if (!isDead)
            {
                if (state == States.Waiting)
                {
                    ResetBoss();
                    if (waitingTimer <= 0)
                    {
                        attackCount++;
                        state = States.Attack;
                    }

                }
                else if (state == States.Attack)
                {
                    Attack(attackCount);

                    if (atkAnimTimer <= 0 || attackRangeCount > (qtdAttRange - 1) || Disabled())
                    {
                        waitingTimer = 5f;
                        state = States.Waiting;
                    }

                }
            }
            else
            {

            }
            yield return new WaitForSeconds(waitTime);

        }
    }

    private void Waiting()
    {
 

    }

    private void Attack(int a)
    {
        isAttack = true;
        if (a == 0)
        {
            if(disableL)
            {
                leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, new Vector3(startPos_LeftH.x, player.transform.position.y, startPos_LeftH.z), step);
                a = 1;
            }
            else
            {
                //rightHand.transform.position = startPos_RightH;
                rightHand.GetComponent<Rigidbody>().isKinematic = true;
                if (attackTimer0 <= 0)
                {
                    leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, new Vector3(leftHand.transform.position.x, player.transform.position.y, leftHand.transform.position.z), step);
                }
                else
                {
                    leftHand.transform.position = Vector3.Lerp(leftHand.transform.position, new Vector3(player.transform.position.x, leftHand.transform.position.y, player.transform.position.z), 0.02f);
                    //leftHand.transform.position = Vector3.ler
                }
            }
        }
        else if (a == 1)
        {
            if(disableR)
            {
                rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, new Vector3(startPos_RightH.x, player.transform.position.y, startPos_RightH.z), step);
                a = 2;
            }
            else
            {
                //leftHand.transform.position = startPos_LeftH;
                leftHand.GetComponent<Rigidbody>().isKinematic = true;

                if (attackTimer1 <= 0)
                {
                    rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, new Vector3(startPos_RightH.x + 40f, rightHand.transform.position.y, rightHand.transform.position.z), step);
                }
                else
                {
                    rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, new Vector3(startPos_RightH.x - 1f, rightHand.transform.position.y, player.transform.position.z), step);
                }
            }
           
        }
        else
        {
            if(disableH)
            {
                Head.transform.position = Vector3.MoveTowards(Head.transform.position, new Vector3(Head.transform.position.x, player.transform.position.y, Head.transform.position.z), step);
            }
            else
            {
                rightHand.GetComponent<Rigidbody>().isKinematic = true;
                leftHand.GetComponent<Rigidbody>().isKinematic = true;

                if (attackRangeCount < qtdAttRange && attackTimer2 < 0)
                {
                    Instantiate(prefab, middleEye.position, middleEye.rotation);
                    Instantiate(prefab, middleEyeL.position, middleEyeL.rotation);

                    attackRangeCount++;
                    attackTimer2 = 2f;
                }
            }
       
        }
    }
    private void ResetBoss()
    {
        isAttack = false;
        attackTimer0 = 3f;
        attackTimer1 = 3f;
        attackTimer2 = 0f;
        attackRangeCount = 0;
        atkAnimTimer = 5f;
        rightHand.GetComponent<Rigidbody>().isKinematic = false;
        leftHand.GetComponent<Rigidbody>().isKinematic = false;
        if (!disableR)
        {
            rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, startPos_RightH, step);
            rightHand.transform.rotation = Quaternion.RotateTowards(rightHand.transform.rotation, startRot_RightH, step);
        }
        else
        {
            rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, new Vector3(startPos_RightH.x, player.transform.position.y, startPos_RightH.z), step);

        }
        if (!disableL)
        {
            leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, startPos_LeftH, step);
            leftHand.transform.rotation = Quaternion.RotateTowards(leftHand.transform.rotation, startRot_LeftH, step);
        }
        else
        {
            leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, new Vector3(startPos_LeftH.x, player.transform.position.y, startPos_LeftH.z), step);

        }
        if (!disableH)
        {
            Head.transform.position = Vector3.MoveTowards(Head.transform.position, startPos_Head, step);
            Head.transform.rotation = Quaternion.RotateTowards(Head.transform.rotation, startRot_Head, step);
        }
        else
        {
            Head.transform.position = Vector3.MoveTowards(Head.transform.position, new Vector3(Head.transform.position.x, player.transform.position.y, Head.transform.position.z), step);
        }





    }

    private bool Disabled()
    {
        if(attackCount == 0)
        {
            return disableL;

        }
        else if(attackCount == 1)
        {
            return disableR;

        }
        else
        {
            return disableH;

        }
    }

}
