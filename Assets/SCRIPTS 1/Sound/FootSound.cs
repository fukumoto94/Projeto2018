using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootSound : MonoBehaviour
{
    public GameObject[] walkSound;
    private int nrand;

    private RPGCharacterControllerFREE player;

    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.moveSpeed > 0)
        {
            if (SceneManager.GetActiveScene().name == "Forest 1")
            {
                nrand = Random.Range(0, 4);

            }
            else
            {
                nrand = Random.Range(5, 8);
            }
        }
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (player.isGrounded)
        {
            walkSound[nrand].GetComponent<AudioSource>().Play();

        }

    }
}
