using UnityEngine;
using System.Collections;

public class MagicDoor : MonoBehaviour
{
    public Transform slot;
    private float timer = 2f;
    private float speed = 3f;
    private Vector3 closedDoor;
    public GameObject orb;
    // Use this for initialization
    void Start ()
    {
        closedDoor = transform.position + transform.up * -10;
    }

    // Update is called once per frame
    void Update ()
    {
	    if(slot.GetComponent<SlotMagicDoor>().orbOn)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, closedDoor, speed * Time.deltaTime);
            }
        }

	}

}
