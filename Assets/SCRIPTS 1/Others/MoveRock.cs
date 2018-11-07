using UnityEngine;
using System.Collections;

public class MoveRock : MonoBehaviour
{
    public GameObject mover;
    public bool isEndPath;
    public float speed;
    private bool isRolling = false;
    private float timer = 8f;
	
	// Update is called once per frame
	void Update ()
    {
        if(isRolling && timer > 0)
        {
            timer -= Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Rock")
        {
            if(isEndPath)
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(mover);
                Destroy(gameObject);
            }
            else
            {
                isRolling = true;
            }
        }
    }
}
