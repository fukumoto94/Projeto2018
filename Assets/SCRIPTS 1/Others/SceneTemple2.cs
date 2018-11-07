using UnityEngine;
using System.Collections;

public class SceneTemple2 : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject[] doorsBow;
    private bool startFight = false;
    public GameObject[] enemies;
    private GameObject[] enemy;
    private RPGCharacterControllerFREE player;
    public GameObject bow;
    public GameObject crashRock;
    public GameObject[] flames;
    public GameObject exit;
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(flames[0].activeSelf && flames[1].activeSelf)
        {
            exit.SetActive(true);
        }

	    if(startFight)
        {
            enemy = GameObject.FindGameObjectsWithTag("EnemyM");

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].GetComponent<CloseDoor>().close = true;
            }

            if(enemy.Length == 0)
            {
                startFight = false;
            }
        }
        else
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].GetComponent<CloseDoor>().close = false;
            }
        }

        if(bow == null)
        {
            for (int i = 0; i < doorsBow.Length; i++)
            {
                doorsBow[i].GetComponent<CloseDoor>().close = true;
            }
            crashRock.GetComponent<Rigidbody>().isKinematic = false;
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if ((player.transform.position - enemies[0].transform.position).magnitude > (player.transform.position - enemies[1].transform.position).magnitude)
            {
                enemies[1].SetActive(true);
            }
            else
            {
                enemies[0].SetActive(true);
            }
            startFight = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
