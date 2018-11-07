using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour
{
    public float rotation_speed;
    private bool rotation_Right = false;
    private bool rotation_Left = false;
    public float rotation_timer;
    public Transform rotation_top;
    public Transform rotation_bot;
    public bool pivot_main;
    float rotation_addSpeed;
    float rotation_low;
    float rotation_medium;
    float rotation_fast;
    float rotation_neutral;
    private RPGCharacterControllerFREE player;
    public Transform mira;
    float current_x = 0.0f;
    float mouse_xPos;
    float rotation_auto;
    float mousexPos_timer;
    float mousexPos_dif;
    int active_bowMode = 0;
    private GameController gc;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent < RPGCharacterControllerFREE >() as RPGCharacterControllerFREE;
       // gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>() as GameController;

        current_x = Input.GetAxis("Mouse X");
        mouse_xPos = Input.mousePosition.x;      

    }

    void Update()
    {
        rotation_speed = 150f;
        if (player.bow_mode)
        {
            active_bowMode = 1;
        }
        else
        {
           // rotation_speed = 50;
        }

        if(active_bowMode == 1)
        {
           // GetComponent<BowCamera>().enabled = player.bow_mode;
            active_bowMode = 2;
        }
        float step = rotation_speed * Time.deltaTime;
        rotation_neutral = Screen.width / 2f;
        rotation_low = Screen.width / 1.6f;
        rotation_medium = Screen.width / 1.4f;
        rotation_fast = Screen.width / 1.2f;
        rotation_auto = Screen.width / 2.1f;


        if (DidMove())
        {
            mousexPos_dif = Mathf.Abs((mouse_xPos - Input.mousePosition.x)/4);


            if(Input.mousePosition.x < rotation_neutral + rotation_auto && Input.mousePosition.x > rotation_neutral - rotation_auto)
            {
                if (Input.mousePosition.x > mouse_xPos)
                {
                    transform.Rotate(Vector3.up * ((rotation_speed/2) + mousexPos_dif) * Time.deltaTime );

                }
                else
                {
                    transform.Rotate(Vector3.down * ((rotation_speed/2)  + mousexPos_dif) * Time.deltaTime);

                }
            }
        }
        else
        {
            mouse_xPos = Input.mousePosition.x;
        }

        if (Input.mousePosition.x > rotation_neutral + rotation_auto)
        {

            transform.Rotate(Vector3.up * (rotation_speed + rotation_addSpeed) * Time.deltaTime);
        }
        else if (Input.mousePosition.x < rotation_neutral - rotation_auto)
        {
            transform.Rotate(Vector3.down * (rotation_speed + rotation_addSpeed) * Time.deltaTime);

        }

        /*
        //Debug.Log(transform.localRotation.eulerAngles.x);
        if (pivot_main && !player.bow_mode)
        {

            if (Input.mousePosition.y < (Screen.height / 2f) / 2.5)
            {
                if (transform.localRotation.eulerAngles.x < 75f || transform.localRotation.eulerAngles.x > 300f && transform.localRotation.eulerAngles.x < 360f)
                {
                    transform.Rotate(Vector3.right * (rotation_speed/2) * Time.deltaTime);
                }

                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation_top.rotation, step);


            }
            else if (Input.mousePosition.y > Screen.height / 1.25f)
            {
                if (transform.localRotation.eulerAngles.x >= 0f && transform.localRotation.eulerAngles.x < 80f || transform.localRotation.eulerAngles.x > 334f && transform.localRotation.eulerAngles.x < 360f)
                {
                    transform.Rotate(Vector3.left * (rotation_speed/2) * Time.deltaTime);
                }
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation_bot.rotation, step);
            }
        }
        */
        /* camera analogica
        float step = rotation_speed * Time.deltaTime;
        rotation_low =     Screen.width / 1.6f;
        rotation_medium =  Screen.width / 1.4f;
        rotation_fast =    Screen.width / 1.2f;
        rotation_neutral = Screen.width / 2f;

        if (Input.mousePosition.x < (rotation_neutral - rotation_low) + rotation_neutral)
        {
            if (Input.mousePosition.x < (rotation_neutral - rotation_medium) + rotation_neutral && Input.mousePosition.x > (rotation_neutral - rotation_fast) + rotation_neutral)
            {
                rotation_addSpeed = 50;
            }
            else if (Input.mousePosition.x < (rotation_neutral - rotation_fast) + rotation_neutral)
            {
                rotation_addSpeed = 100;
            }
            else
            {
                rotation_addSpeed = 0;
            }
            transform.Rotate(Vector3.up * -(rotation_speed + rotation_addSpeed) * Time.deltaTime, Space.World);

            //transform.RotateAround(Vector3.zero, Vector3.up, -rotation_speed * Time.deltaTime);
        }
        else if (Input.mousePosition.x > rotation_low)
        {
            if (Input.mousePosition.x > rotation_medium && Input.mousePosition.x < rotation_fast)
            {
                rotation_addSpeed = 50;
            }
            else if (Input.mousePosition.x > rotation_fast)
            {
                rotation_addSpeed = 100;
            }
            else
            {
                rotation_addSpeed = 0;
            }

            transform.Rotate(Vector3.up * (rotation_speed + rotation_addSpeed) * Time.deltaTime, Space.World);
            // transform.RotateAround(Vector3.zero, Vector3.up, rotation_speed * Time.deltaTime);
        }

        if (pivot_main && !player.bow_mode)
        {

            if (Input.mousePosition.y < (Screen.height / 2f) / 2.5)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation_top.rotation, step);

            }
            else if (Input.mousePosition.y > Screen.height / 1.25f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation_bot.rotation, step);

            }
        }
        */

    }
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
}