using UnityEngine;
using System.Collections;

public class GetMouseMove : MonoBehaviour
{

    // calculation variables
    float current_x = 0.0f;


    // states
    bool x_changed = false;


    // string references to speed string manipulation/calculation
    string x_axis = "Horizontal";


    //initialize your variables here
    void Awake()
    {
        // initialize your current x with the current change in x axis
        current_x = Input.GetAxis("Mouse X");
    }


    void Update()
    {


        if (DidMove())
        {
           // Debug.Log("Mouse moved horizontally.");
        }

    }


    bool DidMove()
    {
        // get the current change in horizontal mouse movement
        current_x = Input.GetAxis("Mouse X");


        // check if the current x has a value of 0. if not, return true;
        if (current_x == 0)
            return false;
        else
            return true;

    }

}
