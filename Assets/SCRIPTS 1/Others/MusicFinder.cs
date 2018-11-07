using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFinder : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		SceneLoad loaded = GameObject.FindObjectOfType<SceneLoad> ();
		loaded.music = gameObject.GetComponent<AudioSource> ();
	}

}
