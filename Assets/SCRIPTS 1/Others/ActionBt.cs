using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBt : MonoBehaviour
{
    public GameObject actionBt;
    public bool climb;
    
    private void OnTriggerStay(Collider other)
    {
        if(climb)
        {
            if(other.gameObject.tag == "Player")
            {
                if (other.GetComponent<RPGCharacterControllerFREE>().transform.position.y <= 1.5f)
                {
                    actionBt.SetActive(true);

                }
            }
        
        }
        else
        {
            if (other.gameObject.tag == "Player")
            {
                actionBt.SetActive(true);

            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        actionBt.SetActive(false);
    }
}
