using UnityEngine;
using System.Collections;

public class MyArrow : MonoBehaviour
{
    public float speed = 5;
    public static RaycastHit hit;

    void Update ()
    {
        //this.transform.Translate(Vector3.forward * speed * Time.deltaTime);]
        //  this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2f))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Destroy(this.gameObject);

    }


}
