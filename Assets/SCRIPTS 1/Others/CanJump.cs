using UnityEngine;
using System.Collections;

public class CanJump : MonoBehaviour
{
    private bool canJump;
    public Transform frog;

	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(new Vector3(frog.transform.position.x, transform.position.y, frog.transform.position.z));

        if (!canJump)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.forward, 2*Time.deltaTime);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        canJump = false;
    }
    private void OnTriggerExit(Collider other)
    {
        canJump = true;
    }
}
