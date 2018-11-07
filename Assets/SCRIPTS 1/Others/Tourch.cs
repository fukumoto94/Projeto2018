using UnityEngine;
using System.Collections;

public class Tourch : MonoBehaviour
{
    private float timer;
    public bool fireOn;

	// Update is called once per frame
	void Update ()
    {
	    if(timer >= 0)
        {
            fireOn = true;
            timer -= Time.deltaTime;
        }
        else
        {
            fireOn = false;
        }
	}
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            timer = 10f;
        }
    }
}
