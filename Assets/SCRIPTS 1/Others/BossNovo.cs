using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNovo : MonoBehaviour
{
    //particle
    public Transform[] attackParticle;
    public GameObject[] eye;
    public GameObject headParticle;

	//light
	public Light bosslight;

	//cutscene
	public GameObject cutscene;
	public GameObject deathParticle;

    //fireattack
    public GameObject Eye;
    public Transform middleEye;
    public GameObject prefab;
    private float timerToFire;
    private bool fire;

    public SkinnedMeshRenderer head;
    public SkinnedMeshRenderer[] body;

    //audio
    private AudioSource source;
    public AudioSource[] sources;
    public GameObject deathSound;
	private bool morto;

    //start
    private bool startFight;
    public Transform[] platform;
    public GameObject sound;

    public bool knockback;
    private bool knockL, knockR;
    public float cdAttack;
    public float timerAttack;
    public int lifes;
    private int randAttack;
    private float timerReturn;
    public bool headHit;
    private Animator anim;
    private bool bossOff;
    public float timerOff;
    public bool[] gemHit;
    public GameObject[] gems;
    public GameObject[] hands;
    public bool firstHit = true;
    //State Machine
    private States state;
    private float waitTime = 0.01f;

    private RPGCharacterControllerFREE player;
    private OrbitCamera oc;

    private bool attacking;

    public enum States
    {
        Iddle,
        Soco,
        Tapa,
        DoisTapas,
        Fogo,
        Desabilitado,
        Knockback,
        Reset,
        Morte
    }

    // Use this for initialization
    void Start ()
    {
        
        source = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
        oc = GameObject.FindGameObjectWithTag("OrbitCamera").GetComponent<OrbitCamera>() as OrbitCamera;

        anim = GetComponent<Animator>();
        state = States.Iddle;
        StartCoroutine(verificaFSM(waitTime));
		bosslight.range = 0;
        anim.speed = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Eye.transform.LookAt(Vector3.Lerp(Eye.transform.position, player.transform.position, 0.00001f));
        timerToFire -= Time.deltaTime;

        if (timerToFire <= 0 && fire)
        {
            Instantiate(prefab, middleEye.position, middleEye.rotation);
            fire = false;
        }

        head.updateWhenOffscreen = bossOff;

		/*if (morto)
			GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume -= Time.deltaTime/2;*/

        // HandClosest();
        Controller();
    }

    IEnumerator verificaFSM(float waitTime)
    {
        while (true)
        {
            if(state == States.Iddle)
            {
                
                if(timerAttack > cdAttack)
                {
                    randAttack = Random.Range(0, 3);
                    Iddle(randAttack);
                }
                else
                {
          
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        anim.SetTrigger("Iddle");
                    }
                }
            }
            else if (state == States.Soco)
            {
                if(HandClosest())
                {
                    Attack("Soco");
                    ParticleController("Soco", .5f, .7f);

                }
                else
                {

                    Attack("SocoL");
                    ParticleController("SocoL", .5f, .7f);

                }
            }
            else if (state == States.Tapa)
            {
                if (HandClosest())
                {
                    Attack("Tapa");
                    ParticleController("Tapa", .5f, .7f);

                }
                else
                {
                    Attack("TapaL");
                    ParticleController("TapaL", .5f, .7f);

                }
            }
            else if (state == States.DoisTapas)
            {
                Attack("DoisTapas");
                ParticleController("DoisTapas", .5f, .7f);

            }
            else if (state == States.Fogo)
            {
                Attack("Fogo");
            }
            else if(state == States.Knockback)
            {
                if(knockback)
                {
                    if (gemHit[0] && !knockL)
                    {
                        anim.SetTrigger("Knockback");
                        knockL = true;

                    }
                    else if (gemHit[1] && !knockR)
                    {
                        anim.SetTrigger("KnockbackL");
                        knockR = true;
                    }
                    knockback = false;

                }
                else
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        state = States.Iddle;
                    }
                }
               
            }
            else if (state == States.Desabilitado)
            {
                Desabilitado();
            }
            else if (state == States.Reset)
            {
                ResetBoss();
            }
            else
            {
                Morte();
            }


            yield return new WaitForSeconds(waitTime);

        }
    }

    private bool HandClosest()
    {

        if (player.transform.position.z < 9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //controla tempo / desabilitar boss e morte
    private void Controller()
    {
		if (bosslight.range < 200 && startFight == true) {
			bosslight.range += 20 * Time.deltaTime;
		}

        if ((Eye.transform.position - player.transform.position).magnitude < 38 && !startFight)
        {
            startFight = true;
            oc.startBoss = true;
            anim.speed = 1;

        }

  
        if (startFight)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Intro"))
            {
                oc.startBoss = false;

            }
        }

        
        if (lifes >= 3)
        {
            state = States.Morte;
        }
        else
        {

            if (state == States.Iddle)
            {
                if(startFight)
                {
                    sound.SetActive(true);
                    platform[0].GetComponent<Rigidbody>().isKinematic = false;
                    platform[0].GetComponent<Collider>().enabled = false;
                    platform[1].GetComponent<Rigidbody>().isKinematic = false;
                    platform[1].GetComponent<Collider>().enabled = false;


                    timerAttack += Time.deltaTime;
                }
            }
            else
            {
                timerAttack = 0f;
            }

            if (state == States.Desabilitado)
            {
                timerReturn += Time.deltaTime;
            }
            else
            {
                timerReturn = 0;
            }

            if (headHit)
            {
                state = States.Desabilitado;
            }

            if(knockback)
            {
                state = States.Knockback;
            }

			if(gemHit[0] && gemHit[1] && state != States.Reset)
            {
                headParticle.SetActive(true);
            }

            gems[0].SetActive(!gemHit[0]);
            gems[1].SetActive(!gemHit[1]);


        }
    }

    private void Iddle(int attack)
    {
        attacking = false;
        switch (attack)
        {
            case 0:
                source.Play();
                state = States.Soco;
                break;
            case 1:
                source.Play();
                state = States.Tapa;
                break;
            default:
                if(gemHit[0] || gemHit[1])
                {
                    source.Play();
                    state = States.Fogo;
                    timerToFire = 1f;
                    fire = true;
                }
                else
                {
                    source.Play();
                    state = States.DoisTapas;
                }
                break;        
        }
    }

    private void ParticleController(string type, float start, float end)
    {
       // Debug.Log(type);
        switch (type)
        {
            case "Soco":

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= start && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < end)
                {
                    sources[0].Play();
                    attackParticle[0].GetComponent<ParticleSystem>().enableEmission = true;
                }

                break;
            case "SocoL":

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= start && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < end)
                {
                    sources[1].Play();
                    attackParticle[1].GetComponent<ParticleSystem>().enableEmission = true;
                }

                break;
            case "Tapa":

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= start && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < end)
                {
                    sources[1].Play(0);

                    attackParticle[2].GetComponent<ParticleSystem>().enableEmission = true;
                }

                break;
            case "TapaL":

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= start && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < end)
                {
                    sources[1].Play();

                    attackParticle[3].GetComponent<ParticleSystem>().enableEmission = true;
                }

                break;
            default:

                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= start && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < end)
                {
                    sources[0].Play();
                    sources[1].Play();

                    attackParticle[4].GetComponent<ParticleSystem>().enableEmission = true;
                }

                break;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= end)
        {
            attackParticle[0].GetComponent<ParticleSystem>().enableEmission = attackParticle[1].GetComponent<ParticleSystem>().enableEmission =
            attackParticle[2].GetComponent<ParticleSystem>().enableEmission = attackParticle[3].GetComponent<ParticleSystem>().enableEmission =
            attackParticle[4].GetComponent<ParticleSystem>().enableEmission = false;
        }

    }
    private void Attack(string attack)
    {
        //Debug.Log(attack);
        if (!attacking)
        {
         
            anim.SetTrigger(attack);
            attacking = true;
        }
        else
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                state = States.Iddle;
            }
        }
    
    }

    private void Desabilitado()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Desabilitado") && !bossOff)
        {
            headParticle.SetActive(false);

            eye[0].SetActive(false);
            eye[1].SetActive(false);

            anim.SetTrigger("Desabilitado");
            bossOff = true;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            if(timerReturn > timerOff)
            {
                headHit = false;
                state = States.Reset;
            }
        }
    }

    private void ResetBoss()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Reset"))
        {
            eye[0].SetActive(true);
            eye[1].SetActive(true);
            anim.SetTrigger("Reset");
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                knockL = false;
                knockR = false;
                bossOff = false;
                firstHit = true;
                gemHit[0] = false;
                gemHit[1] = false;
                state = States.Iddle;
            }
        }    
    }

    private void Morte()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Morte"))
        {
            deathSound.SetActive(true);
			morto = true;
			//cria cutscene
			//prende o player
            anim.SetTrigger("Morte");
			cutscene.SetActive(true);
			eye[0].SetActive(true);
			eye[1].SetActive(true);
			deathParticle.SetActive (true);

        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
            {
                bossOff = true;
                head.updateWhenOffscreen = bossOff;
                body[0].updateWhenOffscreen = bossOff;
                body[1].updateWhenOffscreen = bossOff;
                body[2].updateWhenOffscreen = bossOff;

                //chama o black e troca de cena
                //Destroy(this.gameObject);
            }
             if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                

                //chama o black e troca de cena
                //Destroy(this.gameObject);
            }
        }
      
    }
}
