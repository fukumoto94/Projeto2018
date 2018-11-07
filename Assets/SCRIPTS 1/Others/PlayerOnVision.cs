using UnityEngine;
using System.Collections;

public class PlayerOnVision : MonoBehaviour
{
    public GameObject enemy;
    private GameObject[] arrow;
    void Awake()
    {
    }
    private void Update()
    {
        arrow = GameObject.FindGameObjectsWithTag("Arrow");
        //Physics.IgnoreCollision(arrow.GetComponent<Collider>(), GetComponent<Collider>());

 
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            enemy.GetComponent<EnemyM>().stopAttack = false;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            enemy.GetComponent<EnemyM>().stopAttack = true ;
        }
    }
}
