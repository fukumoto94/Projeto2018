using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour
{
    public Transform player_transform;
    bool player_collide;
    Rigidbody rb;
    RPGCharacterControllerFREE player_script;
    public GameObject tourch;
    public GameObject slot;
    private bool slotOn;
    // Use this for initialization
    void Awake ()
    {
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent < RPGCharacterControllerFREE >() as RPGCharacterControllerFREE;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
      
        if (player_collide && Input.GetMouseButton(0))
        {
            player_script.itemPick = true;
            transform.position = new Vector3(player_transform.position.x, player_transform.position.y, player_transform.position.z);         
        }
        else
        {
            player_script.itemPick = false;

            if (slotOn)
            {
                transform.position = slot.transform.position;

                player_collide = false;
                player_script.object_moving = false;
            }
            else
            {
                player_collide = false;
                player_script.object_moving = false;
            }
         
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player_collide = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Slot")
        {
            //tourch.SetActive(true);
            slotOn = true;

        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
          //  player_collide = false;
        }
        if (col.gameObject.tag == "Slot")
        {
            //tourch.SetActive(false);
        }
    }

}
