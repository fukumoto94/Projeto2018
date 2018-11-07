using UnityEngine;
using System.Collections;

public class MirrorRotate : MonoBehaviour
{
	public GameObject cameraPuzzle;
    public bool rot;
    private float rotY;
    public bool canRot = true;
    public Collider col;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(rot && canRot)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 20);
            col.enabled = true;
        }
        else
        {
            col.enabled = false;

        }
        if (canRot == false)
			cameraPuzzle.SetActive (true);
    }
}
