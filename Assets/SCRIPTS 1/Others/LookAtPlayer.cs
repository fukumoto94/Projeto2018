using UnityEngine;
using System.Collections;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.transform.LookAt(new Vector3(player.transform.position.x, transform.transform.position.y, player.transform.position.z));
	}
}
