using UnityEngine;
using System.Collections;

public class VelocityModifier : MonoBehaviour {
	private bool velocityUp, velocityDown;
	public SplineWalker camera;
	public float speed;
	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("Camera").GetComponent<SplineWalker>();
		if (speed < 0)
			velocityDown = true;
		else
			velocityUp = true;
	}
	
	// Update is called once per frame
	void ModifyVelocity (float speed) {
		if (velocityDown) {
			speed*=-1;
			camera.duration += speed;
		}
		if (velocityUp) {
			float i = camera.duration - speed;
			if (i < 1) {
				i = 1;
			}
			camera.duration = i;
		}

	}

	void OnTriggerEnter(Collider Other){
		if (Other.tag == "Camera") {
			ModifyVelocity (speed);
		}




			}
}
