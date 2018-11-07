using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneChangeScene : MonoBehaviour {
	public AudioSource musicBG;
	public string nextLevel;
	public GameObject fadeOut;
	public bool boss;
	private bool active;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (musicBG != null && active) {
			if (fadeOut != null)
				fadeOut.SetActive (true);
			musicBG.volume -= Time.deltaTime/4;
		}
		if (musicBG.volume <= 0 && active) {
			PassLevel ();
		}

		if (Input.anyKey && !boss) {
			//Debug.Log ("Deu");
			active = true;
			if (fadeOut != null)
				fadeOut.SetActive (true);
		}
	}
	public void PassLevel() {
		SceneManager.LoadScene (nextLevel);
	}

	public void DownMusic() {
		active = true;
	}

	public void ActiveBlack() {
		fadeOut.SetActive (true);
	}
}
