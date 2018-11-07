using UnityEngine;
using System.Collections;

public class BossWeak : MonoBehaviour
{
    public string obj_weak;
    private Boss boss;
	// Use this for initialization
	void Awake ()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>() as Boss;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Arrow")
        {
            if(obj_weak == "LeftH")
            {
                boss.leftH_hit++;
            }
            else if (obj_weak == "RightH")
            {
                boss.rightH_hit++;
            }
            else if(obj_weak == "Head")
            {
                boss.head_hit++;
            }
        }
    }
}
