using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectDestroyer : MonoBehaviour {
	public float timmer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("erro nao");
		if (timmer > 0) {
			timmer -= Time.deltaTime;
		} else if (timmer <= 0) {
			Destroy (gameObject);
		}
	}
}
