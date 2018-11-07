using UnityEngine;
using System.Collections;

public class ChainPathScript : MonoBehaviour {

	public CameraFollow camFollow;
	public SplineWalker path;

	public bool onPath;
	// Use this for initialization

	void Start () {
		onPath = false;
		path.enabled = false;

	
	}
	
	// Update is called once per frame
	void Update () {
		if (onPath) {
			path.enabled = true;
			camFollow.follow = false;
		} else {
			path.enabled = false;
			camFollow.follow = true;
		}

		if (path.progress == 1)
			onPath = false;
	}
}
