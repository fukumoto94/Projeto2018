using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaliAudio : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Untagged")
        {
            source.Play();
        }
    }

}
