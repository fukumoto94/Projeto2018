using UnityEngine;
using System.Collections;

public class Raycast3 : MonoBehaviour
{
    public static bool colideRight;
    public static bool colideLeft;

    public static RaycastHit hit;
    private Vector3 distance;
    public Transform target;
    public bool isRight;
 //   public LineRenderer laserLineRenderer;

    Vector3 start, end;

    void Update()
    {
        distance = target.position - transform.position;

        //  start = transform.position;
        //end = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance.magnitude))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
           // Debug.Log("Coldindo" + hit.transform.tag);
            if (hit.transform.tag == "Untagged")

            // if (hit.transform.tag != "CameraController" && hit.transform.tag != "Floor" && hit.transform.tag != "Player" && hit.transform.tag != "Weapon" && hit.transform.tag != "EnemyM" && hit.transform.tag != "EnemyP" && hit.transform.tag != "EnemyG" && hit.transform.tag != "SceneController" && hit.transform.tag != "BridgeStart")
            {

                if (isRight)
                {
                    colideRight = true;
                }
                else
                {
                    colideLeft = true;
                }
            }
        
        }
        else
        {
            if (isRight)
            {
                colideRight = false;
            }
            else
            {
                colideLeft = false;
            }
        }
      //  Debug.Log(colideRight+"  R   L  "+colideLeft);
        // laserLineRenderer.SetPosition(0, start);
        //laserLineRenderer.SetPosition(1, end);
       // Debug.Log(colideLeft+"   LEFT    RIGHT"+colideRight);

    }
}
