using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private float timer;

    private void Update()
    {
       
       

        timer += Time.deltaTime;

        if(timer > 6f)
        {
            Destroy(gameObject);
        }
    }


}
