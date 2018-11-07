using UnityEngine;
using System.Collections;

public class DodgeWall : MonoBehaviour
{
    private Vector3 direction;
    private RaycastHit hit;
    public int range = 5;
    public bool canMove = true;
    private GameObject[] walls = new GameObject[3];
    public Transform Object;
	// Use this for initialization
	void Start ()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
	}
	
	// Update is called once per frame
	void Update ()
    {
        direction = this.Object.TransformDirection(Vector3.forward);

        if (Physics.Raycast(Object.position, direction, out hit, range))
        {
            if(hit.transform.tag == "Wall")
            {
                canMove = false;
            }
        }
        else
        {
            canMove = true;
        }     
    }
}
