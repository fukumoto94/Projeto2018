using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAux : MonoBehaviour
{
    private EnemyP frog;
	// Use this for initialization
	void Awake ()
    {
        frog = GameObject.FindGameObjectWithTag("EnemyM").GetComponent<EnemyP>() as EnemyP;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            frog.playerOn = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
