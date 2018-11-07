using UnityEngine;
using System.Collections;

public class ProtectCamera : MonoBehaviour
{    
    public Transform m_Cam;
    private bool isBlocking = false;

    void Awake()
    {

    }
    void Start()
    {
       // Physics.IgnoreCollision(m_Cam.GetComponent<Collider>(), GetComponent<Collider>());

    }
    void Update()
    {

        if (isBlocking)
        {
            if(m_Cam.GetComponent<OrbitCamera>()._distance >= -5)
            {
                m_Cam.GetComponent<OrbitCamera>()._distance -= 0.5f;
            }
        }
    }
    void LateUpdate()
    {
      
    }
    void OnTriggerStay(Collider col)
    {
        if(col.transform.tag == "Wall")
        {
            isBlocking = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Wall")
        {
            isBlocking = false;
        }
    }
}