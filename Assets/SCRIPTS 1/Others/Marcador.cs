using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Marcador : MonoBehaviour
{
    private RPGCharacterControllerFREE player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    void Update ()
    {
        transform.position = player.transform.position;
	}
}
