using UnityEngine;
using System.Collections;

public class SceneTemple : MonoBehaviour
{
	public bool fade;
    public bool temple2;
    public GameObject[] Doors;
    public GameObject[] enemies;
    public GameObject giant;
    public GameObject[] enemyHorda;
    private GameObject[] restHorda;
    public GameObject miniMap;
    public GameObject compass;
    public GameObject orb;
    private bool auxBol =false;
    private int auxInt = 0;
    private bool isEnemies;
    private bool startFight = false;
    public GameObject lastEnemies;
    public AudioSource source;
    public bool startGolem;
    public GameObject doorEnd;
    private OrbitCamera oc;
    private bool activeCam;
    private void Awake()
    {
        oc = GameObject.FindGameObjectWithTag("OrbitCamera").GetComponent<OrbitCamera>() as OrbitCamera;

        source = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update ()
    {
        enemies = GameObject.FindGameObjectsWithTag("EnemyM");

        if(temple2)
        {
            if(giant == null)
            {
                source.Stop();

                if(!activeCam)
                {
                    oc.orbOn = true;
                    activeCam = true;
                }
                orb.SetActive(true);
            }
            else
            {
                orb.SetActive(false);
            }

            if (enemyHorda[0].transform.childCount <= 0)
            {
                if(!startGolem)
                {
                    source.Stop();


                }
                Doors[0].GetComponent<CloseDoor>().close = false;
                Doors[1].GetComponent<CloseDoor>().close = false;
            }
        }
        else
        {
            if(enemies.Length <= 0)
            {
                source.Stop();


                doorEnd.GetComponent<CloseDoor>().close = true;
            }
            else
            {
                doorEnd.GetComponent<CloseDoor>().close = false;

            }
            if (auxBol && auxInt <= 2)
            {
                if (enemies.Length <= enemies.Length / 2)
                {
                    enemyHorda[1].SetActive(true);

                    if (miniMap != null)
                    {
                        source.Stop();

                        miniMap.SetActive(true);
                    }

                    startFight = false;
                    GetComponent<Collider>().enabled = true;
                    if (auxInt >= 1)
                    {
                        if (compass != null)
                        {
                            source.Stop();
						
                            compass.SetActive(true);
                        }
                        Doors[auxInt + 1].GetComponent<CloseDoor>().close = startFight;

                    }
                    Doors[auxInt].GetComponent<CloseDoor>().close = startFight;
                    auxInt++;
                    auxBol = false;

                }
            }

            if(auxInt > 1 && !auxBol)
            {
                lastEnemies.SetActive(true);
            }
        }


        


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!temple2)
            {
                source.Play();
                if(auxInt < 2)
                {
                    enemyHorda[auxInt].SetActive(true);
                    startFight = true;
                    auxBol = true;
                    GetComponent<Collider>().enabled = false;


                    transform.position = new Vector3(73, -4.12f, -83.3f);

                    if (auxInt >= 1)
                    {
                        Doors[auxInt + 1].GetComponent<CloseDoor>().close = startFight;

                    }
                    Doors[auxInt].GetComponent<CloseDoor>().close = startFight;
                    transform.GetComponent<BoxCollider>().size = new Vector3(transform.GetComponent<BoxCollider>().size.x, 804f, transform.GetComponent<BoxCollider>().size.z);
                }
              
            }
            else
            {
                source.Play();
                Doors[0].GetComponent<CloseDoor>().close = true;
                Doors[1].GetComponent<CloseDoor>().close = true;
                GetComponent<Collider>().enabled = false;

            }


        }
    }


}
