using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public enum Weapon 
{
	UNARMED = 0,
	RELAX = 8
}
	
public class RPGCharacterControllerFREE : MonoBehaviour 
{
    private bool arcoTemp;

    private Vector3 freezePos;
    public bool givingItem;

    private bool enemyGhit;
    public Material redDmg;
    private Color initialColor;

    //particula
    public GameObject dmgParticle;

    //public GameObject dmgParticleFoot;
    private float timerParticle;
    public float durationParticle;
    private bool isHit;

    public GameObject swordBack;
    public GameObject bowBack;
    public GameObject arrowTempGo;
    private bool arrowTemp;

    public bool itemPick;
    public Transform deathCamera;
    private bool alreadyCharg;
    private float timerItem = 3f;

    #region Variables
   

	//Components
	Rigidbody rb;
	protected Animator animator;
	public GameObject[] target;
	private Vector3 targetDashDirection;
    private bool canDash = true;
    bool change = true;

    //camera
    private OrbitCamera oc;

	//jumping variables
	public float gravity = -9.8f;
	bool canJump;
	bool isJumping = false;
	public bool isGrounded;
	public float jumpSpeed = 12;
	public float doublejumpSpeed = 12;
	bool doublejumping = true;
	bool canDoubleJump = false;
	bool isDoubleJumping = false;
	bool doublejumped = false;
	bool isFalling;
	bool startFall;
	float fallingVelocity = -1f;
    int coutJump = 0;
    private float timer_AutoJump;

    // Used for continuing momentum while in air
    public float inAirSpeed = 8f;
	float maxVelocity = 2f;
	float minVelocity = -2f;

	//rolling variables
	public float rollSpeed = 8;
	public bool isRolling = false;
	public float rollduration;

	//movement variables
	bool canMove = true;
	public float walkSpeed = 1.35f;
	public float moveSpeed;
	public float runSpeed = 6f;
	public float rotationSpeed = 20f;
    private string direction;
    public bool isMoving;
    public bool changeScene;
	float x;
	float z;
	Vector3 m_forward;
    Vector3 m_right;
	Vector3 newVelocity;

	//Weapon and Shield
	private Weapon weapon;
	int rightWeapon = 0;
	int leftWeapon = 0;
	bool isRelax = false;
    public bool isArmed;

	//isStrafing/action variables
	bool canAction = true;
	public bool isStrafing = false;
	bool isDead = false;
	bool isBlocking = false;
	public float knockbackMultiplier = 1f;
	bool isKnockback;
    public Quaternion targetRotation;

    //camera bow
    public Transform focus_object;

    //range attack
    float fireRate = 1f;
    public Transform arrow_prefab;
    public Transform arrow_out;
    public Transform bow;
    public bool bow_mode = false;
    public bool bowOn = false;

    //attack variables
    public Transform sword;
    int attack_comb;
    bool attack_right = true;
    public float attack_time;
    public float attack_animatTime;
    private float timer_trailRender;
    bool[] attack = new bool[3];

    //moving object
    public bool object_moving;

    //camera shake
    public bool camera_shake;
    public float shake_time;

    //bloodScreen
    public GameObject bloodScreen;
    private float blood_time;
    private bool bloodHit;
   
    //climbing stairs
    public int climbing;
    private float climbY;
    private float climbZ;
    private bool climbed;
    private bool climbAux;
    //bridge
    public bool bridge_mode = false;

    //enemy

    //grab
    private bool isGrab = false;
    private bool canGrab = false;
    private float timer_toFall;
    //climb
    private bool climb = false;
    private bool isClimb = false;
    private bool canClimb = false;
    private Vector3 pos_Start_MoveUp = Vector3.zero;
    private Transform direction_MoveClimb;
    private Transform direction_MoveGrab;
    public bool reachTop =false;
    public float range_MoveUp = 0.5f;

    public Vector3 focusTemp;

    //health
    public Image hpImage;
    public float hp;
    public float currentHp;

    //npc
    private bool moveAfterNpc;
    #endregion

    private bool climbout;

    //audio
    public AudioSource[] source;

    //particle
    public Transform[] footParticle;
    private int ftParticle;
    //cameraDoor
    public bool isDoor;
    private bool alreadyClose;

    //minimapa
    public bool mapaOn = false;
    public bool mapOnOff;
    public GameObject miniMapa;
    public GameObject compass;

    #region Initialization
    void Awake()
    {
        initialColor = redDmg.color;
        oc = GameObject.FindGameObjectWithTag("OrbitCamera").GetComponent<OrbitCamera>() as OrbitCamera;
        if (SceneManager.GetActiveScene().name == "Forest 1")
        {
            ftParticle = 0;
        }
        else
        {
            ftParticle = 1;
        }
        attack[0] = true;
        attack[1] = false;
        attack[2] = false;

    }

