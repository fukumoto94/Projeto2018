using UnityEngine;
using System.Collections;

public class ObjectDash : MonoBehaviour
{
    private RPGCharacterControllerFREE player;
    private Vector3 dist;
    public float canDash;
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
	}
	
	// Update is called once per frame
	void Update ()
    {
        dist = transform.position - player.transform.position;

	    if(player.isRolling && dist.magnitude < canDash)
        {
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
