using UnityEngine;
using System.Collections;

public class GetONPath : MonoBehaviour {
	public ChainPathScript cameraOffRail;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider Other) {
		if (Other.tag == "Player") {
			cameraOffRail.onPath = true;
		}
	}
}
