using UnityEngine;
using System.Collections;

public class TargetChange : MonoBehaviour {

	public CameraFollow camFollow;
	public GameObject[] targets;
	public int i=0;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		

	}

	public void TargetPlus() {
		i++;
		if (i > targets.Length -1)
			i = targets.Length -1;
		ChangeTarget ();
	}

	public void TargetMinus() {
		i--;
		if (i < 0)
			i = 0;
		ChangeTarget ();
	}

	public void ChangeTarget() {
		camFollow.target = targets [i].transform;
	}
}
