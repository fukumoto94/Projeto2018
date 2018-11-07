using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour
{
    public Transform target;

	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(new Vector3(target.position.x - target.localScale.x, target.position.y, target.position.z));
	}
}
