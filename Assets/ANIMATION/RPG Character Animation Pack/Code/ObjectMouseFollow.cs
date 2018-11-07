using UnityEngine;
using System.Collections;

public class ObjectMouseFollow : MonoBehaviour
{
    public Transform cam;
    private float timer;
    private RPGCharacterControllerFREE player;
	// Use this for initialization

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

	void Start ()
    {
        // transform.position = new Vector2(cam.position.x, cam.position.y);

    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            timer = 0;
           // transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 20f));
        }
        else
        {
            //timer += Time.deltaTime;

            if(timer > -1)
            {
                if (player.isStrafing)
                {
                    transform.position = new Vector3(player.focusTemp.x, player.focusTemp.y + 2f, player.focusTemp.z);
                }
                else
                {
                    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
                }
            }
         
        }
        //Cursor.lockState = CursorLockMode.Locked;
    
        Cursor.visible = true;
      

    }
}
