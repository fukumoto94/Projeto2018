using UnityEngine;
using System.Collections;

public class DardosInstantiate : MonoBehaviour
{
    public GameObject dardoPrefab;
    private bool dardoOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !dardoOn)
        {
            Instantiate(dardoPrefab, transform.position, transform.rotation);
            dardoOn = true;
        }
    }
}
