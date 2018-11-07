using UnityEngine;
using System.Collections;

public class SceneXibalba : MonoBehaviour
{
    private int stage = -1;
    public GameObject[] enemies;
    private GameObject[] enemy;
    private GameObject[] enemyG;
 
    private bool nextStage = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (stage)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                enemies[0].SetActive(true);
                break;
            case 3:
                break;
            case 4:
                enemies[1].SetActive(true);

                break;
            case 5:
                break;
            case 6:
                enemies[2].SetActive(true);

                break;
            case 7:
                break;

        }
        enemy = GameObject.FindGameObjectsWithTag("EnemyM");
        enemyG = GameObject.FindGameObjectsWithTag("EnemyG");

        if (stage > 1)
        {
            if (enemy.Length > 0 || enemyG.Length > 0)
            {
                nextStage = false;
            }
            else
            {
                nextStage = true;
            }
        }
   

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && nextStage)
        {
            if(stage <= 8)
            {
                stage++;
            }
            transform.position = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);
        }
    }
}
