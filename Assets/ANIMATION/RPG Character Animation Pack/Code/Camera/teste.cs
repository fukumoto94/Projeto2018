using UnityEngine;
using System.Collections;

public class teste : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // transform.position = new Vector3(target.position.x - target.localScale.x, target.position.y, target.position.z);
        transform.rotation = target.rotation;
        transform.position = Camera.main.ScreenToWorldPoint(Vector3.zero);


    }
}
