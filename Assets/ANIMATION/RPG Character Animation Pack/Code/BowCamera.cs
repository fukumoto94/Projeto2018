using UnityEngine;
using System.Collections;
//pos x 1.51 pos y 2.15 pos z 2.83
public class BowCamera : MonoBehaviour
{
    public Transform cam_ThirdPerson;
    public Transform cam_Bow;
    private Quaternion cam_BowStartPos;
    public Transform cam_Bridge;
    //private bool cam_Change = false;
    public float cam_Speed;
    bool cam_bowGetRot = true;
    private RPGCharacterControllerFREE player;
    float distance;
    public Transform rotRef;


    bool hitSomething = false;

    public float clipMoveTime = 0.05f;              // time taken to move when avoiding cliping (low value = fast, which it should be)
    public float returnTime = 0.4f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
    public float closestDistance = 0.5f;            // the closest distance the camera can be from the target

    private float m_OriginalDist;             // the original distance to the camera before any modification are made
    private float m_MoveVelocity;             // the velocity at which the camera moved
    private float m_CurrentDist;

    void Awake()
    {
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    // Update is called once per frame
    void Update ()
    {
        //float targetDist = m_OriginalDist;

        //Debug.Log(cam_);
        if (player.bridge_mode)
        {
            cam_Speed = 100f;
            transform.position = cam_Bridge.position;
            transform.rotation = cam_Bridge.rotation;
        }
        else
        {
            cam_Speed = 1f;

            if (player.bow_mode)
            {
               // Debug.Log("Aqui");
                //transform.position = cam_Bow.position
                //GetComponent<OrbitCamera>()._distance = 2f;
              //  transform.position = Vector3.MoveTowards(transform.position, cam_Bow.position, cam_Speed);
                //transform.rotation = cam_Bow.rotation;
                if (cam_bowGetRot)
                {
                   //transform.rotation = new Quaternion(0, cam_Bow.rotation.y, 0, cam_Bow.rotation.w);
                    cam_bowGetRot = false;
                }
            }
            else
            {


                /*
                distance = Raycast3.distance3;
                float nearest = Mathf.Infinity;

                // only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
                if (Raycast3.hit.distance < nearest && (!Raycast3.hit.collider.isTrigger) &&
                    !(Raycast3.hit.collider.attachedRigidbody != null &&
                      Raycast3.hit.collider.attachedRigidbody.CompareTag("Player")) && Raycast3.hit.transform.tag != "Floor" && Raycast3.hit.transform.tag != "EnemyM")
                {
                    // change the nearest collision to latest
                    nearest = Raycast3.hit.distance;
                    targetDist = -transform.InverseTransformPoint(Raycast3.hit.point).z;
                    hitSomething = true;
                }
                else
                {
                    hitSomething = false;
                }

                if(hitSomething)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, cam_Speed / 2);

                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, cam_ThirdPerson.position, cam_Speed/2);

                }
                */
               // transform.position = Vector3.MoveTowards(transform.position, cam_ThirdPerson.position, cam_Speed);
              //  transform.rotation = cam_ThirdPerson.rotation;

                cam_bowGetRot = true;

            }
        }
        
    }
}
