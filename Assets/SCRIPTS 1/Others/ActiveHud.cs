using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHud : MonoBehaviour {


    private RPGCharacterControllerFREE player;
    public GameObject hudHp;
	// Use this for initialization
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;


	}

    private void LateUpdate()
    {
        hudHp.SetActive(player.isArmed);
    }
}
