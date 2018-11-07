using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public string sceneLoad;
    public bool startFade;
    public bool fade;
    public bool isFaded;
	public AudioSource music;
    // the image you want to fade, assign in inspector
    public Image img;
    float i;

    private RPGCharacterControllerFREE player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>();

        if (startFade)
        {
            i = 1;
        }
    }
	void Start() {
		music = GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource>();
	}

    public void Update()
    {
        // fades the image out when you click
        if(fade)
        {
            if(i <= 1)
            {
    				
                i += Time.deltaTime / 5;
				if (music != null)
					music.volume -= Time.deltaTime / 5;

            }
            else
            {
                SceneManager.LoadScene(sceneLoad);

            }
        }
        else
        {
            if (i >= 0)
            {
                i -= Time.deltaTime / 5;

            }
            else
            {
                isFaded = true;
            }

        }

        img.color = new Color(1, 1, 1, i);

    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.changeScene = true;
            fade = true;
        }
    }
}
