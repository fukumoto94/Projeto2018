using UnityEngine;
using System.Collections;

public class targetBowTest : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        Destroy(this.gameObject);
    }
}
