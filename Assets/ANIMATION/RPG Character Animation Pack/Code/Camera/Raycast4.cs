using UnityEngine;
using System.Collections;

public class Raycast4 : MonoBehaviour
{
    public static bool colide;

    public static RaycastHit hit;
    private string obj_tag;

    void Update()
    {

        //  start = transform.position;
        //end = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f))
        {
            if (hit.transform.tag != "CameraController" && hit.transform.tag != "Floor" && hit.transform.tag != "Player" && hit.transform.tag != "Weapon" && hit.transform.tag != "EnemyM" && hit.transform.tag != "EnemyP" && hit.transform.tag != "EnemyG" && hit.transform.tag != "SceneController")
            {
               // colide = true;
            }
        }
        else
        {


           // colide = false;
        }

    }

    private void OnTriggerStay(Collider hit)
    {

        if (hit.transform.tag != "ClimbOut" && hit.transform.tag != "CameraController" && hit.transform.tag != "Floor" && hit.transform.tag != "Player" && hit.transform.tag != "Weapon" && hit.transform.tag != "EnemyM" && hit.transform.tag != "EnemyP" && hit.transform.tag != "EnemyG" && hit.transform.tag != "SceneController")
        {
            colide = true;
        }

    }
    private void OnTriggerExit(Collider hit)
    {
        if (hit.transform.tag != "ClimbOut" && hit.transform.tag != "CameraController" && hit.transform.tag != "Floor" && hit.transform.tag != "Player" && hit.transform.tag != "Weapon" && hit.transform.tag != "EnemyM" && hit.transform.tag != "EnemyP" && hit.transform.tag != "EnemyG" && hit.transform.tag != "SceneController")
        {
            colide = false;
        }
    }
}