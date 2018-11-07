using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	public float tremerIntensidade = 0.03f;
	public float tremerVelocidade = 0.007f;
	private float tremerFator;
	private Vector3 posicaoOriginal;
	private Quaternion rotacaoOriginal;
    private RPGCharacterControllerFREE player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

	// Use this for initialization
	void Start () {
        rotacaoOriginal = this.transform.rotation;

    }

    // Update is called once per frame
    void Update () {
        tremerFator -= tremerVelocidade;
        player.shake_time = tremerFator;

        if (tremerFator > 0)
		{
			this.transform.position = posicaoOriginal + Random.insideUnitSphere * tremerFator;
			this.transform.rotation = new Quaternion(
				rotacaoOriginal.x + Random.Range (-tremerFator,tremerFator) * .2f,
				rotacaoOriginal.y + Random.Range (-tremerFator,tremerFator) * .2f,
				rotacaoOriginal.z + Random.Range (-tremerFator,tremerFator) * .2f,
				rotacaoOriginal.w + Random.Range (-tremerFator,tremerFator) * .2f);
		}
        if(player.camera_shake)
        {
            Tremer();
            player.camera_shake = false;
        }

        if(!player.bridge_mode)
        {
            tremerIntensidade = 0.007f;
        }
        else
        {
            tremerIntensidade = 0.03f;

        }
    }
	
	
	
	
	public void Tremer()
	{
		if (this.gameObject != null)
        {
			posicaoOriginal = transform.position;
			tremerFator = tremerIntensidade;
		}
	}
}