    void Start() 
	{
        // Cursor.visible = false;
        //set the animator component
        currentHp = hpImage.fillAmount;
        animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody>();
	}

	#endregion
	
	#region UpdateAndInput
	
	void Update()
	{
        //comando facilitar
        Cheat();

        if (currentHp > 100)
        {
            currentHp = 100;
        }

        if(currentHp <= 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                isMoving = true;

                animator.SetTrigger("Death");
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.2f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    currentHp = 100;
                }
            }
          
        }
        else
        {
            particleOn();
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque2") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque3"))
            {
         
                sword.GetComponent<Collider>().enabled = false;


            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                {
                    sword.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    sword.GetComponent<Collider>().enabled = true;

                }


            }

            target = GameObject.FindGameObjectsWithTag("Target");

            if (!oc.isNpc && !isDoor && !oc.isPick && !oc.startBoss && !oc.orbOn)
            {
                freezePos = transform.position;
                animator.SetBool("ItemDrop", itemPick);
                givingItem = false;


                BloodScreen();
                if (miniMapa != null)
                {
                    Minimapa();

                }

                if (moveAfterNpc)
                {
                    rb.isKinematic = false;
                    moveAfterNpc = false;
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    GetDamage(100);
                }
                hpImage.fillAmount = currentHp;



                // Debug.Log(bridge_mode);
                //  Debug.Log("Climb: "+isClimb + "  Dash: "+canDash+"  Grab: "+isGrab);
                //timers
                timer_toFall -= Time.deltaTime;
                timer_AutoJump -= Time.deltaTime;
                if (timer_trailRender >= 0)
                {
                    timer_trailRender -= Time.deltaTime;
                }
                fireRate -= Time.deltaTime;
                //make sure there is animator on character
                if (animator)
                {
                    if (canMove && !isBlocking && !isDead && !isClimb && !isMoving && !changeScene)
                    {
                        MovementInput();
                    }
                    //Rolling();
                    //Jumping();

                    Dashing();
                    Climbing();
                    Grabbing();
                    if (Input.GetMouseButton(1) && bowOn)
                    {
                        transform.LookAt(new Vector3(focus_object.position.x, transform.position.y, focus_object.position.z));

                        // transform.LookAt(focus_object);

                        bow_mode = true;
                    }
                    else
                    {
                        bow_mode = false;
                    }


                    if (bow_mode)
                    {
                        animator.SetBool("isBow", true);
                        RangedAttack();
                    }
                    else
                    {
                        animator.SetBool("isBow", false);
                        MeleeAttack();
                    }


                    if (isArmed)
                    {
                        sword.gameObject.SetActive(!bow_mode);

                    }
                    BackItensController();

                    bow.gameObject.SetActive(bow_mode);
                    focus_object.gameObject.SetActive(bow_mode);

                    if (Input.GetButtonDown("LightHit") && canAction && isGrounded && !isBlocking)
                    {
                        GetHit();
                    }
                    if (Input.GetButtonDown("Death") && canAction && isGrounded && !isBlocking)
                    {
                        if (!isDead)
                        {
                            StartCoroutine(_Death());
                        }
                        else
                        {
                            StartCoroutine(_Revive());
                        }
                    }
                    if (Input.GetButtonDown("AttackL") && canAction && isGrounded && !isBlocking)
                    {
                        Attack(1);
                    }
                    if (Input.GetButtonDown("AttackR") && canAction && isGrounded && !isBlocking)
                    {
                        Attack(2);
                    }
                    if (Input.GetButtonDown("CastL") && canAction && isGrounded && !isBlocking && !isStrafing)
                    {
                        AttackKick(1);
                    }
                    if (Input.GetButtonDown("CastR") && canAction && isGrounded && !isBlocking && !isStrafing)
                    {
                        AttackKick(2);
                    }
                    //target focus

                    //if strafing
                    //if(Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("TargetBlock") > .1 && canAction)
                    //if (Input.GetKey(KeyCode.LeftShift) && canAction)
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        isStrafing = true;
                         animator.SetBool("Strafing", true);
                        if (Input.GetButtonDown("CastL") && canAction && isGrounded && !isBlocking)
                        {
                            CastAttack(1);
                        }
                        if (Input.GetButtonDown("CastR") && canAction && isGrounded && !isBlocking)
                        {
                            CastAttack(2);
                        }
                    }
                    else
                    {
                        isStrafing = false;
                            animator.SetBool("Strafing", false);
                    }

                }
                else
                {
                    Debug.Log("ERROR: There is no animator for character.");
                }
            }
            else
            {
      
                if(!oc.startBoss && !oc.orbOn)
                {
                    if (oc.isPick)
                    {

                        if (SceneManager.GetActiveScene().name == "Temple 1")
                        {
                            bowOn = true;
                        }
                        isArmed = true;

                        transform.eulerAngles = new Vector3(0, 180, 0);

                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ItemPickUp") && timerItem >= 3f)
                        {
                            sword.gameObject.SetActive(false);

                            givingItem = true;
                            animator.SetBool("ItemPick", true);

                        }

                        else
                        {
                            if (timerItem < 0f)
                            {

                                animator.SetBool("ItemPick", false);

                                oc.isPick = false;
                            }
                        }

                        timerItem -= Time.deltaTime;

                    }
                    if (!isDoor && !oc.isPick)
                    {
                        moveAfterNpc = true;

                        transform.LookAt(new Vector3(GameObject.FindGameObjectWithTag("Npc").transform.position.x, transform.position.y, GameObject.FindGameObjectWithTag("Npc").transform.position.z));
                    }
                    moveSpeed = 0;

                    rb.isKinematic = isGrounded;
                }
                else
                {
                    transform.position = freezePos;
                    isMoving = true;


                }

            }

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

    void BackItensController()
    {
        if(itemPick)
        {
            swordBack.SetActive(true);
            bowBack.SetActive(true);
            sword.gameObject.SetActive(false);
            bow.gameObject.SetActive(false);
        }
        else
        {
            swordBack.SetActive(bow_mode);
            if(bowOn)
            {
                bowBack.SetActive(!bow_mode);
            }
            else
            {
                bowBack.SetActive(false);

            }
        }
  
    }

    void Cheat()
    {

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            runSpeed += 5;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            runSpeed = 10;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetDamage(-10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Forest 1");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("Ponte");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("Temple 1");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("Temple 2");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("BossArenaff");
        }
    }

    void Minimapa()
    {
        if (SceneManager.GetActiveScene().name != "Forest 1"  && SceneManager.GetActiveScene().name != "BossArenaff")
        {
            if (mapaOn && Input.GetKeyDown(KeyCode.Tab))
            {
                mapOnOff = !mapOnOff;
            }

            miniMapa.SetActive(mapOnOff);
        }  
    }
    void MovementInput()
	{
        Vector3 front;
        Vector3 right;

        if (bridge_mode)
        {

            front = transform.forward;
            right = -transform.right;
        }
        else
        {

            front = new Vector3(focus_object.forward.x, 0, focus_object.forward.z);
            right = new Vector3(focus_object.right.x, 0, focus_object.right.z);
        }


        //focus_object
        x = Input.GetAxisRaw("Horizontal");
		z = Input.GetAxisRaw("Vertical");

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque1") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque2") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque3"))
        {
            m_forward = z * front;
            m_right = x * right;


        }
        else
        {

            m_forward = (z * front)/2;
            m_right = (x * right)/2 ;
        }
   
	}
	
	#endregion

	#region Fixed/Late Updates
	
	void FixedUpdate()
	{
		CheckForGrounded();
		//apply gravity force
		rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
		//AirControl();
		//check if character can move
		if(canMove && !isBlocking && !isMoving && !changeScene)
		{
			moveSpeed = UpdateMovement();  
		}
        else
        {
            moveSpeed = 0;
        }
		//check if falling
		if(rb.velocity.y < fallingVelocity)
		{
  
      
            isFalling = true;
			animator.SetInteger("Jumping", 2);
			canJump = false;
		} 
		else
		{
			isFalling = false;
		}
	}

	//get velocity of rigid body and pass the value to the animator to control the animations
	void LateUpdate()
	{
        int nrand = 0;
        //Get local velocity of charcter
        float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
		float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
		//Update animator with movement values
		animator.SetFloat("Velocity X", velocityXel / runSpeed);
		animator.SetFloat("Velocity Z", velocityZel / runSpeed);
		//if character is alive and can move, set our animator
		if(!isDead && canMove)
		{
         
            if (moveSpeed > 0 && !isClimb && isGrounded)
			{
             
                footParticle[ftParticle].GetComponent<ParticleSystem>().enableEmission = true;
                if(!oc.isBlocking)
                {
                    oc.ZoomIn();

                    //  oc.ZoomOut();

                }

                if(!isClimb)
                {
                    rb.isKinematic = false;
                }

                animator.SetBool("Moving", true);
			}
			else
			{

                footParticle[ftParticle].GetComponent<ParticleSystem>().enableEmission = false;

                if (!oc.isBlocking)
                {
                    oc.ZoomIn();
                }
                if(!canClimb)
                {
                    rb.isKinematic = isGrounded;
                }
                animator.SetBool("Moving", false);
			}
		}
	}
	
	#endregion

	#region UpdateMovement

	//rotate character towards direction moved
	void RotateTowardsMovementDir()
	{
        //!object_moving
        if (true && !bridge_mode)
        {
            if (m_forward != Vector3.zero && !isStrafing && !isRolling && !isBlocking )
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_forward), Time.deltaTime * rotationSpeed);
            }
            if (m_right != Vector3.zero && !isStrafing && !isRolling && !isBlocking)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_right), Time.deltaTime * rotationSpeed);
            }
        }
      
    }

	float UpdateMovement()
	{
        Vector3 teste = m_right + m_forward;

        //GetCameraRelativeMovement();  
        if (isGrounded)
		{

            if (canMove && !isBlocking)
			{
                //set speed by walking / running
                if (isStrafing)
				{
                    if (m_right.magnitude != 0 && m_forward.magnitude != 0)
                    {
                        newVelocity = (teste * walkSpeed);
                    }
                    else
                    {
                        newVelocity = (teste * runSpeed);
                    }

                }

                else
				{
                    if (m_right.magnitude != 0 && m_forward.magnitude != 0)
                    {
                        newVelocity = (teste * walkSpeed) ;

                    }
                    else
                    {
                        newVelocity = (teste * runSpeed);
                    }

                }
                //if rolling use rolling speed and direction
                if (isRolling)
				{
					//force the dash movement to 1
					targetDashDirection.Normalize();
					newVelocity = rollSpeed * targetDashDirection;
				}
			}
		}
		else
		{
			//if we are falling use momentum
			newVelocity = rb.velocity;
		}
		if(!isStrafing || !canMove)
		{
			RotateTowardsMovementDir();
		}
		if(isStrafing && !isRelax)
		{
            focusTemp = Vector3.zero;
            foreach(GameObject focus in target)
            {
                if(focus != null)
                {
                    if ((focusTemp - transform.position).magnitude > (focus.transform.position - transform.position).magnitude)
                    {
                        focusTemp = focus.transform.position;
                    }
                }
            
            }


			//make character point at target
      	    targetRotation = Quaternion.LookRotation(focusTemp - new Vector3(transform.position.x,0,transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,(rotationSpeed * Time.deltaTime) * rotationSpeed);
		}
		newVelocity.y = rb.velocity.y;
		rb.velocity = newVelocity;


        return teste.magnitude;



    }

    //converts control input vectors into camera facing vectors
    void GetCameraRelativeMovement()
	{  
		Transform cameraTransform = Camera.main.transform;
		//Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(transform.forward);
        forward.y = 0;
        forward = transform.forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.x, 0, -forward.x);
		//directional inputs
		float dv = Input.GetAxisRaw("Vertical");
		float dh = Input.GetAxisRaw("Horizontal");
		if(!isRolling)
		{
			targetDashDirection = dh * right + dv * -forward;
		}
	}

	#endregion

	#region Jumping

	//checks if character is within a certain distance from the ground, and markes it IsGrounded
	void CheckForGrounded()
	{
        float distanceToGround;
		float threshold = 1f;
		RaycastHit hit;
		Vector3 offset = new Vector3(0,.4f,0);
		if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
		{
            distanceToGround = hit.distance;
			if(distanceToGround < threshold)
			{
				isGrounded = true;
				canJump = true;
				startFall = false;
				doublejumped = false;
				canDoubleJump = false;
				isFalling = false;
				if(!isJumping) 
				{
					animator.SetInteger("Jumping", 0);
				}
			}
			else
			{
                isGrounded = false;

            }
        }

        if(!isGrounded)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpTrigger"))
            {
                animator.ResetTrigger("Charging");
                animator.ResetTrigger("Return");

                animator.SetTrigger("JumpTrigger");
            }
        }
	}
    #region Action

    void Grabbing()
    {
        if (canGrab && Input.GetKey(KeyCode.Space) && !isClimb && !isRolling)
        {
            isGrab = true;
            transform.LookAt(new Vector3(direction_MoveGrab.position.x, transform.position.y, direction_MoveGrab.position.z));
            timer_toFall = 2f;
          //  rb.isKinematic = true;

        }
        else
        {
            canGrab = false;
            isGrab = false;
        }
        if(!isGrab)
        {
          //  rb.isKinematic = false;
        }

    }
    void Dashing()
    {
        if(canDash && !canClimb && !isFalling && !canClimb && !isRolling)
        {
            if ((Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
             && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                if(isStrafing)
                {
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        targetDashDirection = transform.right;
                        StartCoroutine(_Roll(2));
                    }
                    else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        targetDashDirection = -transform.right;
                        StartCoroutine(_Roll(4));
                    }
                    else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        targetDashDirection = -transform.forward;
                        StartCoroutine(_Roll(3));
                    }
                    else
                    {
                        targetDashDirection = transform.forward;
                        StartCoroutine(_Roll(1));

                    }
                }

                else
                {
                    targetDashDirection = transform.forward;
                    StartCoroutine(_Roll(1));

                }
            }
        }
    }
    void Climbing()
    {
        float x = 0;
        float y = 0;

        if(!climb)
        {
            canClimb = false;
            canDash = true;
            isClimb = false;
        }
       

        if (canClimb && Input.GetKeyDown(KeyCode.Space) && !isGrab && !isRolling)
        {
            isClimb = !isClimb;
            rb.isKinematic = isClimb;
       
        }


      
        if (isClimb)
        {
            animator.SetBool("Climbing", true);

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Climb") && !climbAux)
            {
                animator.SetTrigger("Climb");
                climbAux = true;
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Climb") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {

                    climbAux = false;

                }
            }

            transform.LookAt(new Vector3(transform.position.x, transform.position.y, direction_MoveClimb.transform.position.z));

  
            if (Input.GetAxis("Vertical") > 0)
            {
                animator.speed = 1;

                y += 0.05f / 2;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                animator.speed = 1;

                if (transform.position.y >= -1.37f)
                {


                    y -= 0.05f/2;


                }

            }
       
            if (Input.GetAxis("Horizontal") > 0)
            {
                animator.speed = 1;

                x += 0.05f / 2;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                animator.speed = 1;

                x -= 0.05f / 2;
            }
      
            if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 )
            {
                animator.speed = 0;
            }

            if (transform.position.y > 1.2f)
            {
                reachTop = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + x, transform.position.y + y, transform.transform.position.z), 0.2f);
                reachTop = false;
            }

            if (reachTop)
            {
                
                animator.SetTrigger("ClimbOut");
                isMoving = true;
                climbY = transform.position.y;
                climbZ = transform.position.z;
                isClimb = false;
            }

        }
        else
        {
            animator.speed = 1;

            if(reachTop)
            {
                climbY = Mathf.Lerp(climbY, 3f, 0.5f);

                if (climbY == 3f)
                {
                    climbZ = Mathf.Lerp(climbZ, -2.8f, 0.02f);

                }
                transform.position = new Vector3(transform.position.x, climbY, climbZ);
              //  Debug.Log(climbY + "   y    z  " + climbZ);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("ClimbOut"))
                {

                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        isMoving = false;
                        climbed = true;
                        reachTop = false;

                    }
                }
            }
            else
            {
                if(!climbed)
                {
                    climbAux = false;
                    animator.SetBool("Climbing", false);
                    reachTop = false;
                }
                
            }


           

        }
  


    }
    void Jumping()
	{
        if (isGrounded)
		{
            if (canJump && Input.GetButtonDown("Jump"))
            {
                StartCoroutine(_Jump());
            }
            coutJump = 0;
        }
		else
		{
            if (Input.GetButtonDown("Jump") && coutJump < 1)
            {
                if (canDoubleJump && !isDoubleJumping)
                {
                    StartCoroutine(_Jump());
                    coutJump++;
                }
            }
           
            canDoubleJump = true;
			canJump = false;
			if(isFalling)
			{
                isClimb = false;
				//set the animation back to falling
				animator.SetInteger("Jumping", 2);
				//prevent from going into land animation while in air
				if(!startFall)
				{
					//animator.SetTrigger("JumpTrigger");
					startFall = true;
				}
			}
            if (canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling)
			{
				// Apply the current movement to launch velocity
				rb.velocity += doublejumpSpeed * Vector3.up;
				animator.SetInteger("Jumping", 3);
				doublejumped = true;
			}
		}
	}
    #endregion
    void RangedAttack()
    {
       
        bool aux = false;
        if (Input.GetMouseButton(0))
        {
            arrowTemp = true;
            aux = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            arrowTemp = false;
            aux = false;
            arcoTemp = false;
            animator.SetTrigger("Return");
        }
        if (aux)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Charging") && !arcoTemp)
            {
                animator.SetTrigger("Charging");
                arcoTemp = true;
            }
        }
        arrowTempGo.SetActive(arrowTemp);
        animator.SetBool("chargOff", !aux);
        Debug.Log(arrowTemp);

    }

    void MeleeAttack()
    {
        if (isArmed)
        {
            int rand = Random.Range(0, 7);
            attack_animatTime -= Time.deltaTime;

            sword.GetComponent<TrailRenderer>().time = timer_trailRender;

            if (Input.GetMouseButtonDown(0) && !isKnockback && attack_animatTime <= 0)
            {
  
           
                timer_trailRender = 1f;
                attack_animatTime = 0.4f;

                if (attack[0])
                {

                    if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque1"))
                    {
                        attack[2] = false;
                        attack_time = 1f;
                        Attack(1);
                    }
                    else
                    {
                        attack[1] = true;

                    }
                }
                if (attack[1] && attack_time > 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque2"))
                    {
                        attack[0] = false;
                        attack_time = 1f;
                        Attack(2);

                    }
                    else
                    {
                        attack[2] = true;

                    }
                }
                if (attack[2] && attack_time > 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque3"))
                    {

                        attack[1] = false;
                        AttackKick(1);
                    }
                    else
                    {

                        attack[0] = true;


                    }
                }
                source[rand].Play();

            }
         
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("ataque3"))
            {
                isMoving = false;
           
            }
            else
            {

                isMoving = true;


            }
            if (attack_time >= 0)
            {

                attack_time -= Time.deltaTime;
            }
            else
            {

                attack[0] = true;
                attack[1] = false;
                attack[2] = false;

            }

  

        }
        //unarmed attack
        else
        {
            //attack_animatTime -= Time.deltaTime;

        }

    }

	IEnumerator _Jump()
	{
		isJumping = true;
		animator.SetInteger("Jumping", 1);
		//animator.SetTrigger("JumpTrigger");
		// Apply the current movement to launch velocity
		rb.velocity += jumpSpeed * Vector3.up;
        canJump = false;
		yield return new WaitForSeconds(.5f);
		isJumping = false;
	}

	void AirControl()
	{
		if(!isGrounded)
		{
			MovementInput();
			Vector3 motion = m_forward;
			//motion *= (Mathf.Abs(m_forward.x) == 1 && Mathf.Abs(m_forward.z) == 1)?.7f:1;
			rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
			//limit the amount of velocity we can achieve
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > maxVelocity)
			{
				velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
				if(velocityX < 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity)
			{
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > maxVelocity)
			{
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity)
			{
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}
	}

    #endregion

    #region Health
    public void GetDamage(float damage)
    {
        currentHp -= damage / hp;
    }
    public void RecoverHP(float hpRec)
    {
        currentHp += hpRec / hp;
    }
    #endregion

    #region MiscMethods

    //0 = No side
    //1 = Left
    //2 = Right
    //3 = Dual
    void Attack(int attackSide)
	{
        int attackNumber = 0;
        if (attackSide == 1)
        {
            attackNumber = 3;
        }
        else if (attackSide == 2)
        {
            attackNumber = 6;
        }
        if (isGrounded)
        {
            animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
            StartCoroutine(_LockMovementAndAttack(0, 0.7f));
        }
    }

	void AttackKick(int kickSide)
	{
		if(isGrounded)
		{
			if(kickSide == 1)
			{
				animator.SetTrigger("AttackKick1Trigger");
            }
            else
			{
				animator.SetTrigger("AttackKick2Trigger");
			}
            StartCoroutine(_LockMovementAndAttack(0, 1.1f));
		}
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	void CastAttack(int attackSide)
	{
		if(weapon == Weapon.UNARMED)
		{
			int maxAttacks = 3;
			if(attackSide == 1)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, .8f));
				}
			}
			if(attackSide == 2)
			{
				int attackNumber = Random.Range(3, maxAttacks + 3);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, .8f));
				}
			}
			if(attackSide == 3)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastDualAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, 1f));
				}
			}
		} 
	}
    void BloodScreen()
    {
        if(bloodHit)
        {
            // SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            //  SpriteRenderer.color = new Color(1f, 1f, 1f, .5f);

            blood_time -= Time.deltaTime;
        }

        if(blood_time < 0)
        {
            blood_time = 0;
            bloodHit = false;
        }
        bloodScreen.GetComponent<Image>().color = new Color(1f, 1f, 1f, blood_time);

        //bloodScreen.SetActive(bloodHit);

    }
    public void GetHit()
	{
        
        isHit = true;
        if(!isRolling)
        {
            bloodHit = true;
            blood_time = 1f;
            camera_shake = true;
            int hits = 5;
            int hitNumber = Random.Range(0, hits);
            source[7].Play();
            if(!bow_mode)
            {
                animator.SetTrigger("GetHit" + (hitNumber + 1).ToString() + "Trigger");

            }
            StartCoroutine(_LockMovementAndAttack(.1f, .4f));
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
        }
       
	}

	IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		isKnockback = true;
		StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
		yield return new WaitForSeconds(.1f);
		isKnockback = false;
	}

	IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		while(isKnockback)
		{
			rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
			yield return null;
		}
	}

	IEnumerator _Death()
	{
		animator.SetTrigger("Death1Trigger");
		StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
        isDead = true;
		animator.SetBool("Moving", false);
        m_forward = new Vector3(0, 0, 0);
		yield return null;
	}

	IEnumerator _Revive()
	{
        animator.SetTrigger("Revive1Trigger");
		isDead = false;
		yield return null;
	}

	#endregion

	#region Rolling
