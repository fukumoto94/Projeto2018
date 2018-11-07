using UnityEngine;
using System.Collections;

public class TargetWeakness : MonoBehaviour
{

    private int hit_count = 0;
    public int hitToDestroy;
	// Update is called once per frame
	void Update ()
    {
       if(hit_count >= hitToDestroy)
        {
            Destroy(gameObject);
        }
       // Debug.Log(hit_count);
    }

    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Weapon")
        {
            hit_count++;
        }
    }
}
