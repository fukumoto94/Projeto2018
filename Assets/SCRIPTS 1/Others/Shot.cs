using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    float timer;
    RPGCharacterControllerFREE player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
    }

    void Update()
    {
        if (timer >= 2f)
        {
            Destroy(this.gameObject);
        }

    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "EnemyWeakness")
        {
            Destroy(this.gameObject);
        }
    }

}
