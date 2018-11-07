using UnityEngine;
using System.Collections;

/**
 * Change the camera into an orbital camera. An orbital is a camera
 * that can be rotated and that will automatically reorient itself to
 * always point to the target.
 * 
 * The orbit camera allow zooming and dezooming with the mouse wheel.
 * 
 * By clicking the mouse and dragging on the screen, the camera is moved. 
 * The angle of rotation  correspond to the distance the cursor travelled. 
 *  
 * The camera will keep the angular position when the button is pressed. To
 * rotate more, simply repress the mouse button et move the cursor.
 *
 * This script must be added on a camera object.
 *
 * @author Mentalogicus
 * @date 11-2011
 */
public class OrbitCamera : MonoBehaviour
{


    //death
    public Transform deathCamera;

    //ItemPickUp
    public Transform ItemPickRef;
    public bool isPick;

    //scene
    public int scene;
    public Transform doorCamera;
    private float timerDoor;

    //bezier curve
    public Transform[] pathObjs;


    private float turnSpeed = 1.5f;

    private float turnsmoothing = .1f;
    private float smoothVelocity = 0.0F;

    private float smoothX = 0;
    private float smoothY = 0;
    private float smoothXvelocity = 0;
    private float smoothYvelocity = 0;
    //The target of the camera. The camera will always point to this object.
    public Transform _target;

    public float disCamera;
    //The default distance of the camera from the target.
    public float _distance = 20.0f;

    //Control the speed of zooming and dezooming.
    public float _zoomStep = 1.0f;

    //The speed of the camera. Control how fast the camera will rotate.
    private float _xSpeed = 1f;
    private float _ySpeed = 1f;

    //The position of the cursor on the screen. Used to rotate the camera.
    private float _x = 0.0f;
    private float _y = 0.0f;

    //limits
    public float yMinLimit = -10;
    public float yMaxLimit = 80;
	private float startYMi;
	private float startYMax;
    //Distance vector. 
    private Vector3 _distanceVector;


    //Protect from wall
    public bool isBlocking = false; 
    private float lastDistace;
    private bool afterBlock = false;
    private Vector3 distancePlayer;
    private Vector3 lastPos;
    public static RaycastHit hit;

    public bool isProtect = false;
    //bow mode
    private bool getPos = false;
    public float speedRot;
    GameObject[] temp;
    private OrbitCamera oc;
    private float rotation_auto;
    private float rotation_neutral;
    //player
    private RPGCharacterControllerFREE player;

    //bridge mode
    public Transform bridgeCam;
    float current_x = 0.0f;
    float mousexPos_dif;
    float mouse_xPos;
    bool camCanMove = true;

    //npcMode
    public Transform[] npcCam;
    public int auxNpc;
    public bool isNpc = false;

    //bossmode
    public bool isBoss;
    public Transform bowRot;
    public GameObject bossModel;
    public bool startBoss;
    
