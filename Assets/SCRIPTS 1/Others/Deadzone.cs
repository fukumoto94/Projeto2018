using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Deadzone : MonoBehaviour
{
    public string sceneLoad;
    private bool fade;
    // the image you want to fade, assign in inspector
    public Image img;
    float i;

    private RPGCharacterControllerFREE player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>();
    }
    public void Update()
    {
        // fades the image out when you click
        if (fade)
        {
            if (i <= 1)
            {

                i += Time.deltaTime;

            }
            else
            {
                SceneManager.LoadScene(sceneLoad);

            }
        }


        img.color = new Color(1, 1, 1, i);

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.changeScene = true;
            fade = true;
        }
    }
}
