using UnityEngine;
using System.Collections;

public class SlotMagicDoor : MonoBehaviour
{
    public bool orbOn = false;
    private float timer = 2f;
    public AudioSource source;
    private bool playOne;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Orb")
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            timer -= Time.deltaTime;

            if(!playOne)
            {
                source.Play();
                playOne = true;
            }

            if(timer<0)
            {
                orbOn = true;
            }
        }
    }
}
