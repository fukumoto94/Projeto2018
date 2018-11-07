using UnityEngine;
using System.Collections;

public class CloseDoor : MonoBehaviour
{
    public bool close = false;
    public bool firstDoor;
    private float speed = 3f;
    private Vector3 closedDoor;
    private Vector3 openDoor;
    private AudioSource source;
    public Collider colTrigger;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        closedDoor = transform.position + transform.forward*11;
        openDoor = transform.position;
    }
    // Update is called once per frame
    void Update ()
    {
	    if(close)
        {
            source.Play();
            transform.position = Vector3.MoveTowards(transform.position, closedDoor, speed * Time.deltaTime);
        }
        else
        {

            transform.position = Vector3.MoveTowards(transform.position, openDoor, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && firstDoor)
        {
            close = true;
        }
    }
}
