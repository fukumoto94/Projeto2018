using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public GameObject hit;
    public Animator anim;
    private bool hitted;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Untagged" && !hitted)
        {
            anim.SetTrigger("Solid");
            hit.SetActive(true);
            hitted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            hit.SetActive(false);
            hitted = false;
        }
    }

}
