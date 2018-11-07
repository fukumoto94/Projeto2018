using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour
{
    public GameObject point;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Mirror")
        {
            other.GetComponent<MirrorRotate>().canRot = false;
            point.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mirror")
        {
            point.SetActive(false);
        }
    }
}
