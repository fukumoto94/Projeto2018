using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject playerCanvas;
    public Canvas canvasOpcoes;
    public Slider music;
    public Slider sensibility;
    private bool pause;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
        }

        if(pause)
        {
			Cursor.visible = true;
            Time.timeScale = 0f;
            AudioListener.volume = music.value;
            PlayerPrefs.SetFloat("Sensibility", sensibility.value);
        }
        else
        {
			Cursor.visible = false;
            sensibility.value = PlayerPrefs.GetFloat("Sensibility");
            Time.timeScale = 1f;
        }
        playerCanvas.SetActive(!pause);
        canvasOpcoes.gameObject.SetActive(pause);


    }

    public void sair()
    {
		Application.LoadLevel("Menu");
    }

    public void voltar()
    {
        pause = false;
        canvasOpcoes.gameObject.SetActive(false);
    }
}
