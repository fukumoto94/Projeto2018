using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Teste : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()

    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("RPG-Character");
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back);
        }
    }
}
