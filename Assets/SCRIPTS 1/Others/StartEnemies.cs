using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemies : MonoBehaviour
{
    public GameObject enemie;
    private SceneTemple st;

    private void Awake()
    {
        st = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneTemple>() as SceneTemple;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            st.startGolem = true;
            st.source.Play();
            enemie.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
