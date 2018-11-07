using UnityEngine;
using System.Collections;

public class Dardo : MonoBehaviour {

    public float dardoForce;
    public float damage;
    private bool wallCollide = false;
    private bool playerCollide = false;
    private RaycastHit hit;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Physics.Raycast(transform.position + (transform.forward*2.5f), transform.TransformDirection(Vector3.forward), out hit, 0.5f))
        {
            Debug.Log(hit.transform.tag);

            if (hit.transform.tag == "Wall")
            {
                GetComponent<Rigidbody>().isKinematic = true;
                wallCollide = true;
            }
        }

        if (!wallCollide && playerCollide)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * dardoForce;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<RPGCharacterControllerFREE>().GetDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerCollide = true;
        }
    }
}
