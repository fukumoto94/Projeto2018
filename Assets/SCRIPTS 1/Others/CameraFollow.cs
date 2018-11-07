using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public Transform position;
	public bool locked;
	public bool follow;
	public float lerpVelocity=1f;
	public float lookSensivity=1f;


	private float yRotation, xRotation;
	private float minVerticalView = -90f, maxVerticalView =90f, minHorizontalView=-180f, maxHorizontalView=180f;
	float currentYRotation, currentXRotation;
	float yRotationV, xRotationV;
	public float lookSmoothDamp = 0.1f;
	// Use this for initialization
	void Start () {
	
	}

	void Update() {
		FieldOfViewChanger ();
		//da lock na visao em algum alvo
		if (Input.GetKeyDown (KeyCode.L))
			locked = !locked;

		//free view
		if (!locked) {
			yRotation += Input.GetAxis ("Mouse X") * lookSensivity;
			xRotation -= Input.GetAxis ("Mouse Y") * lookSensivity;

			//limita a rotacao da camera
			xRotation = Mathf.Clamp (xRotation, minVerticalView, maxVerticalView);
			//yRotation = Mathf.Clamp (yRotation, minHorizontalView, maxHorizontalView);

			currentXRotation = Mathf.SmoothDamp (currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
			currentYRotation = Mathf.SmoothDamp (currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

			transform.rotation = Quaternion.Euler (currentXRotation, currentYRotation, transform.rotation.z);
		}

		
	}
	// Update is called once per frame
	void LateUpdate () {
		if (follow) {
			transform.position = Vector3.Lerp (transform.position, position.position, lerpVelocity * Time.deltaTime);
		}
		if (locked) {
			transform.LookAt (target);
		}
	}

	void FieldOfViewChanger(){
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if(Camera.main.fieldOfView <= 100)
				Camera.main.fieldOfView++;
				
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			if(Camera.main.fieldOfView>=0.5f)
				Camera.main.fieldOfView--;
		}
	}
}
