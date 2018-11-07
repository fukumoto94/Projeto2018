using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private RPGCharacterControllerFREE player;
    public GameObject pivot, pivot2;
	// Use this for initialization
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
	}
	
    void Start()
    {
        pivot.GetComponent<BowCamera>().enabled = true;
    }

}
