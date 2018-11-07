using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Floresta : MonoBehaviour {
	public GameObject player, dummy;
	// Use this for initialization
	void Start () {
		player.SetActive (false);
	}
	
	// Update is called once per frame
	public void Begin () {
		player.SetActive (true);
		dummy.SetActive (false);
		Destroy(gameObject);
	}

	void Update() {
		if (Input.anyKeyDown) {
			Begin ();
		}
	}


}
