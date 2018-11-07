using UnityEngine;
using System.Collections;

public class MyBow : MonoBehaviour
{
    //TargetRayCast

    // public LineRenderer laser = null;
    public float fireRate;
   // public Transform target;
    public Transform[] targets;
    private float distTarget = 2;
    public Light Light_Collider;
    public float range = 25f;

    private bool isTarget = false;
    private Vector3 pos_Start = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 pos_End = Vector3.zero;
    private Transform hit_object = null;
    private RaycastHit hit;
    private float distance;

    //InstantiateArrow
    public GameObject arrow = null;
    private bool isOut = false;
    private float arrow_Force = 10f;
    private float arrowAdd_Force;

    //camera fov
    //private Vector3 scale_base;
    private float fov;
    private float fov_base;

    public Material charging;

    public AudioClip[] sons;
    private AudioSource source;

    //bow animation
    public Animator animator;
    private float charging_bow;
    private bool charg;
    private bool alreadyCharg;

    private RPGCharacterControllerFREE player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
        source = GetComponent<AudioSource>();
        //laser das armas
      //  Light_Collider.transform.position = Vector3.zero;
        fov_base = Camera.main.fieldOfView;
        fov = fov_base;
       // scale_base = target.localScale;
        // laser = this.gameObject.GetComponent<LineRenderer>();
	}
	
    void Update()
    {
        pos_Start = this.transform.position;
        direction = this.transform.TransformDirection(Vector3.forward);
        fireRate -= Time.deltaTime;

        TargetRayCast();

        if(player.bow_mode)
        {
            InstantiateArrow();

        }

        BowAnimation();

        Camera.main.fieldOfView = fov;

    }


    private void BowAnimation()
    {

        if (Input.GetMouseButton(0))
        {
            alreadyCharg = false;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Charging"))
            {
                animator.SetTrigger("Charging");
            }
            source.clip = sons[0];
            source.Play();
 
        }
       else if (Input.GetMouseButtonUp(0))
        {
            source.clip = sons[0];
            source.Stop();

            alreadyCharg = true;
            animator.SetTrigger("Return");
            source.clip = sons[1];
            source.Play();
        }


        if (alreadyCharg)
        {

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Return"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    animator.SetTrigger("Iddle");
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Iddle"))
            {
                alreadyCharg = false;
            }

        }
    }

    private void TargetRayCast()
    {
        // laser.SetPosition(0, pos_Start);      //primeiro ponto do laser
        if(false)
      // if (Physics.Raycast(pos_Start, direction, out hit, range))
        {
            if (hit.transform.name != "Arrow(Clone)")
            {
                if (hit.transform.name != "Stage")
                {
                    // hit.transform.GetComponent<Renderer>().material.color = Color.red;

                    distance = Vector3.Distance(transform.position, hit.point) - 1f;
                    //Light_Collider.transform.position = transform.position + transform.forward * distance;
                    pos_End = transform.position + transform.forward * distance;
                    targets[0].position = pos_End + this.transform.TransformDirection(Vector3.right) * distTarget;
                    targets[1].position = pos_End - this.transform.TransformDirection(Vector3.right) * distTarget;
                    targets[2].position = pos_End + this.transform.TransformDirection(Vector3.up) * distTarget;
                    targets[3].position = pos_End - this.transform.TransformDirection(Vector3.up) * distTarget;


                }
                //laser.SetPosition(1, hit.point);      //ultimo ponto do laser mas somente se o laser  "hit" alguma coisa
                hit_object = hit.transform;
            }
        }
        else
        {
            isTarget = false;
            pos_End = pos_Start + direction * range;
          //  Light_Collider.transform.position = pos_End;
            targets[0].position = pos_End + this.transform.TransformDirection(Vector3.right) * distTarget;
            targets[1].position = pos_End - this.transform.TransformDirection(Vector3.right) * distTarget;
            targets[2].position = pos_End + this.transform.TransformDirection(Vector3.up) * distTarget; 
            targets[3].position = pos_End - this.transform.TransformDirection(Vector3.up) * distTarget;
           // target.gameObject.SetActive(false);

            //laser.SetPosition(1, pos_End);
            if (hit_object != null)
            {
                // hit_object.GetComponent<Renderer>().material.color = Color.gray;

            }

        }
    }

    private void InstantiateArrow()
    {
        if (Input.GetMouseButton(0) && arrowAdd_Force <= 2000 && fireRate < 0)
        {
            arrowAdd_Force += arrow_Force * 5;
            charging_bow += Time.deltaTime;
            if(distTarget > 0.5f)
            {
                distTarget -= Time.deltaTime / 1.5f ;
            }
            
           // target.localScale -= new Vector3(0.05F, 0.05F, 0);
           // fov -= Time.deltaTime * 20;
            
        }
        if (Input.GetMouseButtonUp(0) && fireRate < 0)
        {
            isOut = true;
            fireRate = 1f;
        }
        /*
        if(arrowAdd_Force < 3500)
        {
            charging.color = new Color(0 + ((arrowAdd_Force * ((255 * arrow_Force) / 7000)) / arrow_Force), 0 + (((arrowAdd_Force/2) * ((255 * arrow_Force) / 7000)) / arrow_Force), 0, 255);

        }
        else
        {
            charging.color = new Color(132 + ((arrowAdd_Force * ((255 * arrow_Force) / 7000)) / arrow_Force), 0 - (((arrowAdd_Force / 2) * ((255 * arrow_Force) / 7000)) / arrow_Force), 0, 255);

        }
        */
        if (isOut)
        {
            charg = false;
            GameObject cloneArrow = Instantiate(arrow, this.transform.position, this.transform.rotation) as GameObject;
            cloneArrow.GetComponent<Rigidbody>().AddForce(cloneArrow.transform.forward * arrowAdd_Force);
            // cloneArrow.transform.position = pos_Start;
            //cloneArrow.transform.rotation = this.transform.rotation;
            arrowAdd_Force = 0;
            charging_bow = 0f;
            distTarget = 2;
            //animator.ResetTrigger("Charging");
            isOut = false;
        }
    }
}
