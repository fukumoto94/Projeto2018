using UnityEngine;
using System.Collections;

public class WallDestructive : MonoBehaviour
{
    public int hits;
    private int hit;
    public GameObject poof;
    private float timer = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hit >= (hits - 1))
        {
            poof.SetActive(true);
        }

	    if(hit >= hits)
        {
            GetComponent<MeshRenderer>().enabled = false;
            
            timer -= Time.deltaTime;

            if(timer < 0)
            {

                Destroy(gameObject);

            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        hit++;
    }
}
