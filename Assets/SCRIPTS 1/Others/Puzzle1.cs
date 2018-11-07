using UnityEngine;
using System.Collections;

public class Puzzle1 : MonoBehaviour
{
    public GameObject tourch;
    private bool burn = false;
    public GameObject flame;

	// Update is called once per frame
	void Update ()
    {
	    if(burn && tourch.GetComponent<Tourch>().fireOn)
        {
            flame.SetActive(true);
        }
        if(!tourch.GetComponent<Tourch>().fireOn)
        {
            burn = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            burn = true;
        }
    }
}
