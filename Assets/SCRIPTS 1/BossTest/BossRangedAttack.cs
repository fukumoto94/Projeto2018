using UnityEngine;
using System.Collections;

public class BossRangedAttack : MonoBehaviour {

    public int speed = 3;

    void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.GetComponent<RPGCharacterControllerFREE>().GetDamage(5);
            col.GetComponent<RPGCharacterControllerFREE>().GetHit();
            Destroy(this.gameObject);

        }


    }
}