/*
	void Rolling()
	{
		if(!isRolling && isGrounded)
		{
			if(Input.GetAxis("DashVertical") > .5 || Input.GetAxis("DashVertical") < -.5 || Input.GetAxis("DashHorizontal") > .5 || Input.GetAxis("DashHorizontal") < -.5)
			{
				StartCoroutine(_DirectionalRoll(Input.GetAxis("DashVertical"), Input.GetAxis("DashHorizontal")));
			}
		}
	}

	public IEnumerator _DirectionalRoll(float x, float v)
	{
		//check which way the dash is pressed relative to the character facing
		float angle = Vector3.Angle(targetDashDirection,-transform.forward);
		float sign = Mathf.Sign(Vector3.Dot(transform.up,Vector3.Cross(targetDashDirection,transform.forward)));
		// angle in [-179,180]
		float signed_angle = angle * sign;
		//angle in 0-360
		float angle360 = (signed_angle + 180) % 360;
		//deternime the animation to play based on the angle
		if(angle360 > 315 || angle360 < 45)
		{
			StartCoroutine(_Roll(1));
		}
		if(angle360 > 45 && angle360 < 135)
		{
			StartCoroutine(_Roll(2));
		}
		if(angle360 > 135 && angle360 < 225)
		{
			StartCoroutine(_Roll(3));
		}
		if(angle360 > 225 && angle360 < 315)
		{
			StartCoroutine(_Roll(4));
		}
		yield return null;
	}
    */
	IEnumerator _Roll(int rollNumber)
	{
		if(rollNumber == 1)
		{
			animator.SetTrigger("RollForwardTrigger");
		}
		if(rollNumber == 2)
		{
			animator.SetTrigger("RollRightTrigger");
		}
		if(rollNumber == 3)
		{
			animator.SetTrigger("RollBackwardTrigger");
		}
		if(rollNumber == 4)
		{
			animator.SetTrigger("RollLeftTrigger");
		}
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        GetComponent<CapsuleCollider>().height = 1f;
        isRolling = true;
		yield return new WaitForSeconds(rollduration);
		isRolling = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        GetComponent<CapsuleCollider>().center = new Vector3(0, 1.25f, 0);
        GetComponent<CapsuleCollider>().height = 2.5f;

    }

    #endregion

    #region _Coroutines

    //method to keep character from moveing while attacking, etc
    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
	{
		yield return new WaitForSeconds(delayTime);
		canAction = false;
		canMove = false;
		animator.SetBool("Moving", false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
        m_forward = new Vector3(0, 0, 0);
		animator.applyRootMotion = true;
		yield return new WaitForSeconds(lockTime);
		canAction = true;
		canMove = true;
		animator.applyRootMotion = false;
	}
	
	#endregion

	#region GUI
    /*
	void OnGUI()
	{
		if(!isDead)
		{
			if(canAction && !isRelax)
			{
				if(isGrounded)
				{
					if(!isBlocking)
					{
						if(!isBlocking)
						{
							if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward"))
							{
								targetDashDirection = transform.forward;
								StartCoroutine(_Roll(1));
							}
							if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward"))
							{
								targetDashDirection = -transform.forward;
								StartCoroutine(_Roll(3));
							}
							if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left"))
							{
								targetDashDirection = -transform.right;
								StartCoroutine(_Roll(4));
							}
							if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right"))
							{
								targetDashDirection = transform.right;
								StartCoroutine(_Roll(2));
							}
							//ATTACK LEFT
							if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L"))
							{
								Attack(1);
							}
							//ATTACK RIGHT
							if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R"))
							{
								Attack(2);
							}
							if(weapon == Weapon.UNARMED) 
							{
								if(GUI.Button (new Rect (25, 115, 100, 30), "Left Kick")) 
								{
									AttackKick (1);
								}
								if(GUI.Button (new Rect (130, 115, 100, 30), "Right Kick")) 
								{
									AttackKick (2);
								}
							}
							if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
							{
								GetHit();
							}
						}
					}
				}
				if(canJump || canDoubleJump)
				{
					if(isGrounded)
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Jump"))
						{
							if(canJump && isGrounded)
							{
								StartCoroutine(_Jump());
							}
						}
					} 
					else
					{
						if(GUI.Button(new Rect(25, 165, 100, 30), "Double Jump"))
						{
							if(canDoubleJump && !isDoubleJumping)
							{
								StartCoroutine(_Jump());
							}
						}
					}
				}
				if(!isBlocking && isGrounded)
				{
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death"))
					{
						StartCoroutine(_Death());
					}
				}
			}
		}
		if(isDead)
		{
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive"))
			{
				StartCoroutine(_Revive());
			}
		}
	}
    */
    void OnCollisionEnter(Collision col)
    {
     
        if (col.gameObject.tag == "Dardo")
        {
            GetHit();
        }
       
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "EnemyG")
        {
            if (col.GetComponent<EnemyG>().weakness != null && !enemyGhit)
            {
                animator.SetTrigger("Solid");
                enemyGhit = true;
            }
        }
        if (col.gameObject.tag == "ClimbOut")
        {
            animator.SetBool("Climbing", false);
        }
        if (col.gameObject.tag == "Spike")
        {
            GetDamage(5);
            transform.position = new Vector3(19.48439f, -38.96325f, 8.618758f);
        }


        if (col.gameObject.tag == "GiantWeapon")
        {
            GetDamage(5);
            GetHit();
        }
        if (col.gameObject.tag == "FrogWeapon")
        {
            GetDamage(2);
            GetHit();
        }
        if (col.gameObject.tag == "Compass")
        {
            compass.SetActive(true);
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "Npc")
        {
            canDash = false;

            col.GetComponent<NPC_dialogue>().startDialogue = true;
        }
   

        if (col.gameObject.tag == "MapItem")
        {
            mapaOn = true;
            mapOnOff = true;
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "SwordItem")
        {
            isArmed = true;
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "HealthItem")
        {
            RecoverHP(30);
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "ArrowItem")
        {
            GetDamage(5);
        }
        if (col.gameObject.tag == "BowItem")
        {
            bowOn = true;
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "AutoJump" && timer_AutoJump < 0)
        {
            StartCoroutine(_Jump());
            timer_AutoJump = 1f;
        }
        if (col.gameObject.tag == "BridgeStart")
        {
            transform.LookAt(new Vector3(col.GetComponent<Bridge>().startFocus.transform.position.x, transform.position.y, col.GetComponent<Bridge>().startFocus.transform.position.z));
            bridge_mode = true;
        }
        if (col.gameObject.tag == "BridgeEnd")
        {
            bridge_mode = false;

        }
        if (col.gameObject.tag == "Grab"&& timer_toFall <= -1f)
        {
            direction_MoveGrab = col.transform;
            canDash = false;
            canGrab = true;
        }
        if (col.gameObject.tag == "Climb" )
        {

            direction_MoveClimb = col.transform;
            canDash = false;
            canClimb = true;
        }

    }
    private void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "Climb")
        {
            climb = true;
     
        }
        if (col.gameObject.tag == "Mirror")
        {
            canDash = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                col.GetComponent<MirrorRotate>().rot = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                col.GetComponent<MirrorRotate>().rot = false;
            }

        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "DoorCamera" && !alreadyClose)
        {
            animator.SetTrigger("DoorClose");
            isDoor = true;
            alreadyClose = true;
        }
        if (col.gameObject.tag == "Mirror")
        {
            col.GetComponent<MirrorRotate>().rot = false;

        }
        if (col.gameObject.tag == "EnemyWeapon")
        {
            GetDamage(5);
            GetHit();
        }
        if (col.gameObject.tag == "Npc")
        {
            canDash = true;
            col.GetComponent<NPC_dialogue>().startDialogue = false;
        }
        if (col.gameObject.tag == "Grab")
        {
            canDash = true;
        }
        if (col.gameObject.tag == "Climb")
        {
            //animator.SetBool("Climbing", false);
            climb = false;
          

        }
        if (col.gameObject.tag == "EnemyG")
        {
            enemyGhit = false;


        }

    }


    #endregion
  
}