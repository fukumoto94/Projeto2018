using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour
{
    public GameObject[] bridgeDown;
    private int countDown;
    public float timerDown;
    public float timerCd;
    public bool startDown = false;
    private Rigidbody[] rbs;
    public GameObject startFocus;
    private RPGCharacterControllerFREE player;
    private int tremor = 4;
    private AudioSource source;
    public GameObject sound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        timerDown = timerCd;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.J))

        {
            player.camera_shake = true;

        }
        if (startDown)
        {
            timerDown -= Time.deltaTime;
        }

        if (timerDown <= 0 && countDown < bridgeDown.Length)
        {
            rbs = bridgeDown[countDown].GetComponentsInChildren<Rigidbody>();
            if(countDown == tremor)
            {
                player.camera_shake = true;
                tremor += 2;

            }

            foreach (Rigidbody rb in rbs)
            {

                rb.isKinematic = false;
                rb.AddExplosionForce(300, bridgeDown[countDown].transform.right, 100f, 2.00F);
              
           
            }
         //   rbs[0].AddForce(transform.right * 0.1f);

            countDown++;
            timerDown = timerCd;
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            sound.SetActive(true);
            source.Play();
            startDown = true;
        }
    }
}