    //orb
    public bool orbOn;
    private float timerOrb = 1f;
    public Transform orbCamera;

    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
        oc = GameObject.FindGameObjectWithTag("WallProtect").GetComponent<OrbitCamera>() as OrbitCamera;

    }
    /**
     * Move the camera to its initial position.
     */
    void Start()
    {
		startYMax = yMaxLimit;
		startYMi = yMinLimit;
        _distanceVector = new Vector3(0.0f, 0.0f, -_distance);

        Vector2 angles = this.transform.localEulerAngles;
        _x = angles.x;
        _y = angles.y;

        this.Rotate(_x, _y);
        temp = GameObject.FindGameObjectsWithTag("Wall");
    }

    /**
     * Rotate the camera or zoom depending on the input of the player.
     */
     void Update()
    {
        _xSpeed = PlayerPrefs.GetFloat("Sensibility");
        _ySpeed = PlayerPrefs.GetFloat("Sensibility");

        distancePlayer = transform.position - _target.position;
    }
    void LateUpdate()
    {
        rotation_auto = Screen.width / 4f;
        rotation_neutral = Screen.width / 2f;
        if (_target && player.shake_time <= 0)
        {
            this.RotateControls();

            if (!isProtect && !player.bow_mode)
            {
               // this.Zoom();
                Wall();
            }
            else
            {
                _distanceVector = new Vector3(0, 0, -disCamera);
            }

            if (player.bow_mode)
            {
                if(!getPos)
                {
                    lastPos = _distanceVector;
                    getPos = true;
                }
                if (DidMove())
                {
                    if ((Input.mousePosition.x > mouse_xPos && Input.mousePosition.x > rotation_neutral + rotation_auto) ||
                          (Input.mousePosition.x < mouse_xPos && Input.mousePosition.x < rotation_neutral - rotation_auto))
                    {
                        camCanMove = true;

                    }
                    else
                    {
                        camCanMove = false;

                    }
                }
                else
                {
                    mouse_xPos = Input.mousePosition.x;
                }
                
                if(isBoss)
                {
                    yMinLimit = -3f;
                    yMaxLimit = -40f;
                }
                else
                {
                    yMinLimit = 0;
                    yMaxLimit = 0;
               
                }
                
                if (!isBlocking)
                {


                }
                _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(3.5f, 0.5f, 4f), 0.2f);


                // _xSpeed = 0.5f;


            }
            else if (player.bridge_mode)
            {
                if (!getPos)
                {
                    lastPos = _distanceVector;
                    getPos = true;
                }
                /*
                yMinLimit = 0;
                yMaxLimit = 0;
               _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0, 0, 15f), 0.2f);
                transform.LookAt(player.transform);
                */
            }
            else if(player.isStrafing)
            {
                if (!getPos)
                {
                    lastPos = _distanceVector;
                    transform.rotation = player.transform.rotation;

                    getPos = true;
                }
            }
            else
            {
                if(getPos && !isProtect)
                {
					yMinLimit = startYMi;
					yMaxLimit = startYMax;
                    _distanceVector = lastPos;
                    //_distanceVector = Vector3.Lerp(_distanceVector, lastPos, 0.2f);

                    getPos = false;
                }
                camCanMove = true;

            }
          
        }
    }

    /**
     * Rotate the camera when the first button of the mouse is pressed.
     * 
     */
    void RotateControls()
    {
      
        if (!player.bridge_mode && camCanMove)
        {
            if(!player.isStrafing)
            {
                _x += Input.GetAxis("Mouse X") * _xSpeed;

            }
            _y += -Input.GetAxis("Mouse Y") * _ySpeed;
        }

        _y = ClampAngle(_y, yMinLimit, yMaxLimit);

        if (turnsmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, _x, ref smoothXvelocity, turnsmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, _y, ref smoothYvelocity, turnsmoothing);
        }
        else
        {
            smoothX = _x;
            smoothY = _y;
        }

        this.Rotate(smoothX, smoothY);




    }

    /**
     * Transform the cursor mouvement in rotation and in a new position
     * for the camera.
     */
    void Rotate(float x, float y)
    {
        Quaternion rotation;
        if (player.isStrafing)
        {
            rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, 0.2f);
           // rotation = Quaternion.Euler(y, x, 0.0f);
           // print(player.transform.rotation);
           if(!isBlocking)
            {
                _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(2f, 0, -disCamera), 0.2f);

            }

        }
        else
        {
            rotation = Quaternion.Euler(y, x, 0.0f);
        }
        //Transform angle in degree in quaternion form used by Unity for rotation.

        //The new position is the target position + the distance vector of the camera
        //rotated at the specified angle.
        Vector3 position = rotation * _distanceVector + _target.position;

      
        if(player.bridge_mode && !isProtect && player.currentHp > 0)
        {
            /*
           // transform.rotation = bridgeCam.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, bridgeCam.rotation, 0.05f);
          //  transform.position = bridgeCam.position;
            transform.position = Vector3.Lerp(transform.position, bridgeCam.position, 0.03f);
            */
            transform.position = Vector3.MoveTowards(transform.position, CalculateCubicBezierPoint(1f, pathObjs[0].position,
                                                                                     pathObjs[1].position,
                                                                                     pathObjs[2].position,
                                                                                     pathObjs[3].position),
                                                                                     0.15f);
            //  transform.rotation = Quaternion.Lerp(transform.rotation, bridgeCam.rotation, 0.05f);

      
            transform.LookAt(player.transform);

        }
        else if (orbOn)
        {
            transform.rotation = orbCamera.rotation;
            transform.position = orbCamera.position;
            timerOrb -= Time.deltaTime;

            if(timerOrb < 0)
            {
                orbOn = false;
            }
        }
        else if(startBoss)
        {
            transform.LookAt(bossModel.transform);
        }
        else if (player.currentHp <= 0 && deathCamera!= null)
        {
            transform.rotation = deathCamera.rotation;
            transform.position = deathCamera.position;
        }
        else if(isNpc)
        {
            transform.rotation = npcCam[auxNpc].rotation;
            transform.position = npcCam[auxNpc].position;

        }
        else if (isPick)
        {
            transform.rotation = ItemPickRef.rotation;
            transform.position = ItemPickRef.position;
        }
        else
        {
            if(scene == 2)
            {
                if(player.isDoor)
                {
                    timerDoor += Time.deltaTime;

                    transform.rotation = doorCamera.rotation;
                    transform.position = doorCamera.position;

                    if(timerDoor >= 3f)
                    {
                        player.isDoor = false;
                    }
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
       

        }

    }
    //verifica se moveu mouse
    bool DidMove()
    {
        // get the current change in horizontal mouse movement
        current_x = Input.GetAxis("Mouse X");

        // check if the current x has a value of 0. if not, return true;
        if (current_x == 0)
        {
            return false;

        }
        else
        {
            return true;
        }
    }
    /**
     * Zoom or dezoom depending on the input of the mouse wheel.
     */
    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            this.ZoomOut();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            this.ZoomIn();
        }
        /*
        Debug.Log(distancePlayer.magnitude);

        if (isBlocking)
        {
            afterBlock = true;
            if (_distance >= -5 && distancePlayer.magnitude > 0.6f)
            {
                _distance -= 0.5f;
                _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
            }
        }
        else
        {
            if(afterBlock)
            {
                if (_distance <= 10)
                {
                    _distance += 0.5f;
                    _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
                }
                else
                {
                    afterBlock = false;
                }
            }
        }
        */

        /*
         wall disable
        if (Raycast3.hit.transform.tag == "Wall")
        {
            Raycast3.hit.transform.gameObject.layer = 8;
        }
        else
        {
            foreach(GameObject go in temp)
            {
                go.layer = 0;
            }
        }
        */

        /*
        Debug.Log(isBlocking);

        if (Raycast3.hit.transform.tag == "Wall")
        {
            _distance = -4;
            _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
        }
        else
        {
            if(!isBlocking)
            {
                _distance = 4;
                _distanceVector = new Vector3(0.0f, 0.0f, -_distance);
            }
        
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * -1, out hit, 10f))
        {
            if (hit.transform.tag == "Wall")
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;

            }
        }
        */
    }

    /**
     * Reduce the distance from the camera to the target and
     * update the position of the camera (with the Rotate function).
     */


    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    void Wall()
    {


        isBlocking = oc.isBlocking;

        if (isBlocking)
        {
            afterBlock = true;
            if (Raycast3.colideRight || Raycast3.colideLeft)
            {

                _distance = Mathf.SmoothDamp(_distance, 0.5f, ref smoothVelocity, 0.2f);
                _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0.0f, 0.0f, -_distance), 0.2f);

            }
            else
            {
                if(!Raycast4.colide)
                {
                    _distance = Mathf.SmoothDamp(_distance, disCamera, ref smoothVelocity, 0.2f);
                    _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0.0f, 0.0f, -_distance), 0.2f);
                }

            }
        }
        else
        {
            if (afterBlock)
            {
                _distance = Mathf.SmoothDamp(_distance, disCamera, ref smoothVelocity, 0.2f);
                _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0.0f, 0.0f, -_distance), 0.2f);
                if (_distance > disCamera)
                {
                    afterBlock = false;

                }
            }
        }
    }

    void BowMode()
    {
        yMinLimit = 0;
        yMaxLimit = 200;
        _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(2.0f, 0.0f, -7), 0.2f);


    }
    /**
     * Increase the distance from the camera to the target and
     * update the position of the camera (with the Rotate function).
     */
    public void ZoomIn()
    {
        
        _distance = Mathf.SmoothDamp(_distance, disCamera, ref smoothVelocity, 0.2f);
        _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0.0f, 0.0f, -_distance), 0.2f);


        this.Rotate(_x, _y);
        
    }

    public void ZoomOut()
    {
        _distance = Mathf.SmoothDamp(_distance, 10, ref smoothVelocity, 0.2f);
        _distanceVector = Vector3.Lerp(_distanceVector, new Vector3(0.0f, 0.0f, -_distance), 0.2f);

        this.Rotate(_x, _y);

    
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Wall")
        {
            lastDistace = _distance;
        }
     
    }
    void OnTriggerStay(Collider col)
    {
        if (col.transform.tag != "ClimbOut" && col.transform.tag != "Npc" && col.transform.tag != "Floor" && col.transform.tag != "Player" && col.transform.tag != "Weapon" && col.transform.tag != "EnemyM" && col.transform.tag != "EnemyP" && col.transform.tag != "EnemyG" && col.transform.tag != "BridgeStart" && col.transform.tag != "CameraController" && !player.bridge_mode && col.transform.tag != "SceneController")
        {
            isBlocking = true;
            // Debug.Log(col.tag);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag != "ClimbOut" && col.transform.tag != "Npc" && col.transform.tag != "Floor" && col.transform.tag != "Player" && col.transform.tag != "Weapon" && col.transform.tag != "EnemyM" && col.transform.tag != "EnemyP" && col.transform.tag != "EnemyG" && col.transform.tag != "BridgeStart" && col.transform.tag != "CameraController" && col.transform.tag != "SceneController")
        {
            if (!Raycast3.colideRight && !Raycast3.colideLeft)
            {
                isBlocking = false;
            }
        }
    }

} //End class