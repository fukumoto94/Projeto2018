using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleOnCamera : MonoBehaviour {

    private void Update()
    {


        if (GetComponent<Renderer>().isVisible)
        {
            Debug.Log("visivel");

        }
        else
        {
            Debug.Log("invi");


        }
    }

}
