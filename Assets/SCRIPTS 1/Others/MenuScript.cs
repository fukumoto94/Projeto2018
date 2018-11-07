using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas mainCanvas;
    public Canvas canvasOpcoes;
    public Slider music;
    public Slider sensibility;
	public AudioSource musicBG;
	public string nextLevel;
	public GameObject fadeOut;
	private bool active;


	void Start() {
		Cursor.visible = true;
	}

    private void Update()
    {
        AudioListener.volume = music.value;
        PlayerPrefs.SetFloat("Sensibility", sensibility.value);

		if (musicBG != null && active) {
			if (fadeOut != null)
				fadeOut.SetActive (true);
			musicBG.volume -= Time.deltaTime/4;
		}
		if (musicBG.volume <= 0 && active) {
			PassLevel ();
			Cursor.visible = false;
		}
			
	}
	public void PassLevel() {
		SceneManager.LoadScene (nextLevel);
	}

	public void DownMusic() {
		active = true;
	}

	public void ActiveBlack() {
		fadeOut.SetActive (true);
	

    }

    public void jogar(){
		//Application.LoadLevel("IntroCutscene");
		DownMusic();
	}

    public void opcoes()
    {
        Debug.Log("Clicou");
        mainCanvas.gameObject.SetActive(false);
        canvasOpcoes.gameObject.SetActive(true);

    }

    public void sair(){
			Application.Quit ();
	}

	public void voltar(){
        mainCanvas.gameObject.SetActive(true);
        canvasOpcoes.gameObject.SetActive(false);     
	}

}
